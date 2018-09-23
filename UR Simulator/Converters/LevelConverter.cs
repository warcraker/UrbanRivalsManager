using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using UrbanRivalsCore.Model;

namespace UrbanRivalsManager.ViewModel
{
    public class LevelConverter : IMultiValueConverter
    {
        private static Dictionary<int, BitmapImage> images;
        private static string baseUri;

        static LevelConverter()
        {
            images = new Dictionary<int, BitmapImage>();
            baseUri = @"pack://application:,,,/UR Manager;component/Resources/Levels/";

            images.Add(12, GetImage("12.png"));
            images.Add(13, GetImage("13.png"));
            images.Add(14, GetImage("14.png"));
            images.Add(15, GetImage("15.png"));
            images.Add(22, GetImage("22.png"));
            images.Add(23, GetImage("23.png"));
            images.Add(24, GetImage("24.png"));
            images.Add(25, GetImage("25.png"));
            images.Add(33, GetImage("33.png"));
            images.Add(34, GetImage("34.png"));
            images.Add(35, GetImage("35.png"));
            images.Add(44, GetImage("44.png"));
            images.Add(45, GetImage("45.png"));
            images.Add(55, GetImage("55.png"));
            
        }
        private static BitmapImage GetImage(string file)
        {
            return new BitmapImage(new Uri(baseUri + file, UriKind.Absolute));
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int level, max;
            try
            { 
                level = (int)values[0];
                max = (int)values[1];
            }
            catch
            {
                return null;
            }
            BitmapImage image = null;
            images.TryGetValue(level * 10 + max, out image);
            return image;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
