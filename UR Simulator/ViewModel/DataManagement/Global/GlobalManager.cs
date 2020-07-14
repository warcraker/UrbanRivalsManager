using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;

using Procurios.Public;
using UrbanRivalsApiToCoreAdapter;
using UrbanRivalsApiManager;
using UrbanRivalsCore.Model;
using UrbanRivalsCore.Model.Cards.Skills;
using System.Text;
using System.Windows;
using Warcraker.Utils;
using Warcraker.UrbanRivals.DataRepository;
using Warcraker.UrbanRivals.ORM;

namespace UrbanRivalsManager.ViewModel.DataManagement
{
    public enum CardBaseDefinitionsProgressStep
    {
        GettingValidIds,
        DownloadingCards,
    }

    public class CardBaseDefinitionsProgress
    {
        public CardBaseDefinitionsProgressStep Step { get; set; }
        public int CurrentIteration { get; set; }
        public int TotalIterations { get; set; }
    }

    public class GlobalManager
    {
        private ApiManager ApiManagerInstance;
        private InMemoryManager InMemoryManagerInstance;
        private ImageDownloader ImageDownloaderInstance;
        private ServerQueriesManager ServerQueriesManagerInstance;

        private BackgroundWorker UpdateCardBaseDefinitionsBGW;
        private BackgroundWorker UpdateCardInstanceDefinitionsBGW;

        private static BackgroundWorker prv_getNewBackgroundWorker()
        {
            BackgroundWorker worker;

            worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            return worker;
        }

        private GlobalManager()
        {
            UpdateCardBaseDefinitionsBGW = prv_getNewBackgroundWorker();
            UpdateCardBaseDefinitionsBGW.DoWork += GlobalManager.UpdateCardBaseDefinitions_DoWork;

            UpdateCardInstanceDefinitionsBGW = prv_getNewBackgroundWorker();
            UpdateCardInstanceDefinitionsBGW.DoWork += GlobalManager.UpdateCardInstanceDefinitions_DoWork;

            // None of these is going to be called more than 1-2 times per second. 
            // Using empty delegates makes code cleaner and the performance penalty is negligible.
            UpdateCardBaseDefinitionsBGW.ProgressChanged += delegate { };
            UpdateCardBaseDefinitionsBGW.RunWorkerCompleted += delegate { };
            UpdateCardInstanceDefinitionsBGW.ProgressChanged += delegate { };
            UpdateCardInstanceDefinitionsBGW.RunWorkerCompleted += delegate { };
        }
        public GlobalManager(ApiManager apiManager, ImageDownloader imageManager, InMemoryManager inMemoryManager, ServerQueriesManager serverQueriesManager)
            : this()
        {
            if (apiManager == null)
                throw new ArgumentNullException(nameof(apiManager));
            if (imageManager == null)
                throw new ArgumentNullException(nameof(imageManager));
            if (inMemoryManager == null)
                throw new ArgumentNullException(nameof(inMemoryManager));
            if (serverQueriesManager == null)
                throw new ArgumentNullException(nameof(serverQueriesManager));

            ApiManagerInstance = apiManager;
            ImageDownloaderInstance = imageManager;
            InMemoryManagerInstance = inMemoryManager;
            ServerQueriesManagerInstance = serverQueriesManager;
        }

        public event ProgressChangedEventHandler UpdateCardBaseDefinitions_ProgressChanged
        {
            add { UpdateCardBaseDefinitionsBGW.ProgressChanged += value; }
            remove { UpdateCardBaseDefinitionsBGW.ProgressChanged -= value; }
        }
        public event RunWorkerCompletedEventHandler UpdateCardBaseDefinitions_RunWorkerCompleted
        {
            add { UpdateCardBaseDefinitionsBGW.RunWorkerCompleted += value; }
            remove { UpdateCardBaseDefinitionsBGW.RunWorkerCompleted -= value; }
        }
        public event ProgressChangedEventHandler UpdateCardInstanceDefinitions_ProgressChanged
        {
            add { UpdateCardInstanceDefinitionsBGW.ProgressChanged += value; }
            remove { UpdateCardInstanceDefinitionsBGW.ProgressChanged -= value; }
        }
        public event RunWorkerCompletedEventHandler UpdateCardInstanceDefinitions_RunWorkerCompleted
        {
            add { UpdateCardInstanceDefinitionsBGW.RunWorkerCompleted += value; }
            remove { UpdateCardInstanceDefinitionsBGW.RunWorkerCompleted -= value; }
        }

        public void UpdateCardBaseDefinitionsAsync()
        {
            if (UpdateCardBaseDefinitionsBGW.IsBusy)
                throw new Exception("Can't call to UpdateCardBaseDefinitions until the previous execution finishes");
            var argument = new UpdateCardBaseDefinitionsArgument
                (this, InMemoryManagerInstance, ImageDownloaderInstance, ServerQueriesManagerInstance);
            UpdateCardBaseDefinitionsBGW.RunWorkerAsync(argument);
        }
        public void UdpateCardInstanceDefinitionsAsync()
        {
            if (UpdateCardInstanceDefinitionsBGW.IsBusy)
                throw new Exception("Can't call to UpdateCardInstanceDefinitionsBGW until the previous execution finishes");
            var argument = new UpdateCardInstanceDefinitionsArgument(ApiManagerInstance, InMemoryManagerInstance);
            UpdateCardInstanceDefinitionsBGW.RunWorkerAsync(argument);
        }

        public IEnumerable<int> GetAllCardBaseIdsNotInDatabase()
        {
            var existingIds = ServerQueriesManagerInstance.GetAllCardBaseIds();
            return existingIds;
        }

        private static void UpdateCardBaseDefinitions_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            Debug.Assert(e.Argument != null);
            UpdateCardBaseDefinitionsArgument managers = (UpdateCardBaseDefinitionsArgument)e.Argument;

            worker.ReportProgress(0, "Initialize database...");

            GameDataRepository repo = new GameDataRepository(@"C:\repo.db");
            BlobProcessor processor = new BlobProcessor(repo);

            worker.ReportProgress(0, "Connecting to server...");

            // This process will set the locale to English. Here we take the previous value to restore it at the end.
            string userLocale = managers.ServerQueriesManager.GetUserLocale();

            /* There are 3 API calls to get characters details:
             * - characters.getCharacters (labeled "oldGetCharacters") This call is painfully slow, so we ask only for the info we need 
             * - characters.getCharacterLevels
             * - urc.getCharacters (labeled "newGetCharacters")
             * Some details can be obtained from various of those calls, some only from one of them.
             * "newGetCharacters" is the fastest one, so we will get most of the details from that one, and the rest from "oldGetCharacters".
             * "getCharacterLevels" will not be used, all the details that we can obtain from it we can get it from the "new" one too.
             */

            ApiCall setLocaleToEnglishCall = new ApiCallList.Players.SetLanguages(
                new List<string> { "en" }
            );
            ApiCall getClansCall = new ApiCallList.Characters.GetClans
            {
                ItemsFilter = new List<string> { "id", "name", "bonusDescription" },
            };
            ApiCall getAbilitiesCall = new ApiCallList.Characters.GetCharacters
            {
                maxLevels = true,
                ItemsFilter = new List<string> { "id", "ability", "ability_unlock_level" },
            };
            ApiCall getCharactersCall = new ApiCallList.Urc.GetCharacters
            {
                ItemsFilter = new List<string> { "id", "name", "clanID", "rarity", "levels.level", "levels.power", "levels.damage" },
            };

            ApiRequest request = new ApiRequest(setLocaleToEnglishCall);
            request.EnqueueApiCall(getAbilitiesCall);
            request.EnqueueApiCall(getCharactersCall);
            request.EnqueueApiCall(getClansCall);

            string response;
            HttpStatusCode statusCode = managers.GlobalManager.ApiManagerInstance.SendRequest(request, out response);
            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    break;
                case HttpStatusCode.GatewayTimeout:
                    MessageBox.Show("GatewayTimeout");
                    worker.ReportProgress(0, "Timeout");
                    return; // TODO manage somewhat
                default:
                    throw new Exception("SendRequest returned: " + statusCode.ToString()); // TODO manage somewhat
            }

            // TODO hash response. 
            dynamic decoded = JsonDecoder.Decode(response);

            int progress = 0;

            // TODO: Check if new data returns id, ability and ability_unlock_level
            Dictionary<int, dynamic> clansData = SortServerCharacterDataIntoDictionary(getClansCall.Call, decoded);
            List<int> clanIds = ExtractIds(getClansCall.Call, decoded);
            int leaderClanId = -1;
            foreach (int id in clanIds)
            {
                dynamic clanData = clansData[id];

                // id
                // name
                // clanPictUrl
                // clanNewPictUrl
                // description
                // bonusDescription
                // bonusLongDescription

                string bonusText = clanData["bonusDescription"].ToString();
                if (bonusText == "Cancel Leader")
                {
                    Asserts.Check(leaderClanId == -1, "More than one clan with 'Cancel Leader'");
                    leaderClanId = id;
                }
                else
                {
                    ; // TODO 
                }
            }

            Dictionary<int, dynamic> abilitiesData = SortServerCharacterDataIntoDictionary(getAbilitiesCall.Call, decoded);
            Dictionary<int, dynamic> charactersData = SortServerCharacterDataIntoDictionary(getCharactersCall.Call, decoded);

            foreach (int id in charactersData.Keys)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }

                string abilityText = abilitiesData[id]["ability"].ToString();
                int abilityUnlockLevel = int.Parse(abilitiesData[id]["ability_unlock_level"].ToString());

                string name = charactersData[id]["name"].ToString();
                int clanId = int.Parse(charactersData[id]["clanID"].ToString());
                string rarityText = charactersData[id]["rarity"].ToString();

                List<CardStats> cardStatsPerLevel = new List<CardStats>();
                foreach (dynamic levelItem in charactersData[id]["levels"])
                {
                    int level = int.Parse(levelItem["level"].ToString());
                    int power = int.Parse(levelItem["power"].ToString());
                    int damage = int.Parse(levelItem["damage"].ToString());
                    cardStatsPerLevel.Add(new CardStats(level, power, damage));
                }

                worker.ReportProgress((int)(100 * progress / charactersData.Count()), $"[{id}] {name}");
                progress++;

                UrbanRivalsCore.Model.Cards.Skills.OldSkill newAbility = OldSkillParser.parseSkill(abilityText);

                continue; // TODO REMOVE

                // TODO Inverse OPP on Backlash and Defeat

                var card = ApiToCardDefinitionAdapter.createCardDefinitionByServerData(id, name, clanId, rarityText, abilityText, abilityUnlockLevel, cardStatsPerLevel);

                managers.InMemoryManager.LoadToMemoryCardDefinition(card);
                managers.ImageDownloader.AddCardDefinitionToDownloadQueue(card, CharacterImageFormat.Color800x640);
            }

            managers.ServerQueriesManager.SetUserLocale(userLocale);
            worker.ReportProgress(100);
        }
        private static void UpdateCardInstanceDefinitions_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            UpdateCardInstanceDefinitionsArgument managers = (UpdateCardInstanceDefinitionsArgument)e.Argument;

            worker.ReportProgress(0);

            var getCardsFromCollectionPageCall = new ApiCallList.Collections.GetCollectionPage
            {
                nbPerPage = 52, // Maximum allowed by the API
                ItemsFilter = new List<string>() { "id", "id_player_character", "level", "xp" },
                ContextFilter = new List<string>() { "totalPages" }
            };

            var request = new ApiRequest(getCardsFromCollectionPageCall);

            int page = 0;
            int totalPages = 1;
            // First valid page = 0. Last valid page = totalPages - 1
            bool firstIteration = true;
            var instances = new List<CardInstance>();

            while (page < totalPages)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    return;
                }

                getCardsFromCollectionPageCall.page = page;

                string response;
                HttpStatusCode statusCode = managers.ApiManager.SendRequest(request, out response);
                if (statusCode != HttpStatusCode.OK)
                    throw new Exception("GetCardInstancesFromSummary returned: " + statusCode.ToString());
                dynamic decoded = JsonDecoder.Decode(response);

                if (firstIteration)
                {
                    dynamic context = decoded[getCardsFromCollectionPageCall.Call]["context"];
                    totalPages = int.Parse(context["totalPages"].ToString());
                    getCardsFromCollectionPageCall.ContextFilter.Remove("totalPages");
                    firstIteration = false;
                }
                worker.ReportProgress(100 * page / totalPages); // totalPages can't be zero, a player can't sell cards on their current deck

                dynamic items = decoded[getCardsFromCollectionPageCall.Call]["items"];
                foreach (dynamic item in items)
                {
                    int cardBaseId;
                    int cardInstanceId;
                    int level;
                    int experience;
                    CardDefinition cardDefinition;
                    CardInstance cardInstance;

                    cardBaseId = int.Parse(item["id"].ToString());
                    cardDefinition = managers.InMemoryManager.GetCardDefinition(cardBaseId);
                    cardInstanceId = int.Parse(item["id_player_character"].ToString());
                    level = int.Parse(item["level"].ToString());
                    experience = int.Parse(item["xp"].ToString());
                    cardInstance = CardInstance.createCardInstance(cardDefinition, cardInstanceId, level, experience);

                    instances.Add(cardInstance);
                }

                page++;
            }

            managers.InMemoryManager.ReloadToMemoryCardInstances(instances);

            worker.ReportProgress(100);
        }

        private static List<int> ExtractIds(string apiCallString, dynamic decodedData)
        {
            List<int> decodedIds = new List<int>();
            foreach (dynamic item in decodedData[apiCallString]["items"])
            {
                int id = int.Parse(item["id"].ToString());
                decodedIds.Add(id);
            }

            return decodedIds;
        }

        private static Dictionary<int, dynamic> SortServerCharacterDataIntoDictionary(string apiCallString, dynamic decodedData)
        {
            Dictionary<int, dynamic> result = new Dictionary<int, dynamic>();
            foreach (dynamic item in decodedData[apiCallString]["items"])
            {
                int id = int.Parse(item["id"].ToString());
                result.Add(id, item);
            }
            return result;
        }
    }

    internal class UpdateCardBaseDefinitionsArgument
    {
        public GlobalManager GlobalManager;
        public InMemoryManager InMemoryManager;
        public ImageDownloader ImageDownloader;
        public ServerQueriesManager ServerQueriesManager;

        public UpdateCardBaseDefinitionsArgument
            (GlobalManager globalManager, InMemoryManager inMemoryManager,
            ImageDownloader imageDownloader, ServerQueriesManager serverQueriesManager)
        {
            GlobalManager = globalManager;
            InMemoryManager = inMemoryManager;
            ImageDownloader = imageDownloader;
            ServerQueriesManager = serverQueriesManager;
        }
    }
    internal class UpdateCardInstanceDefinitionsArgument
    {
        public ApiManager ApiManager;
        public InMemoryManager InMemoryManager;

        public UpdateCardInstanceDefinitionsArgument(ApiManager apiManager, InMemoryManager inMemoryManager)
        {
            ApiManager = apiManager;
            InMemoryManager = inMemoryManager;
        }
    }
}
