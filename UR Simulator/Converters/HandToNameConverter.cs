using System;
using System.Windows.Data;
using UrbanRivalsCore.Model;
using UrbanRivalsCore.ViewModel;

namespace UrbanRivalsManager.ViewModel
{
    class HandToNameConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Hand hand = (Hand)values[0];
            int selectedCard = (int)values[1];
            return hand[selectedCard].name;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
