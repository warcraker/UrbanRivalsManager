using System.Globalization;
using Microsoft.Extensions.Logging;
using Warcraker.UrbanRivals.Interfaces;
using Warcraker.UrbanRivals.URManager.Configuration;
using Warcraker.Utils;

namespace Warcraker.UrbanRivals.URManager.ViewModels
{
    public class WindowsLanguageVm : ILanguageVm
    {
        private readonly ILogger<WindowsLanguageVm> _log;
        private readonly ISettingsManager _settings;

        public WindowsLanguageVm(ILogger<WindowsLanguageVm> log, ISettingsManager settings)
        {
            AssertArgument.CheckIsNotNull(log, nameof(log));
            AssertArgument.CheckIsNotNull(settings, nameof(settings));

            _log = log;
            _settings = settings;
        }

        public string GetLanguage()
        {
            return _settings.Language;
        }
        public void SetLanguage(string language)
        {
            switch (language)
            {
                case SupportedLanguages.ENGLISH:
                case SupportedLanguages.SPANISH:
                    _log.LogDebug("Applying language {language}", language);
                    SetApplicationLanguage(language);
                    break;
                default:
                    _log.LogWarning("Not supported language [{language}]. Using English per default", language);
                    Asserts.Fail($"Not supported language [{language}]");
                    SetApplicationLanguage(SupportedLanguages.ENGLISH);
                    break;
            }
        }

        private void SetApplicationLanguage(string language)
        {
            _settings.Language = language;
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(language);
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(language);
        }
    }
}
