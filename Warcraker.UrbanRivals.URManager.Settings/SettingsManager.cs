using System.Configuration;
using Microsoft.Extensions.Logging;
using Warcraker.Utils;

namespace Warcraker.UrbanRivals.URManager.Configuration
{
    public class SettingsManager
    {
        private readonly ILogger log;

        public SettingsManager(ILogger<SettingsManager> log)
        {
            AssertArgument.CheckIsNotNull(log, nameof(log));

            this.log = log;
        }

        public string Language
        {
            get
            {
                return GetValue(nameof(Language));
            }
            set
            {
                switch (value)
                {
                    case SupportedLanguages.ENGLISH:
                    case SupportedLanguages.SPANISH:
                        SetValue(nameof(Language), value);
                        break;
                    default:
                        log.LogWarning("Provided unsupported language: {language}. Setting language to default (English)", value);
                        Asserts.Fail($"Unsupported language {value}");
                        SetValue(nameof(Language), SupportedLanguages.ENGLISH);
                        break;
                }
            }
        }
        public string ConsumerKey
        {
            get
            {
                return GetValue(nameof(ConsumerKey));
            }
            set
            {
                SetValue(nameof(ConsumerKey), value);
            }
        }
        public string ConsumerSecret
        {
            get
            {
                return GetValue(nameof(ConsumerSecret));
            }
            set
            {
                SetValue(nameof(ConsumerSecret), value);
            }
        }
        public string AccessKey
        {
            get
            {
                return GetValue(nameof(AccessKey));
            }
            set
            {
                SetValue(nameof(AccessKey), value);
            }
        }
        public string AccessSecret
        {
            get
            {
                return GetValue(nameof(AccessSecret));
            }
            set
            {
                SetValue(nameof(AccessSecret), value);
            }
        }

        private string GetValue(string key)
        {
            string value = ConfigurationManager.AppSettings.Get(key);
            log.LogTrace("Obtained setting [{key}]: {value}", key, value);

            return value;
        }
        private void SetValue(string key, string value)
        {
            log.LogTrace("Stored setting [{key}]: {value}", key, value);
            ConfigurationManager.AppSettings.Set(key, value);
        }
    }
}
