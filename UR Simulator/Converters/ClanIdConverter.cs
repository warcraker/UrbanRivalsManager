using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using UrbanRivalsCore.Model;

namespace UrbanRivalsManager.ViewModel
{
    public class ClanIdConverter : IValueConverter
    {
        private static Dictionary<ClanId, BitmapImage> images;
        private static string baseUri;

        static ClanIdConverter()
        {
            images = new Dictionary<ClanId, BitmapImage>();
            baseUri = @"pack://application:,,,/UR Manager;component/Resources/Clans/";

            images.Add(ClanId.AllStars, GetImage("ALLSTARS.png"));
            images.Add(ClanId.Bangers, GetImage("BANGERS.png"));
            images.Add(ClanId.Berzerk, GetImage("BERZERK.png"));
            images.Add(ClanId.FangPiClang, GetImage("FANGPICLANG.png"));
            images.Add(ClanId.Freaks, GetImage("FREAKS.png"));
            images.Add(ClanId.Frozn, GetImage("FROZN.png"));
            images.Add(ClanId.GHEIST, GetImage("GHEIST.png"));
            images.Add(ClanId.Huracan, GetImage("HURACAN.png"));
            images.Add(ClanId.Jungo, GetImage("JUNGO.png"));
            images.Add(ClanId.Junkz, GetImage("JUNKZ.png"));
            images.Add(ClanId.LaJunta, GetImage("LAJUNTA.png"));
            images.Add(ClanId.Leader, GetImage("LEADER.png"));
            images.Add(ClanId.Montana, GetImage("MONTANA.png"));
            images.Add(ClanId.Nightmare, GetImage("NIGHTMARE.png"));
            images.Add(ClanId.Piranas, GetImage("PIRANAS.png"));
            images.Add(ClanId.Pussycats, GetImage("PUSSYCATS.png"));
            images.Add(ClanId.Raptors, GetImage("RAPTORS.png"));
            images.Add(ClanId.Rescue, GetImage("RESCUE.png"));
            images.Add(ClanId.Riots, GetImage("RIOTS.png"));
            images.Add(ClanId.Roots, GetImage("ROOTS.png"));
            images.Add(ClanId.Sakrohm, GetImage("SAKROHM.png"));
            images.Add(ClanId.Sentinel, GetImage("SENTINEL.png"));
            images.Add(ClanId.Skeelz, GetImage("SKEELZ.png"));
            images.Add(ClanId.UluWatu, GetImage("ULUWATU.png"));
            images.Add(ClanId.Uppers, GetImage("UPPERS.png"));
            images.Add(ClanId.Vortex, GetImage("VORTEX.png"));
        }
        private static BitmapImage GetImage(string file)
        {
            return new BitmapImage(new Uri(baseUri + file, UriKind.Absolute));
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ClanId clan;
            try
            {
                clan = (ClanId)value;
            }
            catch
            {
                return null;
            }
            BitmapImage image = null;
            images.TryGetValue(clan, out image);
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
