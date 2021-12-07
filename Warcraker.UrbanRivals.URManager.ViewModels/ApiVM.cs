using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Warcraker.UrbanRivals.Interfaces;
using Warcraker.Utils;
using Manager = Warcraker.UrbanRivals.ApiManager.ApiManager;

namespace Warcraker.UrbanRivals.URManager.ViewModels
{
    public class ApiVm
    {
        private static readonly Regex CONSUMER_TOKEN_REGEX = new Regex(@"^(?<key>[a-f0-9]{39}),(?<secret>[a-f0-9]{32})$");

        private readonly ILogger<ApiVm> _log;
        private readonly ISettingsManager _settings;
        private Manager _apiManager; // TODO refactor ApiManager project to bootstrap it

        public ApiVm(ILogger<ApiVm> log, ISettingsManager settings)
        {
            AssertArgument.CheckIsNotNull(log, nameof(log));
            AssertArgument.CheckIsNotNull(settings, nameof(settings));

            this._log = log;
            this._settings = settings;

            string consumerKey = _settings.ConsumerKey;
            string consumerSecret = _settings.ConsumerSecret;
            bool isConsumerTokenValid = IsTokenValid(consumerKey, consumerSecret);
            string accessKey = _settings.AccessKey;
            string accessSecret = _settings.AccessSecret;
            bool isAccessTokenValid = IsTokenValid(consumerKey, consumerSecret);

            if (isConsumerTokenValid)
            {
                if (isAccessTokenValid)
                {
                    _log.LogDebug("Initializing API with access token");
                    _apiManager = Manager.CreateApiManagerReadyForRequests(consumerKey, consumerSecret, accessKey, accessSecret);
                }
                else
                {
                    _log.LogDebug("Initializing API without access token");
                    _apiManager = Manager.CreateApiManager(consumerKey, consumerSecret);
                }
            }
            else
            {
                string[] consumerToken = GetConsumerKeyFromFile();

                if (consumerToken != null)
                {
                    _log.LogDebug("Initializing API with consumer token from file");
                    _apiManager = Manager.CreateApiManager(consumerToken[0], consumerToken[1]);
                }
                else
                {
                    log.LogWarning("Created API without consumer token");
                    _apiManager = Manager.CreateApiManager(null, null);
                }
            }
        }

        public bool SetConsumerToken(string consumerKey, string consumerSecret)
        {
            if (!IsTokenValid(consumerKey, consumerSecret))
            {
                _log.LogWarning($"Called {nameof(SetConsumerToken)} with invalid tokens: [{consumerKey}], [{consumerSecret}]");
                return false;
            }

            _apiManager = Manager.CreateApiManager(consumerKey, consumerSecret);
            _settings.ConsumerKey = consumerKey;
            _settings.ConsumerSecret = consumerSecret;
            _settings.AccessKey = null;
            _settings.AccessSecret = null;

            return true;
        }
        public string GetAuthorizeUrl()
        {
            string url;
            HttpStatusCode statusCode = _apiManager.GetAuthorizeURL(out url);

            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    _log.LogTrace("Obtained authorize URL: {authorizeUrl}", url);
                    break;
                default:
                    _log.LogWarning("Failed to obtain authorize URL. Status code: {statusCode}", statusCode);
                    break;
            }

            return url;
        }
        public bool ValidateAuthorization()
        {
            bool obtainedAccessToken;

            string accessKey;
            string accessSecret;

            HttpStatusCode statusCode = _apiManager.GetAccessToken(out accessKey, out accessSecret);

            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    _log.LogInformation("Validated access token");
                    obtainedAccessToken = true;
                    _settings.AccessKey = accessKey;
                    _settings.AccessSecret = accessSecret;
                    break;
                default:
                    _log.LogWarning("Failed to validate access token: {statusCode}", statusCode);
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
                    //match.Captures // TODO

                    throw new NotImplementedException();
                }
                else
                {
                    _log.LogWarning("Consumer token file not found");
                    result = null;
                }
            }
            catch(Exception ex)
            {
                _log.LogError(ex, "An error happened while reading consumer token from file: {exception}", ex.Message);
                result = null;
            }

            return result;
        }
    }
}
