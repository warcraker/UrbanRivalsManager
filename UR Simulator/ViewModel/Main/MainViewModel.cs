using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UrbanRivalsApiManager;
using UrbanRivalsManager.Utils;
using UrbanRivalsManager.ViewModel.DataManagement;

namespace UrbanRivalsManager.ViewModel
{
    public class MainViewModel : DependencyObject
    {
        public ApiManager ApiManager
        {
            get { return (ApiManager)GetValue(ApiManagerProperty); }
            set { SetValue(ApiManagerProperty, value); }
        }
        public static readonly DependencyProperty ApiManagerProperty =
            DependencyProperty.Register("ApiManager", typeof(ApiManager), typeof(MainViewModel), new PropertyMetadata(null));
        public IDatabaseManager DatabaseManager
        {
            get { return (IDatabaseManager)GetValue(DatabaseManagerProperty); }
            set { SetValue(DatabaseManagerProperty, value); }
        }
        public static readonly DependencyProperty DatabaseManagerProperty =
            DependencyProperty.Register("DatabaseManager", typeof(IDatabaseManager), typeof(MainViewModel), new PropertyMetadata(null));
        public FilepathManager FilepathManager
        {
            get { return (FilepathManager)GetValue(FilepathManagerProperty); }
            set { SetValue(FilepathManagerProperty, value); }
        }
        public static readonly DependencyProperty FilepathManagerProperty =
            DependencyProperty.Register("FilepathManager", typeof(FilepathManager), typeof(MainViewModel), new PropertyMetadata(null));
        public ImageCache ImageCache
        {
            get { return (ImageCache)GetValue(ImageCacheProperty); }
            set { SetValue(ImageCacheProperty, value); }
        }
        public static readonly DependencyProperty ImageCacheProperty =
            DependencyProperty.Register("ImageCache", typeof(ImageCache), typeof(MainViewModel), new PropertyMetadata(null));
        public ImageDownloader ImageDownloader
        {
            get { return (ImageDownloader)GetValue(ImageDownloaderProperty); }
            set { SetValue(ImageDownloaderProperty, value); }
        }
        public static readonly DependencyProperty ImageDownloaderProperty =
            DependencyProperty.Register("ImageDownloader", typeof(ImageDownloader), typeof(MainViewModel), new PropertyMetadata(null));
        public ImageRetriever ImageRetriever
        {
            get { return (ImageRetriever)GetValue(ImageRetrieverProperty); }
            set { SetValue(ImageRetrieverProperty, value); }
        }
        public static readonly DependencyProperty ImageRetrieverProperty =
            DependencyProperty.Register("ImageRetriever", typeof(ImageRetriever), typeof(MainViewModel), new PropertyMetadata(null));
        public InMemoryManager InMemoryManager
        {
            get { return (InMemoryManager)GetValue(InMemoryManagerProperty); }
            set { SetValue(InMemoryManagerProperty, value); }
        }
        public static readonly DependencyProperty InMemoryManagerProperty =
            DependencyProperty.Register("InMemoryManager", typeof(InMemoryManager), typeof(MainViewModel), new PropertyMetadata(null));
        public ServerQueriesManager ServerQueriesManager
        {
            get { return (ServerQueriesManager)GetValue(ServerQueriesManagerProperty); }
            set { SetValue(ServerQueriesManagerProperty, value); }
        }
        public static readonly DependencyProperty ServerQueriesManagerProperty =
            DependencyProperty.Register("ServerQueriesManager", typeof(ServerQueriesManager), typeof(MainViewModel), new PropertyMetadata(null));
        public GlobalManager GlobalManager
        {
            get { return (GlobalManager)GetValue(GlobalManagerProperty); }
            set { SetValue(GlobalManagerProperty, value); }
        }
        public static readonly DependencyProperty GlobalManagerProperty =
            DependencyProperty.Register("GlobalManager", typeof(GlobalManager), typeof(MainViewModel), new PropertyMetadata(null));


        public HttpStatusCode GetApiAutenticateUrl(out string url)
        {
            HttpStatusCode statusCode;

            statusCode = ApiManager.GetAuthorizeURL(out url);

            return statusCode;
        }
        public void ValidateAccessTokenAndStoreInSettings()
        {
            string[] accessToken;

            accessToken = new string[2];
            ApiManager.GetAccessToken(out accessToken[0], out accessToken[1]);
            Properties.Settings.Default.AccessKey = accessToken[0];
            Properties.Settings.Default.AccessSecret = accessToken[1];
            Properties.Settings.Default.Save();
        }
    }
}
