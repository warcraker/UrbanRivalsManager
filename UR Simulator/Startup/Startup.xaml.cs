using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UrbanRivalsManager.Utils;
using UrbanRivalsManager.ViewModel;

namespace UrbanRivalsManager.Startup
{
    /// <summary>
    /// Interaction logic for Startup.xaml
    /// </summary>
    public partial class Startup : Window
    {
        BackgroundWorker worker;

        public Startup()
        {
            InitializeComponent();

            worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

            worker.RunWorkerAsync();
        }

        private void InitializeCultures()
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Culture))
            {
                //FrameworkElement.LanguageProperty.OverrideMetadata(
                //    typeof(FrameworkElement),
                //    new FrameworkPropertyMetadata(
                //    System.Windows.Markup.XmlLanguage.GetLanguage(Properties.Settings.Default.Culture)));

                Thread.CurrentThread.CurrentCulture = new CultureInfo(Properties.Settings.Default.Culture);
            }
            if (!string.IsNullOrEmpty(Properties.Settings.Default.UICulture))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Properties.Settings.Default.UICulture);
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.InvokeIfRequired(() =>
            {
                // TODO: Implement
            }, System.Windows.Threading.DispatcherPriority.Background);
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.InvokeIfRequired(() =>
            {
                new MainView().Show();
                this.Close();
            }, System.Windows.Threading.DispatcherPriority.Background);
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // TODO: Show progress on UI
            this.InvokeIfRequired(() =>
            {
                BackgroundErrorCatcher.Init();
                ViewModelLocator.Init();
            }, System.Windows.Threading.DispatcherPriority.Background);
        }

    }
}
