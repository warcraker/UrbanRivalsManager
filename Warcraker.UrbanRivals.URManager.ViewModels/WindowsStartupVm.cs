using System.Reflection;
using Microsoft.Extensions.Logging;
using Warcraker.UrbanRivals.Interfaces;
using Warcraker.UrbanRivals.URManager.Configuration;
using Warcraker.Utils;

namespace Warcraker.UrbanRivals.URManager.ViewModels
{
    public class WindowsStartupVm
    {
        private readonly ILogger<WindowsStartupVm> _log;
        private readonly ILanguageVM _languageVm;

        public WindowsStartupVm(ILogger<WindowsStartupVm> log, ILanguageVM languageVm)
        {
            AssertArgument.CheckIsNotNull(log, nameof(log));
            AssertArgument.CheckIsNotNull(languageVm, nameof(languageVm));

            _log = log;
            _languageVm = languageVm;
        }

        public bool IsLanguageDefined()
        {
            _log.LogTrace($"Entering {nameof(IsLanguageDefined)}");

            switch (_languageVm.GetLanguage())
            {
                case SupportedLanguages.ENGLISH:
                case SupportedLanguages.SPANISH:
                    return true;
                default:
                    return false;
            }
        }
        public bool IsOAuthSesionValidated()
        {
            _log.LogTrace($"Entering {nameof(IsOAuthSesionValidated)}");

            return false; // TODO
        }
        public void OnApplicationStart()
        {
            _log.LogTrace($"Entering {nameof(OnApplicationStart)}");

            if (!IsLanguageDefined())
            {
                _log.LogDebug("Language is not defined at start.");
                // TODO ask for language
            }
            else
            {
                string language = _languageVm.GetLanguage();
                _log.LogInformation("Application starts with language = {language}", language);
            }

            if (!IsOAuthSesionValidated())
            {
                _log.LogDebug("OAuth is not validated at start.");
                // TODO OAuth validation and start
            }
            else
            {
                // TODO log OAuth info
            }
        }
    }
}
