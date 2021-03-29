using System;
using System.Configuration;
using Microsoft.Extensions.Logging;
using Warcraker.Utils;

namespace Warcraker.UrbanRivals.URManager.Configuration
{
    public class URConfigurationManager
    {
        private readonly ILogger log;

        public URConfigurationManager(ILogger<URConfigurationManager> log) 
        {
            AssertArgument.CheckIsNotNull(log, nameof(log));

            this.log = log;
        }

        public bool IsFirstRun
        {
            get
            {
                string storedValue = GetValue(nameof(IsFirstRun));
                return String.IsNullOrEmpty(storedValue);
            }
            set
            {
                if (value)
                {
                    log.LogInformation($"Setting {nameof(IsFirstRun)} as true");
                    SetValue(nameof(IsFirstRun), null);
                }
                else
                {
                    SetValue(nameof(IsFirstRun), value.ToString());
                }
            }
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
                        log.LogInformation("Setting language: {language}", value);
                        SetValue(nameof(Language), value);
                        break;
                    default:
                        log.LogError("Provided unsupported language: {language}. Setting language to default (English)", value);
                        Asserts.Fail($"Unsupported language {value}");
                        SetValue(nameof(Language), SupportedLanguages.ENGLISH);
                        break;
                }
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
