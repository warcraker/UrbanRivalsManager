using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;

using Procurios.Public;
using UrbanRivalsApiAdapter;
using UrbanRivalsApiManager;
using UrbanRivalsCore.Model;

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
        private IDatabaseManager DatabaseManagerInstance;
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
        public GlobalManager(ApiManager apiManager, IDatabaseManager databaseManager, ImageDownloader imageManager, InMemoryManager inMemoryManager, ServerQueriesManager serverQueriesManager)
            : this()
        {
            if (apiManager == null)
                throw new ArgumentNullException(nameof(apiManager));
            if (databaseManager == null)
                throw new ArgumentNullException(nameof(databaseManager));
            if (imageManager == null)
                throw new ArgumentNullException(nameof(imageManager));
            if (inMemoryManager == null)
                throw new ArgumentNullException(nameof(inMemoryManager));
            if (serverQueriesManager == null)
                throw new ArgumentNullException(nameof(serverQueriesManager));

            ApiManagerInstance = apiManager;
            DatabaseManagerInstance = databaseManager;
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
                (this, DatabaseManagerInstance, InMemoryManagerInstance, ImageDownloaderInstance, ServerQueriesManagerInstance);
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
            var storedIds = DatabaseManagerInstance.getAllCardBaseIds();
            return existingIds.Except(storedIds);
        }

        private static void UpdateCardBaseDefinitions_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            Debug.Assert(e.Argument != null);
            UpdateCardBaseDefinitionsArgument managers = (UpdateCardBaseDefinitionsArgument)e.Argument;

            worker.ReportProgress(0, "Connecting to server...");

            // This process will set the locale to English. Here we take the previous value to restore it at the end.
            string userLocale = managers.ServerQueriesManager.GetUserLocale();

            var existingIds = managers.ServerQueriesManager.GetAllCardBaseIds();
            var storedIds = managers.DatabaseManager.getAllCardBaseIds();
            var idsToAnalyze = existingIds.Except(storedIds).ToList();
            idsToAnalyze.Sort();

            /* There are 3 API calls to get characters details:
             * - characters.getCharacters (labeled "oldGetCharacters")
             * - characters.getCharacterLevels
             * - urc.getCharacters (labeled "newGetCharacters")
             * Some details can be obtained from various of those calls, some only from one of them.
             * "newGetCharacters" is the fastest one, so we will get most of the details from that one, and the rest from "oldGetCharacters".
             * "getCharacterLevels" will not be used, all the details that we can obtain from it we can get it from the "new" one too.
             */

            var setLocaleToEnglishCall = new ApiCallList.Players.SetLanguages(new List<string> { ServerQueriesManager.EnglishLocale });
            var request = new ApiRequest(setLocaleToEnglishCall);
            var oldGetCharactersCall = new ApiCallList.Characters.GetCharacters
            {
                // This call is painfully slow, so we optimize it as much as we can, asking only for the info we need 
                charactersIDs = new List<int>(idsToAnalyze),
                maxLevels = true,
                ItemsFilter = new List<string>() { "id", "ability", "ability_unlock_level" },
            };
            request.EnqueueApiCall(oldGetCharactersCall);
            var newGetCharactersCall = new ApiCallList.Urc.GetCharacters();
            request.EnqueueApiCall(newGetCharactersCall);

            string response;
            HttpStatusCode statusCode = managers.GlobalManager.ApiManagerInstance.SendRequest(request, out response);
            if (statusCode != HttpStatusCode.OK)
                throw new Exception("GetCardBase SendRequest returned: " + statusCode.ToString()); // TODO: Manage timeouts gracefully (GatewayTimeout)
            dynamic decoded = JsonDecoder.Decode(response);

            int progress = 0;

            // TODO: Check if new data returns id, ability and ability_unlock_level
            var oldData = SortServerCharacterDataIntoDictionary(oldGetCharactersCall.Call, decoded);
            var newData = SortServerCharacterDataIntoDictionary(newGetCharactersCall.Call, decoded);

            foreach (int id in idsToAnalyze)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }

                string abilityText = oldData[id]["ability"].ToString();
                int abilityUnlockLevel = int.Parse(oldData[id]["ability_unlock_level"].ToString());

                string name = newData[id]["name"].ToString();
                int clanId = int.Parse(newData[id]["clanID"].ToString());
                string rarityText = newData[id]["rarity"].ToString();

                var cardLevels = new List<CardLevel>();
                foreach(dynamic levelItem in newData[id]["levels"])
                {
                    int level = int.Parse(levelItem["level"].ToString());
                    int power = int.Parse(levelItem["power"].ToString());
                    int damage = int.Parse(levelItem["damage"].ToString());
                    cardLevels.Add(new CardLevel(level, power, damage));
                }

                worker.ReportProgress((int)(100 * progress / idsToAnalyze.Count()), $"[{id}] {name}");
                progress++;

                var card = ApiToCardBaseAdapter.ToCardBase(id, name, clanId, rarityText, abilityText, abilityUnlockLevel, cardLevels);

                if (card == null) continue; // TODO: Remove this line if day/night is implemented, or after 11/2018, whatever happens first

                managers.DatabaseManager.storeCardBase(card);
                managers.InMemoryManager.LoadToMemoryCardBase(card);
                managers.ImageDownloader.AddCardBaseToDownloadQueue(card, CharacterImageFormat.Color800x640);
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
                    CardBase cardBase;
                    CardInstance cardInstance;

                    cardBaseId = int.Parse(item["id"].ToString());
                    cardBase = managers.InMemoryManager.GetCardBase(cardBaseId);
                    cardInstanceId = int.Parse(item["id_player_character"].ToString());
                    level = int.Parse(item["level"].ToString());
                    experience = int.Parse(item["xp"].ToString());
                    cardInstance = CardInstance.createCardInstance(cardBase, cardInstanceId, level, experience);

                    instances.Add(cardInstance);
                }

                page++;
            } 

            managers.InMemoryManager.ReloadToMemoryCardInstances(instances);

            worker.ReportProgress(100);
        }

        private static Dictionary<int, dynamic> SortServerCharacterDataIntoDictionary(string apiCallString, dynamic decodedData)
        {
            var result = new Dictionary<int, dynamic>();
            foreach (dynamic item in decodedData[apiCallString]["items"])
                result.Add(int.Parse(item["id"].ToString()), item);
            return result;
        }
    }

    internal class UpdateCardBaseDefinitionsArgument
    {
        public GlobalManager GlobalManager;
        public IDatabaseManager DatabaseManager;
        public InMemoryManager InMemoryManager;
        public ImageDownloader ImageDownloader;
        public ServerQueriesManager ServerQueriesManager;

        public UpdateCardBaseDefinitionsArgument
            (GlobalManager globalManager, IDatabaseManager databaseManager, InMemoryManager inMemoryManager, 
            ImageDownloader imageDownloader, ServerQueriesManager serverQueriesManager)
        {
            GlobalManager = globalManager;
            DatabaseManager = databaseManager;
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
