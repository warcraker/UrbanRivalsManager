using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.IO;


namespace UrbanRivalsManager.ViewModel.DataManagement
{
    public class ImageRetriever
    {
        private FilepathManager FilepathManager;
        private ImageCache ImageCache;

        private ImageRetriever() { }
        public ImageRetriever(FilepathManager filepathManager, ImageCache imageCache)
        {
            FilepathManager = filepathManager;
            ImageCache = imageCache;
        }

        public BitmapImage GetImage(int cardBaseId, int level, CharacterImageFormat format)
        {
            BitmapImage result = ImageCache.GetImage(cardBaseId, level, format);

            if (result == null)
            {
                result = GetImageFromFile(cardBaseId, level, format);
                if (result != null)
                    ImageCache.SetImage(cardBaseId, level, result, format);
            }

            return result;
        }

        private BitmapImage GetImageFromFile(int cardBaseId, int level, CharacterImageFormat format)
        {
            string formatString = "";
            switch (format)
            {
                case CharacterImageFormat.Color800x640:
                    formatString = CharacterImageFileFormatString.Color800x640;
                    break;
                case CharacterImageFormat.Color140x112:
                    formatString = CharacterImageFileFormatString.Color140x112;
                    break;
                case CharacterImageFormat.Gray140x112:
                    formatString = CharacterImageFileFormatString.Gray140x112;
                    break;
            }
            string filename = String.Format(formatString, cardBaseId, level);
            string fullPath = Path.Combine(FilepathManager.ImagesDirectory, filename);

            try
            {
                return new BitmapImage(new Uri(fullPath));
            }
            catch
            {
                return null;
            }
        }
    }
}
