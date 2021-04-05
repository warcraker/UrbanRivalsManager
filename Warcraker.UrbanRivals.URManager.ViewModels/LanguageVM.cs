using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.Extensions.Logging;
using Warcraker.UrbanRivals.URManager.Configuration;
using Warcraker.Utils;

namespace Warcraker.UrbanRivals.URManager.ViewModels
{
    public class LanguageVM
    {
        private readonly ILogger<LanguageVM> log;

        public LanguageVM(ILogger<LanguageVM> log)
        {
            AssertArgument.CheckIsNotNull(log, nameof(log));

            this.log = log;
        }

        public void SetLanguage(string language)
        {
            switch (language)
            {
                case SupportedLanguages.ENGLISH:
                case SupportedLanguages.SPANISH:
                    log.LogDebug("Applying language {language}", language);
                    SetApplicationLanguage(language);
                    break;
                default:
                    log.LogWarning("Not supported language [{language}]. Using English per default", language);
                    Asserts.Fail($"Not supported language [{language}]");
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
