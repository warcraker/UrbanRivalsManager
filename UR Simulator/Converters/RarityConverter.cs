using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UrbanRivalsCore.Model;

namespace UrbanRivalsManager.ViewModel
{
    public class RarityConverter : IValueConverter
    {
        private static Dictionary<CardRarity, Brush> Brushes;
        private static Dictionary<CardRarity, BitmapImage> Backgrounds;
        private static string ImagesPath = @"pack://application:,,,/UR Manager;component/Resources/Rarity/";

        static RarityConverter()
        {
            // TODO: Add Mythic
            Brushes = new Dictionary<CardRarity, Brush>();
            Brushes.Add(CardRarity.Common, FormBrush(Colors.Sienna, Colors.Chocolate));
            Brushes.Add(CardRarity.Uncommon, FormBrush(Colors.Gray, Colors.Silver));
            Brushes.Add(CardRarity.Rare, FormBrush(Colors.Orange, Colors.Wheat));
            Brushes.Add(CardRarity.Collector, FormBrush(Colors.Gold, Colors.Orange));
            Brushes.Add(CardRarity.Legendary, FormBrush(Colors.DarkViolet, Colors.Purple));
            Brushes.Add(CardRarity.Rebirth, FormBrush(Colors.DarkCyan, Colors.Cyan));

            Backgrounds = new Dictionary<CardRarity, BitmapImage>();
            Backgrounds.Add(CardRarity.Common, GetImage("common"));
            Backgrounds.Add(CardRarity.Uncommon, GetImage("uncommon"));
            Backgrounds.Add(CardRarity.Rare, GetImage("rare"));
            Backgrounds.Add(CardRarity.Collector, GetImage("collector"));
            Backgrounds.Add(CardRarity.Legendary, GetImage("legendary"));
            Backgrounds.Add(CardRarity.Rebirth, GetImage("rebirth"));
        }

        private static Brush FormBrush(Color color1, Color color2)
        {
            GradientStopCollection stops = new GradientStopCollection();
            stops.Add(new GradientStop(color1, 0));
            stops.Add(new GradientStop(color2, 0.4));
            stops.Add(new GradientStop(color2, 0.6));
            stops.Add(new GradientStop(color1, 1));
            return new LinearGradientBrush(stops);
        }
        private static BitmapImage GetImage(string rarity)
        {
            return new BitmapImage(new Uri(ImagesPath + rarity + ".png"));
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            CardRarity rarity = (CardRarity)value;
            string type = (string)parameter;
            switch (type)
            {
                case "Brush":
                    return Brushes[rarity];
                case "Background":
                    return Backgrounds[rarity];
                default:
                    throw new ArgumentException(nameof(parameter) + " must be either Brush or Background");
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
