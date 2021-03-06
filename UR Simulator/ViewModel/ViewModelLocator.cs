﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UrbanRivalsApiManager;
using UrbanRivalsManager.ViewModel.DataManagement;

namespace UrbanRivalsManager.ViewModel
{
    public sealed class ViewModelLocator
    {
        public static ViewModelLocator Locator { get { return lazy.Value; } }

        public static void Init()
        {
            // It will force the creation at this point. Won't do nothing if already created.
            var disposable = Locator;
        }

        public object this[string id]
        {
            get
            {
                if (ViewModels.Keys.Contains(id))
                    return ViewModels[id];
                else
                    throw new ArgumentException("The ViewModel indicated doesn't exist: " + id);
            }
        }

        private ViewModelLocator()
        {
            ViewModels = new Dictionary<string, object>();

            InitializeManagers();

            ViewModels["Main"] = CreateMainViewModel();
            ViewModels["CombatCalculatorCreationMode"] = CreateCombatCalculatorCreationModeViewModel();
            ViewModels["CombatCalculatorCombatMode"] = CreateCombatCalculatorCombatModeViewModel();
        }

        private static readonly Lazy<ViewModelLocator> lazy = new Lazy<ViewModelLocator>(() => new ViewModelLocator());
        private Dictionary<string, object> ViewModels;

        private ApiManager ApiManagerInstance;
        private IDatabaseManager DatabaseManagerInstance;
        private FilepathManager FilepathManagerInstance;
        private GlobalManager GlobalManagerInstance;
        private ImageCache ImageCacheInstance;
        private ImageDownloader ImageDownloaderInstance;
        private ImageRetriever ImageRetrieverInstance;
        private InMemoryManager InMemoryManagerInstance;
        private ServerQueriesManager ServerQueriesManagerInstance;

        private void InitializeManagers()
        {
            FilepathManagerInstance = new FilepathManager(GetFileManagerPath());

            DatabaseManagerInstance = new SQLiteDatabaseManager(FilepathManagerInstance);

            InMemoryManagerInstance = new InMemoryManager(DatabaseManagerInstance);

            ApiManagerInstance = new ApiManager
                (Properties.Settings.Default.ConsumerKey, Properties.Settings.Default.ConsumerSecret, 
                Properties.Settings.Default.AccessKey, Properties.Settings.Default.AccessSecret,
                Properties.Settings.Default.RequestKey, Properties.Settings.Default.RequestSecret);

            ServerQueriesManagerInstance = new ServerQueriesManager(ApiManagerInstance, InMemoryManagerInstance);

            ImageCacheInstance = new ImageCache(
                Properties.Settings.Default.ImageCacheSizeInMB,
                new TimeSpan(0, 0, Properties.Settings.Default.ImageCacheSlidingExpirationInSeconds));

            ImageRetrieverInstance = new ImageRetriever(FilepathManagerInstance, ImageCacheInstance);

            ImageDownloaderInstance = new ImageDownloader(FilepathManagerInstance, InMemoryManagerInstance);

            GlobalManagerInstance = new GlobalManager
                (ApiManagerInstance, DatabaseManagerInstance, ImageDownloaderInstance, InMemoryManagerInstance, ServerQueriesManagerInstance);

            PortraitConverter portraitConv = (PortraitConverter)Application.Current.FindResource("PortraitConv");
            portraitConv.ImageRetriever = ImageRetrieverInstance;
        }
        private object CreateMainViewModel()
        {
            var vm = new MainViewModel();
            vm.ApiManager = ApiManagerInstance;
            vm.DatabaseManager = DatabaseManagerInstance;
            vm.FilepathManager = FilepathManagerInstance;
            vm.GlobalManager = GlobalManagerInstance;
            vm.ImageDownloader = ImageDownloaderInstance;
            vm.InMemoryManager = InMemoryManagerInstance;
            vm.ServerQueriesManager = ServerQueriesManagerInstance;
            return vm;
        }
        private object CreateCombatCalculatorCreationModeViewModel()
        {
            var vm = new CombatCalculatorCreationModeViewModel();
            vm.InMemoryManager = InMemoryManagerInstance;

            return vm;
        }
        private object CreateCombatCalculatorCombatModeViewModel()
        {
            var vm = new CombatCalculatorCombatModeViewModel();
            vm.InMemoryManager = InMemoryManagerInstance;
            return vm;
        }

        private string GetFileManagerPath()
        {
            #if DEBUG
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UR Simulator");
            #else
            return Path.Combine(Environment.CurrentDirectory, "UR Data");
            #endif
        }
    }
}
