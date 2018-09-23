using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using UrbanRivalsManager.Utils;

namespace UrbanRivalsManager.ViewModel
{
    public abstract class DeckBaseDialogViewModel : DependencyObject
    {
        protected DeckEditorViewModel DeckEditorViewModel;
        public virtual string Title { get { return "Title"; } }
        public virtual string ButtonText { get { return "Action"; } }
        public virtual bool EditableName { get { return false; } }

        public string DeckName
        {
            get { return (string)GetValue(DeckNameProperty); }
            set { SetValue(DeckNameProperty, value); }
        }
        public static readonly DependencyProperty DeckNameProperty =
            DependencyProperty.Register("DeckName", typeof(string), typeof(SaveViewModel), new PropertyMetadata(null));

        protected RelayCommand c_Action;
        public ICommand Action
        {
            get
            {
                if (c_Action == null)
                    c_Action = new RelayCommand(p => this.ActionExecute(), p => this.ActionCanExecute());

                return c_Action;
            }
        }
        protected virtual void ActionExecute() { }
        private bool ActionCanExecute()
        {
            return !(DeckName == null || DeckName == "");
        }

        protected DeckBaseDialogViewModel() { }
        public DeckBaseDialogViewModel(DeckEditorViewModel deckEditorViewModel)
        {
            if (deckEditorViewModel == null)
                throw new NullReferenceException("deckEditorViewModel");

            DeckEditorViewModel = deckEditorViewModel;
        }
    }
}
