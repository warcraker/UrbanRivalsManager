using Autofac;
using System.Diagnostics;
using System.Windows;
using Microsoft.Extensions.Logging;
using Warcraker.UrbanRivals.URManager.View.LanguageSelection;
using Warcraker.UrbanRivals.URManager.View.Main;
using Warcraker.UrbanRivals.URManager.ViewModels;
using System.Reflection;
using System.Globalization;
using System;

namespace Warcraker.UrbanRivals.URManager.View.Startup
{
    public partial class StartupWindow : Window
    {
        private readonly ILogger _log;
        private readonly WindowsStartupVm _vm;

        public StartupWindow()
        {
            InitializeComponent();

            using (ILifetimeScope scope = AutofacContainer.INSTANCE.BeginLifetimeScope())
            {
                _vm = scope.Resolve<WindowsStartupVm>() ?? throw new ArgumentNullException(nameof(WindowsStartupVm));
                _log = scope.Resolve<ILogger<StartupWindow>>() ?? throw new ArgumentNullException(nameof(ILogger<StartupWindow>));
            }

            AssemblyName assembly = Assembly.GetExecutingAssembly().GetName();
            string applicationName = assembly.FullName;
            string applicationVersion = assembly.Version.ToString();
            _log.LogInformation("Starting {ApplicationName}. Version: {ApplicationVersion}.", applicationName, applicationVersion);

            _vm.OnApplicationStart();

            if(!_vm.IsLanguageDefined())
            {
                _log.LogInformation("Locale was not defined on start.");
                var languageSelectionWindow = new LanguageSelectionWindow();
                languageSelectionWindow.ShowDialog();
                if (_vm.IsLanguageDefined())
                {
                    _log.LogDebug("Restarting application...");
                    RestartApplication();
                }
                else
                {
                    _log.LogWarning("Application closed with no defined locale.");
                }
            }
            else
            {
                string cultureName = CultureInfo.DefaultThreadCurrentCulture.Name;
                _log.LogInformation("Current culture set to {CurrentCulture}.", cultureName);
                var mainWindow = new MainWindow();
                mainWindow.ShowDialog();
                _log.LogInformation("Closing application...");
            }

            CloseApplication();
        }

        private void CloseApplication()
        {
            Application.Current.Shutdown();
        }
        private void RestartApplication()
        {
            string currentProcessPath = Process.GetCurrentProcess().MainModule.FileName;
            Process.Start(currentProcessPath);
            CloseApplication();
        }
    }
}
