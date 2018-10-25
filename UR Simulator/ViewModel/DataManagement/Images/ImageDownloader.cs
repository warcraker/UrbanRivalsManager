using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

using UrbanRivalsCore.Model;
using System.Windows.Media.Imaging;

namespace UrbanRivalsManager.ViewModel.DataManagement
{
    public class ImageDownloader
    {
        private readonly object OnOffLock = new object();
        private FilepathManager FilepathManagerInstance;
        private InMemoryManager InMemoryManagerInstance;
        private ConcurrentQueue<CharacterImageIdentifier> ImagesQueue;
        private BackgroundWorker Worker;

        private static void DownloadImagesQueueProcessing_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            Debug.Assert(e.Argument is DownloadImagesQueueDoWorkArgument);
            var argument = (DownloadImagesQueueDoWorkArgument)e.Argument;
            var queue = argument.Queue;
            var filepathManagerInstance = argument.FilepathManagerInstance;

            while (true)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                if (queue.IsEmpty)
                {
                    Thread.Sleep(Constants.SleepWhenIdleInMiliseconds);
                    continue;
                }

                CharacterImageIdentifier enqueuedImageId;
                if (!queue.TryDequeue(out enqueuedImageId))
                {
                    Thread.Sleep(Constants.SleepWhenIdleInMiliseconds);
                    continue;
                }

                using (var client = new WebClient())
                {
                    string URL = enqueuedImageId.Url;
                    string filepath = filepathManagerInstance.ImagesDirectory;
                    string filename = enqueuedImageId.Filename;
                    string fullFilepath = Path.Combine(filepath, filename);
                    try
                    {
                        client.DownloadFile(URL, fullFilepath);
                    }
                    catch (Exception ex)
                    {
                        // TODO: log errors, catch WebException
                        Console.WriteLine(ex.Message);
                        //throw new Exception("Failed DownloadImagesQueueProcessing_DoWork", ex);
                    }
                }
            }
        }

        private ImageDownloader() { } 
        public ImageDownloader(FilepathManager filepathManager, InMemoryManager inMemoryManager)
        {
            if (filepathManager == null)
                throw new ArgumentNullException(nameof(filepathManager));
            if (inMemoryManager == null)
                throw new ArgumentNullException(nameof(inMemoryManager));

            FilepathManagerInstance = filepathManager;
            InMemoryManagerInstance = inMemoryManager;
            ImagesQueue = new ConcurrentQueue<CharacterImageIdentifier>();
            Worker = new BackgroundWorker();
            Worker.WorkerSupportsCancellation = true;
            Worker.DoWork += DownloadImagesQueueProcessing_DoWork;

            StartDownloadQueue();
        }

        public void StartDownloadQueue()
        {
            lock (OnOffLock)
            {
                if (Worker.IsBusy)
                    return;
                var argument = new DownloadImagesQueueDoWorkArgument(this.ImagesQueue, this.FilepathManagerInstance);
                Worker.RunWorkerAsync(argument);
            }
        }
        public void StopDownloadQueue()
        {
            lock (OnOffLock)
            {
                if (!Worker.IsBusy)
                    return;
                Worker.CancelAsync();
            }
        }
        public void AddCardBaseToDownloadQueue(CardBase card, CharacterImageFormat imageFormat)
        {
            for (int level = card.minLevel; level <= card.maxLevel; level++)
            {
                ImagesQueue.Enqueue(new CharacterImageIdentifier(card, level, imageFormat));
            }
        }
        public void AddCardInstanceToDownloadQueue(CardInstance card)
        {
            ImagesQueue.Enqueue(new CharacterImageIdentifier(card, CharacterImageFormat.Color800x640));
        }
        public void CheckAndEnqueueNotDownloadedImages(CharacterImageFormat format)
        {
            List<CharacterImageIdentifier> images = new List<CharacterImageIdentifier>();
            foreach (CardBase cardBase in InMemoryManagerInstance.GetAllCardBases())
                for (int level = cardBase.minLevel; level <= cardBase.maxLevel; level++)
                    images.Add(new CharacterImageIdentifier(cardBase, level, format));

            foreach (CharacterImageIdentifier image in images)
            {
                string pathToImage = Path.Combine(FilepathManagerInstance.ImagesDirectory, image.Filename);
                if (!File.Exists(pathToImage))
                    ImagesQueue.Enqueue(image);
            }
        }
    }

    internal class ImageDownloadIdentifier
    {
        public int CardBaseId { get; set; }
        public int Level { get; set; }
        public string Url { get; set; }
        public CharacterImageFormat ImageFormat { get; set; }
    }
}