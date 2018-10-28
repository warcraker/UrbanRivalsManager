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
    public partial class CardDefinitionSelector : UserControl
    {
        public InMemoryManager InMemoryManager
        {
            get { return (InMemoryManager)GetValue(InMemoryManagerProperty); }
            set { SetValue(InMemoryManagerProperty, value); }
        }
        public static readonly DependencyProperty InMemoryManagerProperty =
            DependencyProperty.Register("InMemoryManager", typeof(InMemoryManager), typeof(CardDefinitionSelector), new PropertyMetadata(null));

        public CardDefinition SelectedCardDefinition
        {
            get { return (CardDefinition)GetValue(SelectedCardDefinitionProperty); }
            set { SetValue(SelectedCardDefinitionProperty, value); }
        }
        public static readonly DependencyProperty SelectedCardDefinitionProperty =
            DependencyProperty.Register(nameof(SelectedCardDefinition), typeof(CardDefinition), typeof(CardDefinitionSelector), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(SelectedCardBaseChanged)));
        private static void SelectedCardBaseChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CardDefinitionSelector)d;
            if (e.NewValue == null)
                control.Level = null;
            else
                control.Level = ((CardDefinition)e.NewValue).maxLevel;
        }

        public int? Level
        {
            get { return (int?)GetValue(LevelProperty); }
            set { SetValue(LevelProperty, value); }
        }
        public static readonly DependencyProperty LevelProperty =
            DependencyProperty.Register(nameof(Level), typeof(int?), typeof(CardDefinitionSelector), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, new CoerceValueCallback(LevelCoerce)));
        private static object LevelCoerce(DependencyObject d, object baseValue)
        {
            var control = ((CardDefinitionSelector)d);
            var card = control.SelectedCardDefinition;

            if (card == null) 
                return null;

            var level = (int?)baseValue;
            if (level.HasValue)
                return Math.Min(Math.Max(level.Value, card.minLevel), card.maxLevel);

            return control.Level;
        }
        
        public CardDefinitionSelector()
        {
            InitializeComponent();
        }
    }
}
