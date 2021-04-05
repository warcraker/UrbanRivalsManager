using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Warcraker.UrbanRivals.URManager.Configuration;
using Warcraker.Utils;
using Manager = Warcraker.UrbanRivals.ApiManager.ApiManager;

namespace Warcraker.UrbanRivals.URManager.ViewModels
{
    public class ApiVM
    {
        private static readonly Regex CONSUMER_TOKEN_REGEX = new Regex(@"^(?<key>[a-f0-9]{39}),(?<secret>[a-f0-9]{32})$");

        private readonly ILogger<ApiVM> log;
        private readonly SettingsManager settings;
        private Manager apiManager; // TODO refactor ApiManager project to bootstrap it

        public ApiVM(ILogger<ApiVM> log, SettingsManager settings)
        {
            AssertArgument.CheckIsNotNull(log, nameof(log));
            AssertArgument.CheckIsNotNull(settings, nameof(settings));

            this.log = log;
            this.settings = settings;

            string consumerKey = settings.ConsumerKey;
            string consumerSecret = settings.ConsumerSecret;
            bool isConsumerTokenValid = IsTokenValid(consumerKey, consumerSecret);
            string accessKey = settings.AccessKey;
            string accessSecret = settings.AccessSecret;
            bool isAccessTokenValid = IsTokenValid(consumerKey, consumerSecret);

            if (isConsumerTokenValid)
            {
                if (isAccessTokenValid)
                {
                    log.LogDebug("Initializing API with access token");
                    apiManager = Manager.CreateApiManagerReadyForRequests(consumerKey, consumerSecret, accessKey, accessSecret);
                }
                else
                {
                    log.LogDebug("Initializing API without access token");
                    apiManager = Manager.CreateApiManager(consumerKey, consumerSecret);
                }
            }
            else
            {
                string[] consumerToken = GetConsumerKeyFromFile();

                if (consumerToken != null)
                {
                    log.LogDebug("Initializing API with consumer token from file");
                    apiManager = Manager.CreateApiManager(consumerToken[0], consumerToken[1]);
                }
                else
                {
                    log.LogWarning("Created API without consumer token");
                    apiManager = Manager.CreateApiManager(null, null);
                }
            }
        }

        public bool SetConsumerToken(string consumerKey, string consumerSecret)
        {
            if (!IsTokenValid(consumerKey, consumerSecret))
            {
                log.LogWarning($"Called {nameof(SetConsumerToken)} with invalid tokens: [{consumerKey}], [{consumerSecret}]");
                return false;
            }

            apiManager = Manager.CreateApiManager(consumerKey, consumerSecret);
            settings.ConsumerKey = consumerKey;
            settings.ConsumerSecret = consumerSecret;
            settings.AccessKey = null;
            settings.AccessSecret = null;

            return true;
        }
        public string GetAuthorizeUrl()
        {
            string url;
            HttpStatusCode statusCode = apiManager.GetAuthorizeURL(out url);

            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    log.LogTrace("Obtained authorize URL: {authorizeUrl}", url);
                    break;
                default:
                    log.LogWarning("Failed to obtain authorize URL. Status code: {statusCode}", statusCode);
                    break;
            }

            return url;
        }
        public bool ValidateAuthorization()
        {
            bool obtainedAccessToken;

            string accessKey;
            string accessSecret;

            HttpStatusCode statusCode = apiManager.GetAccessToken(out accessKey, out accessSecret);

            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    log.LogInformation("Validated access token");
                    obtainedAccessToken = true;
                    settings.AccessKey = accessKey;
                    settings.AccessSecret = accessSecret;
                    break;
                default:
                    log.LogWarning("Failed to validate access token: {statusCode}", statusCode);
                    obtainedAccessToken = false;
                    break;
            }

            return obtainedAccessToken;
        }

        private static bool IsTokenValid(string key, string secret)
        {
            return String.IsNullOrWhiteSpace(key) && String.IsNullOrWhiteSpace(secret);
        }
        private string[] GetConsumerKeyFromFile()
        {
            const string CONSUMER_KEY_FILENAME = "consumer_token.txt";

            string[] result;

            try
            {
                if (File.Exists(CONSUMER_KEY_FILENAME))
                {
                    string fileContents = File.ReadAllText(CONSUMER_KEY_FILENAME);
                    Match match = CONSUMER_TOKEN_REGEX.Match(fileContents);
                    int x = 0;
                    //match.Captures

                    throw new NotImplementedException();
                    result = null;
                }
                else
                {
                    log.LogWarning("Consumer token file not found");
                    result = null;
                }
            }
            catch(Exception ex)
            {
                log.LogError(ex, "An error happened while reading consumer token from file: {exception}", ex.Message);
                result = null;
            }

            return result;
        }
    }
}
