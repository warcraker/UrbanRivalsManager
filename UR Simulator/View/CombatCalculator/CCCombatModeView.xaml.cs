using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

using UrbanRivalsManager.ViewModel;
using UrbanRivalsManager.Utils;
using XC = Xceed.Wpf.Toolkit;
using System;
using System.Windows.Threading;

namespace UrbanRivalsManager.View
{
    public partial class CombatCalculatorCombatModeView : CustomChromeLibrary.CustomChromeWindow
    {
        public CombatCalculatorCombatModeViewModel CombatCalculatorCombatModeViewModel
        {
            get { return (CombatCalculatorCombatModeViewModel)GetValue(CombatCalculatorCombatModeViewModelProperty); }
            set { SetValue(CombatCalculatorCombatModeViewModelProperty, value); }
        }
        public static readonly DependencyProperty CombatCalculatorCombatModeViewModelProperty =
            DependencyProperty.Register("CombatCalculatorCombatModeViewModel", typeof(CombatCalculatorCombatModeViewModel), typeof(CombatCalculatorCombatModeView), new PropertyMetadata(null));

        public bool StayOnTop
        {
            get { return (bool)GetValue(StayOnTopProperty); }
            set { SetValue(StayOnTopProperty, value); }
        }
        public static readonly DependencyProperty StayOnTopProperty =
            DependencyProperty.Register("StayOnTop", typeof(bool), typeof(CombatCalculatorCombatModeView), new PropertyMetadata(true, StayOnTopChanged));
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
            DependencyProperty.Register("Column1Width", typeof(GridLength), typeof(CombatCalculatorCombatModeView));
        public GridLength Column2Width
        {
            get { return (GridLength)GetValue(Column2WidthProperty); }
            set { SetValue(Column2WidthProperty, value); }
        }
        public static readonly DependencyProperty Column2WidthProperty =
            DependencyProperty.Register("Column2Width", typeof(GridLength), typeof(CombatCalculatorCombatModeView));
        public GridLength Row1Height
        {
            get { return (GridLength)GetValue(Row1HeightProperty); }
            set { SetValue(Row1HeightProperty, value); }
        }
        public static readonly DependencyProperty Row1HeightProperty =
            DependencyProperty.Register("Row1Height", typeof(GridLength), typeof(CombatCalculatorCombatModeView));
        public GridLength Row2Height
        {
            get { return (GridLength)GetValue(Row2HeightProperty); }
            set { SetValue(Row2HeightProperty, value); }
        }
        public static readonly DependencyProperty Row2HeightProperty =
            DependencyProperty.Register("Row2Height", typeof(GridLength), typeof(CombatCalculatorCombatModeView));

        public CombatCalculatorCombatModeView()
        {
            CombatCalculatorCombatModeViewModel = (CombatCalculatorCombatModeViewModel)ViewModelLocator.Locator["CombatCalculatorCombatMode"];

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

            CombatCalculatorCombatModeViewModel.OnRightUsedPillzChanged += OnRightUsedPillzChanged;
            CombatCalculatorCombatModeViewModel.OnLeftUsedPillzChanged += OnLeftUsedPillzChanged;
            CombatCalculatorCombatModeViewModel.OnRightSelectedCardChanged += OnRightSelectedCardChanged;
            CombatCalculatorCombatModeViewModel.OnLeftSelectedCardChanged += OnLeftSelectedCardChanged;

            DataContext = CombatCalculatorCombatModeViewModel;
            InitializeComponent();
        }

        private void OnRightSelectedCardChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EnsureFocus(RightPillz);
        }
        private void OnLeftSelectedCardChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EnsureFocus(LeftPillz);
        }
        private void OnRightUsedPillzChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CombatCalculatorCombatModeViewModel vm = (CombatCalculatorCombatModeViewModel)d;

            bool canFuryBeUsed = vm.Combat.RightPlayerStatus.Pillz - vm.RightUsedPillz >= 3;

            if (RightFury.IsEnabled && !canFuryBeUsed)
                RightFury.IsEnabled = false;
            else if (!RightFury.IsEnabled && canFuryBeUsed)
                RightFury.IsEnabled = true;
        }
        private void OnLeftUsedPillzChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CombatCalculatorCombatModeViewModel vm = (CombatCalculatorCombatModeViewModel)d;

            bool canFuryBeUsed = vm.Combat.LeftPlayerStatus.Pillz - vm.LeftUsedPillz >= 3;

            if (LeftFury.IsEnabled && !canFuryBeUsed)
                LeftFury.IsEnabled = false;
            else if (!LeftFury.IsEnabled && canFuryBeUsed)
                LeftFury.IsEnabled = true;
        }
        private void PlayerStatus_GotFocus(object sender, RoutedEventArgs e)
        {
            ClearSelections();
        }
        private void UsedPillzEnabled(object sender, DependencyPropertyChangedEventArgs e)
        {
            XC.IntegerUpDown control = ((XC.IntegerUpDown)sender);
            control.Value = 0;
            EnsureFocus(control);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ClearSelections();
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
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CreateNewCombat_Click(object sender, RoutedEventArgs e)
        {
            ChangeToCreationMode();
        }

        private void ChangeToCreationMode()
        {
            new CombatCalculatorCreationModeView().Show();
            this.Close();
        }
        private void ClearSelections()
        {
            CombatCalculatorCombatModeViewModel.LeftSelectedCard = null;
            CombatCalculatorCombatModeViewModel.RightSelectedCard = null;
        }

        private void EnsureFocus(IInputElement control)
        {
            if (control.IsEnabled)
            {
                Keyboard.Focus(Window);
                Dispatcher.BeginInvoke(new Action(() => 
                    {
                        Keyboard.Focus(control);
                    }),
                    DispatcherPriority.ApplicationIdle);
            }
        }

        private void StayOnTop_Click(object sender, RoutedEventArgs e)
        {
            StayOnTop = !StayOnTop;
        }
    }
}
