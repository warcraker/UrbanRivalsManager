using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using Warcraker.UrbanRivals.URManager.Configuration;
using Warcraker.UrbanRivals.URManager.ViewModels;
using Warcraker.Utils;

namespace Warcraker.UrbanRivals.URManager.ViewModels
{
    public class StartupVM
    {
        private readonly ILogger log;
        private readonly SettingsManager settings;
        private readonly LanguageVM languageConfigurator;

        public StartupVM(ILogger<StartupVM> log, SettingsManager settings, LanguageVM languageConfigurator)
        {
            AssertArgument.CheckIsNotNull(log, nameof(log));
            AssertArgument.CheckIsNotNull(settings, nameof(settings));
            AssertArgument.CheckIsNotNull(languageConfigurator, nameof(languageConfigurator));

            this.log = log;
            this.settings = settings;
            this.languageConfigurator = languageConfigurator;
        }

        public bool IsLanguageDefined
        {
            get
            {
                switch (settings.Language)
                {
                    case SupportedLanguages.ENGLISH:
                    case SupportedLanguages.SPANISH:
                        return true;
                    default:
                        return false;
                }
            }
        }
        public bool IsOAuthSesionValidated
        {
            get
            {
                return false; // TODO
            }
        }

        public void OnApplicationStart()
        {
            string assemblyFullName = Assembly.GetCallingAssembly().GetName().FullName;
            log.LogInformation("Starting application... {assembly}", assemblyFullName);

            string language;
            if (!IsLanguageDefined)
            {
                language = SupportedLanguages.ENGLISH;
                log.LogDebug("Language is not defined at start.");
            }
            else
            {
                language = settings.Language;
                log.LogDebug("Language is {language}", language);
            }
            languageConfigurator.SetLanguage(language);
        }
    }
}
