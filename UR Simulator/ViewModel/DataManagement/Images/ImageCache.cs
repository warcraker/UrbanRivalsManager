using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Runtime.Caching;
using System.Collections.Specialized;

namespace UrbanRivalsManager.ViewModel.DataManagement
{
    public class ImageCache
    {
        private MemoryCache Cache;
        private CacheItemPolicy ItemPolicy;

        public ImageCache(int cacheMemoryLimitInMegabytes, TimeSpan slidingExpiration)
        {
            NameValueCollection config = new NameValueCollection();
            config.Add("CacheMemoryLimitMegabytes", cacheMemoryLimitInMegabytes.ToString());
            Cache = new MemoryCache("ImageCache", config);

            ItemPolicy = new CacheItemPolicy();
            ItemPolicy.SlidingExpiration = slidingExpiration;
        }

        public BitmapImage GetImage(int cardBaseId, int level, CharacterImageFormat format)
        {
            string identifier = GetId(cardBaseId, level, format);
            return Cache.Get(identifier) as BitmapImage;
        }

        public void SetImage(int cardBaseId, int level, BitmapImage image, CharacterImageFormat format)
        {
            string identifier = GetId(cardBaseId, level, format);
            Cache.Set(identifier, image, ItemPolicy);
        }

        private string GetId(int cardBaseId, int level, CharacterImageFormat format)
        {
            return cardBaseId.ToString() + level.ToString() + format.ToString();
        }
    }
}
