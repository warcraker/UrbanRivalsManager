using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using UrbanRivalsManager.Utils;
using UrbanRivalsManager.ViewModel;
using UrbanRivalsManager.View;
using System.Net;

namespace UrbanRivalsManager
{
    public partial class MainView : Window
    {
        MainViewModel MainViewModel;

        public MainView()
        {
            MainViewModel = (MainViewModel)ViewModelLocator.Locator["Main"]; 
            
            /* --- Subscribe Events --- */

            MainViewModel.GlobalManager.UpdateCardBaseDefinitions_ProgressChanged += UpdateCardBaseDefinitions_ProgressChanged;
            MainViewModel.GlobalManager.UpdateCardBaseDefinitions_RunWorkerCompleted += UpdateCardBaseDefinitions_RunWorkerCompleted;
            MainViewModel.GlobalManager.UpdateCardInstanceDefinitions_ProgressChanged += delegate { }; // TODO: Subscribe
            MainViewModel.GlobalManager.UpdateCardInstanceDefinitions_RunWorkerCompleted += delegate { };  // TODO: Subscribe

            DataContext = MainViewModel;

            InitializeComponent();
        }

        // --- Eventos api ---

        private void btnDownloadLibrary_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.GlobalManager.UpdateCardBaseDefinitionsAsync();
        }

        private void btnDownloadInstances_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.GlobalManager.UdpateCardInstanceDefinitionsAsync();
        }

        // --- Backgroundworkers --- // TODO: Update these
        private void UpdateCardBaseDefinitions_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.InvokeIfRequired(() =>
                {
                    int progressPercentage = e.ProgressPercentage;
                    object userState = (e.UserState == null ? "" : e.UserState);

                    lblP1.Content = progressPercentage;
                    lblP2.Content = userState.ToString();
                },DispatcherPriority.Background);
        }
        private void UpdateCardBaseDefinitions_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                throw new Exception("Backgroundworker failed" + ". " + e.Error.Message, e.Error);
            }
        }

        // TODO: This is meant as a separation, when the editor creates a event, it will place it after this 
        private void NothingHere() { /* :) */ }

        private void Crear_transparente(object sender, RoutedEventArgs e)
        {
            new CombatCalculatorCreationModeView().Show();
            Close();
        }

        private void Restart(object sender, RoutedEventArgs e)
        {
            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void AutenticarPaso1(object sender, RoutedEventArgs e)
        {
            this.InvokeIfRequired(() =>
            {
                HttpStatusCode statusCode;
                string url;

                statusCode = MainViewModel.GetApiAutenticateUrl(out url);

                switch (statusCode)
                {
                    case HttpStatusCode.OK:
                        AutenticadoURL.Text = url;
                        break;
                    case HttpStatusCode.Unauthorized:
                        MessageBox.Show("Unauthorized");
                        break;
                    case HttpStatusCode.MethodNotAllowed:
                        MessageBox.Show("MethodNotAllowed");
                        break;
                    default:
                        MessageBox.Show($"Something happened. Error code: {statusCode}");
                        break;
                }

            }, DispatcherPriority.Background);
        }

        private void AutenticarPaso2(object sender, RoutedEventArgs e)
        {
            this.InvokeIfRequired(() =>
            {
                MainViewModel.ValidateAccessTokenAndStoreInSettings();
           }, DispatcherPriority.Background);
            
        }

        private void DownloadImages(object sender, RoutedEventArgs e)
        {
            MainViewModel.ImageDownloader.CheckAndEnqueueNotDownloadedImages(ViewModel.DataManagement.CharacterImageFormat.Color800x640);
        }
    }
}
