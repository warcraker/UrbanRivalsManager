using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using UrbanRivalsCore.Model;
using UrbanRivalsManager.ViewModel.DataManagement;

namespace UrbanRivalsManager.UserControls
{
    public partial class CardDrawedView : UserControl
    {
        public CardDrawed CardDrawed
        {
            get { return (CardDrawed)GetValue(CardDrawedProperty); }
            set { SetValue(CardDrawedProperty, value); }
        }
        public static readonly DependencyProperty CardDrawedProperty =
            DependencyProperty.Register("CardDrawed", typeof(CardDrawed), typeof(CardDrawedView), new PropertyMetadata(null));

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(CardDrawedView), new PropertyMetadata(false)); //new PropertyMetadata(false, null, CoerceIsSelected));
        private static object CoerceIsSelected(DependencyObject d, object baseValue)
        {
            CardDrawedView control = (CardDrawedView)d;
            if (control.HasBattled)
                return false;
            return baseValue;
        }

        public bool HasBattled
        {
            get { return (bool)GetValue(HasBattledProperty); }
            set { SetValue(HasBattledProperty, value); }
        }
        public static readonly DependencyProperty HasBattledProperty =
            DependencyProperty.Register("HasBattled", typeof(bool), typeof(CardDrawedView), new PropertyMetadata(false));// new PropertyMetadata(false, HasBattledChanged));
        private static void HasBattledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CardDrawedView control = (CardDrawedView)d;
            if ((bool)e.NewValue)
                control.IsSelected = false;
        }

        public bool? HasWon
        {
            get { return (bool?)GetValue(HasWonProperty); }
            set { SetValue(HasWonProperty, value); }
        }
        public static readonly DependencyProperty HasWonProperty =
            DependencyProperty.Register("HasWon", typeof(bool?), typeof(CardDrawedView), new PropertyMetadata(null));

        public bool? HasUsedFury
        {
            get { return (bool?)GetValue(HasUsedFuryProperty); }
            set { SetValue(HasUsedFuryProperty, value); }
        }
        public static readonly DependencyProperty HasUsedFuryProperty =
            DependencyProperty.Register("HasUsedFury", typeof(bool?), typeof(CardDrawedView), new PropertyMetadata(null));

        public int? UsedPillz
        {
            get { return (int?)GetValue(UsedPillzProperty); }
            set { SetValue(UsedPillzProperty, value); }
        }
        public static readonly DependencyProperty UsedPillzProperty =
            DependencyProperty.Register("UsedPillz", typeof(int?), typeof(CardDrawedView), new PropertyMetadata(null));

        public CardDrawedView()
        {
            InitializeComponent();
        }
    }
}
