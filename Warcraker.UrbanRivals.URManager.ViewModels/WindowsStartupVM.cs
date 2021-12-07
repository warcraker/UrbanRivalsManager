using System.Reflection;
using Microsoft.Extensions.Logging;
using Warcraker.UrbanRivals.Interfaces;
using Warcraker.UrbanRivals.URManager.Configuration;
using Warcraker.Utils;

namespace Warcraker.UrbanRivals.URManager.ViewModels
{
    public class WindowsStartupVM
    {
        private readonly ILogger _log;
        private readonly ILanguageVM _languageVm;

        public WindowsStartupVM(ILogger<WindowsStartupVM> log, ILanguageVM languageVm)
        {
            AssertArgument.CheckIsNotNull(log, nameof(log));
            AssertArgument.CheckIsNotNull(languageVm, nameof(languageVm));

            _log = log;
            _languageVm = languageVm;
        }

        public bool IsLanguageDefined()
        {
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
            return false; // TODO
        }
        public void OnApplicationStart()
        {
            string assemblyFullName = Assembly.GetCallingAssembly().GetName().FullName;
            _log.LogInformation("Starting application... {assembly}", assemblyFullName);

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
