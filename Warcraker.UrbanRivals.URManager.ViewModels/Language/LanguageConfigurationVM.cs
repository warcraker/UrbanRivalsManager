using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.Extensions.Logging;
using Warcraker.UrbanRivals.URManager.Configuration;
using Warcraker.Utils;

namespace Warcraker.UrbanRivals.URManager.ViewModels.Language
{
    public class LanguageConfigurationVM
    {
        private readonly ILogger<LanguageConfigurationVM> log;
        private readonly URConfigurationManager config;

        public LanguageConfigurationVM(ILogger<LanguageConfigurationVM> log, URConfigurationManager config)
        {
            AssertArgument.CheckIsNotNull(log, nameof(log));
            AssertArgument.CheckIsNotNull(config, nameof(config));

            this.log = log;
            this.config = config;
        }

        public void SetLanguage(string language)
        {
            switch (language)
            {
                case SupportedLanguages.ENGLISH:
                case SupportedLanguages.SPANISH:
                    log.LogDebug("Start application with language {language}", language);
                    SetApplicationLanguage(language);
                    break;
                default:
                    log.LogWarning("Invalid language on startup [{language}]. Using English per default", language);
                    Asserts.Fail($"Invalid language on startup [{language}]");
                    SetApplicationLanguage(SupportedLanguages.ENGLISH);
                    break;
            }
        }

        private void SetApplicationLanguage(string language)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(language);
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(language);
        }
    }
}
