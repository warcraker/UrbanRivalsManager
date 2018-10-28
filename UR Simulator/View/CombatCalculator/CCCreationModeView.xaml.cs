using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

using UrbanRivalsCore.Model;
using UrbanRivalsManager.ViewModel;

namespace UrbanRivalsManager.View
{
    public partial class CombatCalculatorCreationModeView : CustomChromeLibrary.CustomChromeWindow
    {
        public CombatCalculatorCreationModeViewModel CombatCalculatorCreationModeViewModel
        {
            get { return (CombatCalculatorCreationModeViewModel)GetValue(CombatCalculatorCreationModeViewModelProperty); }
            set { SetValue(CombatCalculatorCreationModeViewModelProperty, value); }
        }
        public static readonly DependencyProperty CombatCalculatorCreationModeViewModelProperty =
            DependencyProperty.Register("CombatCalculatorCreationModeViewModel", typeof(CombatCalculatorCreationModeViewModel), typeof(CombatCalculatorCreationModeView), new PropertyMetadata(null));

        public bool StayOnTop
        {
            get { return (bool)GetValue(StayOnTopProperty); }
            set { SetValue(StayOnTopProperty, value); }
        }
        public static readonly DependencyProperty StayOnTopProperty =
            DependencyProperty.Register("StayOnTop", typeof(bool), typeof(CombatCalculatorCreationModeView), new PropertyMetadata(false, StayOnTopChanged));
        private static void StayOnTopChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Properties.Settings.Default.CalculatorStayOnTop = (bool)e.NewValue;
            Properties.Settings.Default.Save();
        }

        public GridLength Column1Width
        {
            get { return (GridLength)GetValue(Column1WidthProperty); }
            set { SetValue(Column1WidthProperty, value); }
        }
        public static readonly DependencyProperty Column1WidthProperty =
            DependencyProperty.Register("Column1Width", typeof(GridLength), typeof(CombatCalculatorCreationModeView));
        public GridLength Column2Width
        {
            get { return (GridLength)GetValue(Column2WidthProperty); }
            set { SetValue(Column2WidthProperty, value); }
        }
        public static readonly DependencyProperty Column2WidthProperty =
            DependencyProperty.Register("Column2Width", typeof(GridLength), typeof(CombatCalculatorCreationModeView));
        public GridLength Row1Height
        {
            get { return (GridLength)GetValue(Row1HeightProperty); }
            set { SetValue(Row1HeightProperty, value); }
        }
        public static readonly DependencyProperty Row1HeightProperty =
            DependencyProperty.Register("Row1Height", typeof(GridLength), typeof(CombatCalculatorCreationModeView));
        public GridLength Row2Height
        {
            get { return (GridLength)GetValue(Row2HeightProperty); }
            set { SetValue(Row2HeightProperty, value); }
        }
        public static readonly DependencyProperty Row2HeightProperty =
            DependencyProperty.Register("Row2Height", typeof(GridLength), typeof(CombatCalculatorCreationModeView));

        public CombatCalculatorCreationModeView()
        {
            CombatCalculatorCreationModeViewModel = (CombatCalculatorCreationModeViewModel)ViewModelLocator.Locator["CombatCalculatorCreationMode"];

            StayOnTop = Properties.Settings.Default.CalculatorStayOnTop;
            Top = Properties.Settings.Default.CalculatorTopPosition;
            Left = Properties.Settings.Default.CalculatorLeftPosition;
            Width = Properties.Settings.Default.CalculatorWidth;
            Height = Properties.Settings.Default.CalculatorHeight;

            Column1Width = new GridLength(Properties.Settings.Default.CalculatorColumn1Width);
            Column2Width = new GridLength(Properties.Settings.Default.CalculatorColumn2Width);
            Row1Height = new GridLength(Properties.Settings.Default.CalculatorRow1Height);
            Row2Height = new GridLength(Properties.Settings.Default.CalculatorRow2Height);

            if (Properties.Settings.Default.CalculatorMaximized)
                WindowState = WindowState.Maximized;

            DataContext = CombatCalculatorCreationModeViewModel;
            InitializeComponent();
        }

        private void CreateCombatAndChangeToCombatMode()
        {
            CombatCalculatorCreationModeViewModel.CreateCombat.Execute(null);

            new CombatCalculatorCombatModeView().Show();
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                Properties.Settings.Default.CalculatorTopPosition = RestoreBounds.Top;
                Properties.Settings.Default.CalculatorLeftPosition = RestoreBounds.Left;
                Properties.Settings.Default.CalculatorWidth = RestoreBounds.Width;
                Properties.Settings.Default.CalculatorHeight = RestoreBounds.Height;
            }
            else
            {
                Properties.Settings.Default.CalculatorTopPosition = Top;
                Properties.Settings.Default.CalculatorLeftPosition = Left;
                Properties.Settings.Default.CalculatorWidth = Width;
                Properties.Settings.Default.CalculatorHeight = Height;
            }

            // If the user maximizes the window, then minimizes it, and then closes it while minimized, the state will be saved as not maximized
            Properties.Settings.Default.CalculatorMaximized = (WindowState == WindowState.Maximized);

            Properties.Settings.Default.CalculatorTopPosition = Top;
            Properties.Settings.Default.CalculatorLeftPosition = Left;
            Properties.Settings.Default.CalculatorWidth = Width;
            Properties.Settings.Default.CalculatorHeight = Height;

            Properties.Settings.Default.CalculatorColumn1Width = Column1Width.Value;
            Properties.Settings.Default.CalculatorColumn2Width = Column2Width.Value;
            Properties.Settings.Default.CalculatorRow1Height = Row1Height.Value;
            Properties.Settings.Default.CalculatorRow2Height = Row2Height.Value;

            Properties.Settings.Default.Save();
        }
        private void CreateCombat_Click(object sender, RoutedEventArgs e)
        {
            CreateCombatAndChangeToCombatMode();
        }

        private void FillValues(object sender, RoutedEventArgs e)
        {
            CardDefinition card = CombatCalculatorCreationModeViewModel.InMemoryManager.GetCardDefinition(444);
            int level = card.maxLevel;
            CombatCalculatorCreationModeViewModel.CreationLeftCards[0] = card;
            CombatCalculatorCreationModeViewModel.CreationLeftCards[1] = card;
            CombatCalculatorCreationModeViewModel.CreationLeftCards[2] = card;
            CombatCalculatorCreationModeViewModel.CreationLeftCards[3] = card;
            CombatCalculatorCreationModeViewModel.CreationRightCards[0] = card;
            CombatCalculatorCreationModeViewModel.CreationRightCards[1] = card;
            CombatCalculatorCreationModeViewModel.CreationRightCards[2] = card;
            CombatCalculatorCreationModeViewModel.CreationRightCards[3] = card;
            CombatCalculatorCreationModeViewModel.CreationLeftLevels[0] = level;
            CombatCalculatorCreationModeViewModel.CreationLeftLevels[1] = level;
            CombatCalculatorCreationModeViewModel.CreationLeftLevels[2] = level;
            CombatCalculatorCreationModeViewModel.CreationLeftLevels[3] = level;
            CombatCalculatorCreationModeViewModel.CreationRightLevels[0] = level;
            CombatCalculatorCreationModeViewModel.CreationRightLevels[1] = level;
            CombatCalculatorCreationModeViewModel.CreationRightLevels[2] = level;
            CombatCalculatorCreationModeViewModel.CreationRightLevels[3] = level;
        }

        private void StayOnTop_Click(object sender, RoutedEventArgs e)
        {
            StayOnTop = !StayOnTop;
        }
    }
}
