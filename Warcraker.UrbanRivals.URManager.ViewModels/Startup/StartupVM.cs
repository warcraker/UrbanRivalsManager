using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.Extensions.Logging;
using Warcraker.UrbanRivals.URManager.Configuration;
using Warcraker.UrbanRivals.URManager.ViewModels.Language;
using Warcraker.Utils;

namespace Warcraker.UrbanRivals.URManager.ViewModels.Startup
{
    public class StartupVM
    {
        private readonly ILogger<StartupVM> log;
        private readonly URConfigurationManager config;

        public StartupVM(ILogger<StartupVM> log, URConfigurationManager config)
        {
            AssertArgument.CheckIsNotNull(log, nameof(log));
            AssertArgument.CheckIsNotNull(config, nameof(config));

            this.log = log;
            this.config = config;
        }

        public void OnApplicationStart()
        {
            log.LogInformation("Starting application...");

            if (config.IsFirstRun)
            {
                ExecuteFirstTimeConfiguration();
                config.IsFirstRun = false;
            }

            throw new NotImplementedException();
        }
        private void ExecuteFirstTimeConfiguration()
        {
            log.LogInformation("Executing first time configuration...");
            


            throw new NotImplementedException();
        }
    }
}
