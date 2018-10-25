using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// <summary>
    /// Interaction logic for CardBaseSelector.xaml
    /// </summary>
    public partial class CardBaseSelector : UserControl
    {
        public InMemoryManager InMemoryManager
        {
            get { return (InMemoryManager)GetValue(InMemoryManagerProperty); }
            set { SetValue(InMemoryManagerProperty, value); }
        }
        public static readonly DependencyProperty InMemoryManagerProperty =
            DependencyProperty.Register("InMemoryManager", typeof(InMemoryManager), typeof(CardBaseSelector), new PropertyMetadata(null));

        public CardBase SelectedCardBase
        {
            get { return (CardBase)GetValue(SelectedCardBaseProperty); }
            set { SetValue(SelectedCardBaseProperty, value); }
        }
        public static readonly DependencyProperty SelectedCardBaseProperty =
            DependencyProperty.Register("SelectedCardBase", typeof(CardBase), typeof(CardBaseSelector), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(SelectedCardBaseChanged)));
        private static void SelectedCardBaseChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CardBaseSelector)d;
            if (e.NewValue == null)
                control.Level = null;
            else
                control.Level = ((CardBase)e.NewValue).maxLevel;
        }

        public int? Level
        {
            get { return (int?)GetValue(LevelProperty); }
            set { SetValue(LevelProperty, value); }
        }
        public static readonly DependencyProperty LevelProperty =
            DependencyProperty.Register("Level", typeof(int?), typeof(CardBaseSelector), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, new CoerceValueCallback(LevelCoerce)));
        private static object LevelCoerce(DependencyObject d, object baseValue)
        {
            var control = ((CardBaseSelector)d);
            var card = control.SelectedCardBase;

            if (card == null) 
                return null;

            var level = (int?)baseValue;
            if (level.HasValue)
                return Math.Min(Math.Max(level.Value, card.minLevel), card.maxLevel);

            return control.Level;
        }
        
        public CardBaseSelector()
        {
            InitializeComponent();
        }
    }
}
