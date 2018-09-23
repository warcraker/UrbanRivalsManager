using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using UrbanRivalsManager.Model;

namespace UrbanRivalsManager.ViewModel
{
    public class GameModeConverter : IValueConverter
    {
        static GameModeConverter()
        {
            References = new Dictionary<GameMode, string>();
            foreach (GameMode mode in Enum.GetValues(typeof(GameMode)))
            {
                References.Add(mode, GetGameModeResxReference(mode));
            }
        }
        private static Dictionary<GameMode, string> References;
        private static string GetGameModeResxReference(GameMode mode)
        {
            return "ui_gamemode_" + mode.ToString().ToLower();
        }

        public Dictionary<GameMode, string> Cosas
        {
            get { return References; }
        }
        public List<GameMode> GameModes
        {
            get { return References.Keys.ToList<GameMode>(); } 
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            GameMode gamemode = (GameMode)value;
            string location;
            if (References.TryGetValue(gamemode, out location))
                return Properties.UIStrings.ResourceManager.GetString(location);
            else
                return "-";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Enum.Parse(typeof(GameMode), value.ToString());
        }

    }
}
