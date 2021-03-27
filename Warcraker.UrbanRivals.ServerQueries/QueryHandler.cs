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
        private readonly ApiManager apiManager;

        public QueryHandler(string consumerKey, string consumerSecret, string accessKey, string accessSecret) 
        {
            apiManager = ApiManager.CreateApiManagerReadyForRequests(consumerKey, consumerSecret, accessKey, accessSecret);
        }
        
        public string GetCycleBlob()
        {
            string blob;
            string previousUserLocale = GetUserLocale();
                       
            ApiCall setLocaleToEnglishCall = new ApiCallList.Players.SetLanguages(new List<string> { "en" });
            var request = new ApiRequest(setLocaleToEnglishCall);

            ApiCall getAbilitiesCall = new ApiCallList.Characters.GetCharacters
            {
                maxLevels = true,
                ItemsFilter = new List<string> { "id", "ability", "ability_unlock_level" },
            };
            request.EnqueueApiCall(getAbilitiesCall);
            ApiCall getCharactersCall = new ApiCallList.Urc.GetCharacters
            {
                ItemsFilter = new List<string> { "id", "name", "clanID", "rarity", "levels.level", "levels.power", "levels.damage" },
            };
            request.EnqueueApiCall(getCharactersCall);
            ApiCall getClansCall = new ApiCallList.Characters.GetClans
            {
                ItemsFilter = new List<string> { "id", "name", "bonusDescription" },
            };
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
            var request = new ApiRequest(getLocaleCall);

            string response = ExecuteCall(request);
            dynamic decoded = JsonConvert.DeserializeObject(response);
            locale = decoded[getLocaleCall.Call]["context"]["player"]["locale"].ToString();

            return locale;
        }
        private void SetUserLocale(string locale)
        {
            ApiCall setLocaleCall = new ApiCallList.Players.SetLanguages(new List<string> { locale });
            var request = new ApiRequest(setLocaleCall);

            ExecuteCall(request);
        }
    }
}
