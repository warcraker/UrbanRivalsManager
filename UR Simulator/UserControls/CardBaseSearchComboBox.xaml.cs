using System;
using System.Diagnostics;
using System.Collections;
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
using UrbanRivalsManager.ViewModel;
using UrbanRivalsManager.ViewModel.DataManagement;
using UrbanRivalsManager.Utils;

namespace UrbanRivalsManager.UserControls
{
    /// <summary>
    /// Interaction logic for CardBaseSearchComboBox.xaml
    /// </summary>
    public partial class CardBaseSearchComboBox : UserControl
    {
        public InMemoryManager InMemoryManager
        {
            get { return (InMemoryManager)GetValue(InMemoryManagerProperty); }
            set { SetValue(InMemoryManagerProperty, value); }
        }
        public static readonly DependencyProperty InMemoryManagerProperty =
            DependencyProperty.Register("InMemoryManager", typeof(InMemoryManager), typeof(CardBaseSearchComboBox), new PropertyMetadata(null));        

        public CardBase SelectedCard
        {
            get { return (CardBase)GetValue(SelectedCardProperty); }
            set { SetValue(SelectedCardProperty, value); }
        }
        public static readonly DependencyProperty SelectedCardProperty =
            DependencyProperty.Register("SelectedCard", typeof(CardBase), typeof(CardBaseSearchComboBox), 
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,SelectedCardChanged));

        private static void SelectedCardChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CardBaseSearchComboBox instance = (CardBaseSearchComboBox)d;
            CardBase newCard = (CardBase)e.NewValue;
            if (newCard == null)
                instance.SearchBox.SetTextWithoutSearching(instance.DefaultText);
            else
                instance.SearchBox.SetTextWithoutSearching(newCard.name);
        }

        public int MinimumSearchChars
        {
            get { return (int)GetValue(MinimumSearchCharsProperty); }
            set { SetValue(MinimumSearchCharsProperty, value); }
        }
        public static readonly DependencyProperty MinimumSearchCharsProperty =
            DependencyProperty.Register("MinimumSearchChars", typeof(int), typeof(CardBaseSearchComboBox), new FrameworkPropertyMetadata(3), MinimumSearchCharsValidate);
        private static bool MinimumSearchCharsValidate(object value)
        {
            if ((int)value < 1)
                return false;
            return true;
        }

        public string DefaultText { get { return String.Format(Properties.UIStrings.ui_searchcard_defaulttext, MinimumSearchChars); } }

        public CardBaseSearchComboBox()
        {
            InitializeComponent();
            SearchBox.SetTextWithoutSearching(DefaultText);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchBox.IsSearchNeeded)
            {
                if (SearchBox.Text.Length >= MinimumSearchChars)
                    SearchBox.ItemsSource = Search(SearchBox.Text);
                else
                    SearchBox.ItemsSource = null;
            }
        }
        private void SearchBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (SelectedCard == null)
                SearchBox.SetTextWithoutSearching(DefaultText);
            else
                SearchBox.SetTextWithoutSearching(SelectedCard.name);
        }
        private void SearchBox_DropDownClosed(object sender, EventArgs e)
        {
            if (SearchBox.SelectedIndex != -1)
                SelectedCard = (CardBase)SearchBox.SelectedItem;
            if (SelectedCard != null)
                SearchBox.Text = SelectedCard.name;
        }

        private IEnumerable<CardBase> Search(string partialName)
        {
            if (InMemoryManager == null || partialName.Length < MinimumSearchChars)
                yield break;

            foreach (string name in InMemoryManager.LookForCardNames(partialName))
                yield return InMemoryManager.GetCardBase(name);
        }
    }

    internal class SearchComboBox : ComboBox
    {
        internal bool IsSearchNeeded = true;

        internal void SetTextWithoutSearching(string text)
        {
            IsSearchNeeded = false;
            Text = text;
            IsSearchNeeded = true;
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            // Empty on purpose
        }
        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            Text = "";
            IsDropDownOpen = true;
            base.OnGotKeyboardFocus(e);
        }
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Tab || e.Key == Key.Enter)
            {
                // If user confirms with Enter or Tab when no item is selected, it means he wants to select the first item if there is one
                if (SelectedIndex == -1 && HasItems)
                    SelectedIndex = 0; 
                IsDropDownOpen = false;
            }
            else if (e.Key == Key.Escape)
            {
                SelectedIndex = -1;
                IsDropDownOpen = false;
            }
            else
            {
                if (e.Key == Key.Down)
                    IsDropDownOpen = true;
                base.OnPreviewKeyDown(e);
            }
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (!(e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Tab || e.Key == Key.Enter))
                base.OnKeyUp(e);
        }
    }
}