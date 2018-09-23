using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

using UrbanRivalsCore.Model;
using UrbanRivalsManager.ViewModel.DataManagement;

namespace UrbanRivalsManager.ViewModel
{
    public class PortraitConverter : DependencyObject, IMultiValueConverter
    {
        public ImageRetriever ImageRetriever
        {
            get { return (ImageRetriever)GetValue(ImageRetrieverProperty); }
            set { SetValue(ImageRetrieverProperty, value); }
        }
        public static readonly DependencyProperty ImageRetrieverProperty =
            DependencyProperty.Register("ImageRetriever", typeof(ImageRetriever), typeof(PortraitConverter), new PropertyMetadata(null));

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                int cardBaseId = (int)values[0];
                int level = (int)values[1];
                CharacterImageFormat format = CharacterImageFormat.Color800x640; // TODO: allow different formats
                return ImageRetriever.GetImage(cardBaseId, level, format); 
            }
            catch
            {
                return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
