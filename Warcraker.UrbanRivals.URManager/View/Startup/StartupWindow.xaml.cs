using System.Diagnostics;
using System.Windows;
using Autofac;
using Warcraker.UrbanRivals.URManager.View.LanguageSelection;
using Warcraker.UrbanRivals.URManager.ViewModels;

namespace Warcraker.UrbanRivals.URManager.View.Startup
{
    public partial class StartupWindow : Window
    {
        public StartupWindow()
        {
            InitializeComponent();

            WindowsStartupVm vm;
            using (ILifetimeScope scope = AutofacContainer.INSTANCE.BeginLifetimeScope())
            {
                vm = scope.Resolve<WindowsStartupVm>();
            }

            vm.OnApplicationStart();

            if(!vm.IsLanguageDefined())
            {
                var languageSelectionWindow = new LanguageSelectionWindow();
                languageSelectionWindow.ShowDialog();
                //Process.Start(Application.ResourceAssembly.Location);
                //Application.Current.Shutdown();
            }
        }
    }
}
