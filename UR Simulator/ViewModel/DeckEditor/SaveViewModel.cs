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
    public class SaveViewModel : DeckBaseDialogViewModel
    {
        public override string Title { get { return ""; } }
        public override bool EditableName { get { return true; } }
        protected override void ActionExecute()
        {
        }
    }
}
