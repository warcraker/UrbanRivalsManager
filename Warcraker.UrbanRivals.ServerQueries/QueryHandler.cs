using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using UrbanRivalsApiManager;

namespace Warcraker.UrbanRivals.ServerQueries
{
    public class QueryHandler
    {
        private ApiManager apiManager;

        public QueryHandler(string consumerKey, string consumerSecret, string accessKey, string accessSecret) 
        {
            apiManager = ApiManager.CreateApiManagerReadyForRequests(consumerKey, consumerSecret, accessKey, accessSecret);
        }
        
        public string GetCardDefinitionsBlob()
        {
            string blob;
            string previousUserLocale = GetUserLocale();
                       
            ApiCall setLocaleToEnglishCall = new ApiCallList.Players.SetLanguages(new List<string> { "en" });
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

            blob = ExecuteCall(request);
            SetUserLocale(previousUserLocale);

            return blob;
        }

        private string ExecuteCall(ApiRequest request, [CallerMemberName] string caller = "Unknown Method")
        {
            string response;
            HttpStatusCode statusCode = apiManager.SendRequest(request, out response);
            if (statusCode != HttpStatusCode.OK)
            {
                throw new Exception($"{caller} returned an error status code: {statusCode.ToString()}");
            }

            return response;
        }
        private string GetUserLocale()
        {
            string locale;
            ApiCall getLocaleCall = new ApiCallList.General.GetPlayer
            {
                ContextFilter = new List<string>() { "player.locale" }
            };
            ApiRequest request = new ApiRequest(getLocaleCall);

            string response = ExecuteCall(request);
            dynamic decoded = JsonConvert.DeserializeObject(response);
            locale = decoded[getLocaleCall.Call]["context"]["player"]["locale"].ToString();

            return locale;
        }
        private void SetUserLocale(string locale)
        {
            ApiCall setLocaleCall = new ApiCallList.Players.SetLanguages(new List<string> { locale });
            ApiRequest request = new ApiRequest(setLocaleCall);

            ExecuteCall(request);
        }
    }
}
