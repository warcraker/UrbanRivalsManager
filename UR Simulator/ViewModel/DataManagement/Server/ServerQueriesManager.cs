using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Procurios.Public;
using UrbanRivalsApiManager;
using UrbanRivalsCore.Model;
using UrbanRivalsApiToCoreAdapter;

namespace UrbanRivalsManager.ViewModel.DataManagement
{
    public class ServerQueriesManager
    {
        public static readonly string EnglishLocale = "en";

        private ApiManager ApiManagerInstance;
        private InMemoryManager InMemoryManagerInstance;

        private ServerQueriesManager() { }
        public ServerQueriesManager(ApiManager apiManager, InMemoryManager inMemoryManager)
        {
            if (apiManager == null)
                throw new ArgumentNullException(nameof(ApiManagerInstance));
            if (inMemoryManager == null)
                throw new ArgumentNullException(nameof(inMemoryManager));

            ApiManagerInstance = apiManager;
            InMemoryManagerInstance = inMemoryManager;
        }

        public CardDefinition GetCardDefinition(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), id, "Must be greater than 0");

            var setEnglishLocaleCall = new ApiCallList.Players.SetLanguages(new List<string>() { EnglishLocale });

            var getCardBaseInfoCall = new ApiCallList.Characters.GetCharacters();
            getCardBaseInfoCall.charactersIDs = new List<int>() { id };
            getCardBaseInfoCall.maxLevels = true;
            getCardBaseInfoCall.ItemsFilter = new List<string>() { "name", "clan_id", "rarity", "ability", "ability_unlock_level" };

            var getCardLevelsCall = new ApiCallList.Characters.GetCharacterLevels(id);
            getCardLevelsCall.levelMax = -1; // Get all the levels
            getCardLevelsCall.ItemsFilter = new List<string>() { "level", "power", "damage" };

            var request = new ApiRequest(setEnglishLocaleCall);
            request.EnqueueApiCall(getCardBaseInfoCall);
            request.EnqueueApiCall(getCardLevelsCall);

            string response;
            HttpStatusCode statusCode = ApiManagerInstance.SendRequest(request, out response);
            if (statusCode != HttpStatusCode.OK)
                throw new Exception("GetCardBase SendRequest returned: " + statusCode.ToString());
            dynamic decoded = JsonDecoder.Decode(response);

            dynamic dynBaseCard;
            try
            {
                dynBaseCard = decoded[getCardBaseInfoCall.Call]["items"][0];
            }
            catch
            {
                // TODO: Log attempt to download invalid ID
                throw new ArgumentException("The id doesn't exist", nameof(id));
            }

            string name = dynBaseCard["name"].ToString();
            int clan_id = int.Parse(dynBaseCard["clan_id"].ToString());
            string rarity = dynBaseCard["rarity"].ToString();
            string ability = dynBaseCard["ability"].ToString();
            int ability_unlock_level = int.Parse(dynBaseCard["ability_unlock_level"].ToString());

            var cardStatsPerLevel = new List<CardStats>();
            foreach (dynamic item in decoded[getCardLevelsCall.Call]["items"])
            {
                int level = int.Parse(item["level"].ToString());
                int power = int.Parse(item["power"].ToString());
                int damage = int.Parse(item["damage"].ToString());
                cardStatsPerLevel.Add(new CardStats(level, power, damage));
            }

            return ApiToCardDefinitionAdapter.createCardDefinitionByServerData(id, name, clan_id, rarity, ability, ability_unlock_level, cardStatsPerLevel);
        }
        public string GetUserLocale()
        {
            var getLocaleCall = new ApiCallList.General.GetPlayer();
            getLocaleCall.ContextFilter = new List<string>() { "player.locale" };

            var request = new ApiRequest(getLocaleCall);

            string response;
            HttpStatusCode statusCode = ApiManagerInstance.SendRequest(request, out response);
            if (statusCode != HttpStatusCode.OK)
                throw new Exception("GetUserLocale returned: " + statusCode.ToString());
            dynamic decoded = JsonDecoder.Decode(response);

            return decoded[getLocaleCall.Call]["context"]["player"]["locale"].ToString();
        }
        public void SetUserLocale(string locale)
        {
            if (String.IsNullOrWhiteSpace(locale))
                throw new ArgumentNullException(nameof(locale));

            var setLocaleCall = new ApiCallList.Players.SetLanguages(new List<string>() { locale });
            var getLocaleCall = new ApiCallList.General.GetPlayer();
            getLocaleCall.ContextFilter = new List<string>() { "player.locale" };

            var request = new ApiRequest(setLocaleCall);
            request.EnqueueApiCall(getLocaleCall);

            string response;
            HttpStatusCode statusCode = ApiManagerInstance.SendRequest(request, out response);
            if (statusCode != HttpStatusCode.OK)
                throw new Exception("SetUserLocale returned: " + statusCode.ToString());
        }
        public string GetUserName()
        {
            var getUserNameCall = new ApiCallList.General.GetPlayer();
            getUserNameCall.ContextFilter = new List<string>() { "player.name" };

            throw new NotImplementedException();
        }

        public IEnumerable<int> GetAllCardBaseIds()
        {
            var getExistingCardBaseIdsCall = new ApiCallList.Urc.GetCharacters();
            getExistingCardBaseIdsCall.ItemsFilter = new List<string>() { "id" };

            var request = new ApiRequest(getExistingCardBaseIdsCall);

            string response;
            HttpStatusCode statusCode = ApiManagerInstance.SendRequest(request, out response);
            if (statusCode != HttpStatusCode.OK)
                throw new Exception("GetAllCardBaseIds SendRequest returned: " + statusCode.ToString());
            dynamic decoded = JsonDecoder.Decode(response);

            dynamic items = decoded[getExistingCardBaseIdsCall.Call]["items"];
            foreach (var item in items)
                yield return int.Parse(item["id"].ToString());
        }
    }
}
