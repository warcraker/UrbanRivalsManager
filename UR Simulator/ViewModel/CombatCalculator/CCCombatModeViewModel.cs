using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using UrbanRivalsCore.Model;
using UrbanRivalsCore.ViewModel;
using UrbanRivalsManager.Utils;
using UrbanRivalsManager.ViewModel.DataManagement;

namespace UrbanRivalsManager.ViewModel
{
    public class CombatCalculatorCombatModeViewModel : DependencyObject
    {
        public event PropertyChangedCallback OnLeftSelectedCardChanged;
        public event PropertyChangedCallback OnRightSelectedCardChanged;
        public event PropertyChangedCallback OnLeftUsedPillzChanged;
        public event PropertyChangedCallback OnRightUsedPillzChanged;
        public event PropertyChangedCallback OnLeftUsedFuryChanged;
        public event PropertyChangedCallback OnRightUsedFuryChanged;


        public InMemoryManager InMemoryManager
        {
            get { return (InMemoryManager)GetValue(InMemoryManagerProperty); }
            set { SetValue(InMemoryManagerProperty, value); }
        }
        public static readonly DependencyProperty InMemoryManagerProperty =
            DependencyProperty.Register("InMemoryManager", typeof(InMemoryManager), typeof(CombatCalculatorCombatModeViewModel), new PropertyMetadata(null));

        public Combat Combat
        {
            get { return (Combat)GetValue(CombatProperty); }
            set { SetValue(CombatProperty, value); }
        }
        public static readonly DependencyProperty CombatProperty =
            DependencyProperty.Register("Combat", typeof(Combat), typeof(CombatCalculatorCombatModeViewModel), new PropertyMetadata(null));

        public ObservableCollection<RoundResults> InteractionResults
        {
            get { return (ObservableCollection<RoundResults>)GetValue(InteractionResultsProperty); }
            set { SetValue(InteractionResultsProperty, value); }
        }
        public static readonly DependencyProperty InteractionResultsProperty =
            DependencyProperty.Register("InteractionResults", typeof(ObservableCollection<RoundResults>), typeof(CombatCalculatorCombatModeViewModel), 
                new PropertyMetadata(new ObservableCollection<RoundResults>()));

        public int? LeftSelectedCard
        {
            get { return (int?)GetValue(LeftSelectedCardProperty); }
            set { SetValue(LeftSelectedCardProperty, value); }
        }
        public static readonly DependencyProperty LeftSelectedCardProperty =
            DependencyProperty.Register("LeftSelectedCard", typeof(int?), typeof(CombatCalculatorCombatModeViewModel), 
                new PropertyMetadata(null, LeftSelectedCardChanged), ValidateSelectedCard);
        public int? RightSelectedCard
        {
            get { return (int?)GetValue(RightSelectedCardProperty); }
            set { SetValue(RightSelectedCardProperty, value); }
        }
        public static readonly DependencyProperty RightSelectedCardProperty =
            DependencyProperty.Register("RightSelectedCard", typeof(int?), typeof(CombatCalculatorCombatModeViewModel), 
                new PropertyMetadata(null, RightSelectedCardChanged), ValidateSelectedCard);

        public int? LeftUsedPillz
        {
            get { return (int?)GetValue(LeftUsedPillzProperty); }
            set { SetValue(LeftUsedPillzProperty, value); }
        }
        public static readonly DependencyProperty LeftUsedPillzProperty = 
            DependencyProperty.Register("LeftUsedPillz", typeof(int?), typeof(CombatCalculatorCombatModeViewModel), 
                new PropertyMetadata(null, LeftUsedPillzChanged, CoerceLeftUsedPillz));
        public int? RightUsedPillz
        {
            get { return (int?)GetValue(RightUsedPillzProperty); }
            set { SetValue(RightUsedPillzProperty, value); }
        }
        public static readonly DependencyProperty RightUsedPillzProperty =
            DependencyProperty.Register("RightUsedPillz", typeof(int?), typeof(CombatCalculatorCombatModeViewModel),
            new PropertyMetadata(null, RightUsedPillzChanged, CoerceRightUsedPillz));
        public bool LeftUsedFury
        {
            get { return (bool)GetValue(LeftUsedFuryProperty); }
            set { SetValue(LeftUsedFuryProperty, value); }
        }
        public static readonly DependencyProperty LeftUsedFuryProperty =
            DependencyProperty.Register("LeftUsedFury", typeof(bool), typeof(CombatCalculatorCombatModeViewModel), new PropertyMetadata(false, LeftFuryChanged, CoerceLeftUsedFury));
        public bool RightUsedFury
        {
            get { return (bool)GetValue(RightUsedFuryProperty); }
            set { SetValue(RightUsedFuryProperty, value); }
        }
        public static readonly DependencyProperty RightUsedFuryProperty =
            DependencyProperty.Register("RightUsedFury", typeof(bool), typeof(CombatCalculatorCombatModeViewModel), new PropertyMetadata(false, RightFuryChanged, CoerceRightUsedFury));

        private static bool ValidateSelectedCard(object value)
        {
            if (value == null)
                return true;

            int card = (int)value;
            if (card >= 0 && card <= 3)
                return true;
            return false;
        }

        private static object CoerceLeftUsedPillz(DependencyObject d, object baseValue)
        {
            CombatCalculatorCombatModeViewModel vm = (CombatCalculatorCombatModeViewModel)d;

            if (vm.LeftSelectedCard == null || baseValue == null)
                return null;
            if ((int)baseValue < 0)
                return 0;

            int usedPillz = (int)baseValue;
            int maxAllowedPillz = vm.Combat.LeftPlayerStatus.Pillz - (vm.LeftUsedFury ? 3 : 0);
            Debug.Assert(maxAllowedPillz >= 0);
            return Math.Min(usedPillz, maxAllowedPillz);
        }
        private static object CoerceRightUsedPillz(DependencyObject d, object baseValue)
        {
            CombatCalculatorCombatModeViewModel vm = (CombatCalculatorCombatModeViewModel)d;

            if (vm.RightSelectedCard == null || baseValue == null)
                return null;
            if ((int)baseValue < 0)
                return 0;

            int usedPillz = (int)baseValue;
            int maxAllowedPillz = vm.Combat.RightPlayerStatus.Pillz - (vm.RightUsedFury ? 3 : 0);
            Debug.Assert(maxAllowedPillz >= 0);
            return Math.Min(usedPillz, maxAllowedPillz);
        }
        private static object CoerceLeftUsedFury(DependencyObject d, object baseValue)
        {
            if ((bool)baseValue == false)
                return false;

            CombatCalculatorCombatModeViewModel vm = (CombatCalculatorCombatModeViewModel)d;

            if (vm.LeftUsedPillz == null)
                return false;

            int usedPillz = (int)vm.LeftUsedPillz;
            int remainingPillz = vm.Combat.LeftPlayerStatus.Pillz;
            if (remainingPillz - usedPillz < 3)
                return false;

            return true;
        }
        private static object CoerceRightUsedFury(DependencyObject d, object baseValue)
        {
            if ((bool)baseValue == false)
                return false;

            CombatCalculatorCombatModeViewModel vm = (CombatCalculatorCombatModeViewModel)d;

            if (vm.RightUsedPillz == null)
                return false;

            int usedPillz = (int)vm.RightUsedPillz;
            int remainingPillz = vm.Combat.RightPlayerStatus.Pillz;
            if (remainingPillz - usedPillz < 3)
                return false;

            return true;
        }

        private static void LeftSelectedCardChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CombatCalculatorCombatModeViewModel vm = (CombatCalculatorCombatModeViewModel)d;

            if (e.NewValue == null)
                vm.LeftUsedPillz = null;

            vm.PreviewRound.Execute(null);

            if (vm.OnLeftSelectedCardChanged != null)
                vm.OnLeftSelectedCardChanged(d, e);
        }
        private static void RightSelectedCardChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CombatCalculatorCombatModeViewModel vm = (CombatCalculatorCombatModeViewModel)d;

            if (e.NewValue == null)
                vm.RightUsedPillz = null;

            vm.PreviewRound.Execute(null);

            if (vm.OnRightSelectedCardChanged != null)
                vm.OnRightSelectedCardChanged(d, e);
        }
        private static void LeftUsedPillzChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CombatCalculatorCombatModeViewModel vm = (CombatCalculatorCombatModeViewModel)d;

            if (e.NewValue == null)
                vm.LeftUsedFury = false;
            else if ((int)e.NewValue < 0)
                vm.LeftUsedPillz = null;

            vm.PreviewRound.Execute(null);

            if (vm.OnLeftUsedPillzChanged != null)
                vm.OnLeftUsedPillzChanged(d, e);
        }
        private static void RightUsedPillzChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CombatCalculatorCombatModeViewModel vm = (CombatCalculatorCombatModeViewModel)d;

            if (e.NewValue == null)
                vm.RightUsedFury = false;
            else if ((int)e.NewValue < 0)
                vm.RightUsedPillz = null;

            vm.PreviewRound.Execute(null);

            if (vm.OnRightUsedPillzChanged != null)
                vm.OnRightUsedPillzChanged(d, e);
        }
        private static void LeftFuryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CombatCalculatorCombatModeViewModel vm = (CombatCalculatorCombatModeViewModel)d;

            vm.PreviewRound.Execute(null);

            if (vm.OnLeftUsedFuryChanged != null)
                vm.OnLeftUsedFuryChanged(d, e);
        }
        private static void RightFuryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CombatCalculatorCombatModeViewModel vm = (CombatCalculatorCombatModeViewModel)d;

            vm.PreviewRound.Execute(null);

            if (vm.OnRightUsedFuryChanged != null)
                vm.OnRightUsedFuryChanged(d, e);
        }

        private RelayCommand c_SelectCard;
        // For some reason using SelectCardCanExecute() the application doesn't allow to select any card, and it
        // causes some visual bugs, so the "CanExecute" check is executed inside the "Execute" function instead.
        public ICommand SelectCard
        {
            get
            {
                if (c_SelectCard == null)
                    c_SelectCard = new RelayCommand(p => this.SelectCardExecute(p), null); 

                return c_SelectCard;
            }
        }
        private void SelectCardExecute(object param)
        {
            if (!SelectCardCanExecute(param))
                return;

            bool isLeft;
            int selectedCard;
            DecodeSelectedCard(param, out isLeft, out selectedCard);

            if (isLeft)
            {
                if (LeftSelectedCard == selectedCard)
                {
                    LeftSelectedCard = null;
                }
                else
                {
                    if (!Combat.LeftPlayerStatus.Hand.Status.HasCardBeingPlayed[selectedCard])
                    {
                        LeftSelectedCard = selectedCard;
                    }
                }
            }
            else // !isLeft
            {
                if (RightSelectedCard == selectedCard)
                {
                    RightSelectedCard = null;
                }
                else
                {
                    if (!Combat.RightPlayerStatus.Hand.Status.HasCardBeingPlayed[selectedCard])
                    {
                        RightSelectedCard = selectedCard;
                    }
                }
            }
        }
        private bool SelectCardCanExecute(object param)
        {
            return Combat.CombatWinner == CombatWinner.None;
        }

        private bool AvoidPreviewRound = false;
        private RelayCommand c_PreviewRound;
        public ICommand PreviewRound
        {
            get
            {
                if (c_PreviewRound == null)
                    c_PreviewRound = new RelayCommand(p => this.PreviewRoundExecute());

                return c_PreviewRound;
            }
        }
        private void PreviewRoundExecute()
        {
            if (AvoidPreviewRound)
                return;

            bool validLeftCard = IsPosiviteOrZero(LeftSelectedCard);
            bool validRightCard = IsPosiviteOrZero(RightSelectedCard);

            if (!validLeftCard && !validRightCard) // No card selected
            {
                ResetInteractionResults();
            }
            else if (validLeftCard && validRightCard) // Both cards selected
            {
                    SetInteractionResults(Combat.PreviewRound(
                        (int)LeftSelectedCard, (int)RightSelectedCard, LeftUsedFury, RightUsedFury, (int)LeftUsedPillz, (int)RightUsedPillz));
            }
            else if (validLeftCard) // Only Left card selected
            {
                SetInteractionResults(CalculateCardInteractionsOneVsAll(PlayerSide.Left, (int)LeftSelectedCard, (int)LeftUsedPillz, LeftUsedFury));
            }
            else // (validRightCard) // Only Right card selected
            {
                SetInteractionResults(CalculateCardInteractionsOneVsAll(PlayerSide.Right, (int)RightSelectedCard, (int)RightUsedPillz, RightUsedFury));
            }
        }

        private RelayCommand c_PlayRound;
        public ICommand PlayRound
        {
            get
            {
                if (c_PlayRound == null)
                    c_PlayRound = new RelayCommand(p => this.PlayRoundExecute(), p => this.PlayRoundCanExecute());
                
                return c_PlayRound;
            }
        }
        private void PlayRoundExecute()
        {
            Combat.PlayRound((int)LeftSelectedCard, (int)RightSelectedCard, LeftUsedFury, RightUsedFury, (int)LeftUsedPillz, (int)RightUsedPillz);
            ResetSelectedCards();
        }
        private bool PlayRoundCanExecute()
        {
            return IsPosiviteOrZero(LeftSelectedCard) && IsPosiviteOrZero(RightSelectedCard);
        }

        private RelayCommand c_RewindRound;
        public ICommand RewindRound
        {

            get
            {
                if (c_RewindRound == null)
                    c_RewindRound = new RelayCommand(p => this.RewindRoundExecute(), p => this.RewindRoundCanExecute());

                return c_RewindRound;
            }
        }
        private void RewindRoundExecute()
        {
            Combat.RewindRound();
        }
        private bool RewindRoundCanExecute()
        {
            return Combat.RoundCounter > 0;
        }

        private IEnumerable<RoundResults> CalculateCardInteractionsOneVsAll(PlayerSide attackerSide, int attackerCard, int attackerUsedPillz, bool attackerUsedFury)
        {
            PlayerStatus attacker = (attackerSide == PlayerSide.Left) ? Combat.LeftPlayerStatus : Combat.RightPlayerStatus;
            PlayerStatus defender = (attackerSide == PlayerSide.Left) ? Combat.RightPlayerStatus : Combat.LeftPlayerStatus;

            for (int defenderCard = 0; defenderCard <= 3; defenderCard++)
            {
                if (!defender.Hand.Status.HasCardBeingPlayed[defenderCard])
                    yield return CalculateCardInteractionsOneVsOne(attackerSide, attackerCard, defenderCard, attackerUsedPillz, attackerUsedFury);
            }
        }
        private RoundResults CalculateCardInteractionsOneVsOne(PlayerSide attackerSide, int cardAttackerNumber, int cardDefenderNumber, int attackerUsedPillz, bool attackerUsedFury)
        {

            PlayerStatus attacker = (attackerSide == PlayerSide.Left) ? Combat.LeftPlayerStatus : Combat.RightPlayerStatus;
            PlayerStatus defender = (attackerSide == PlayerSide.Left) ? Combat.RightPlayerStatus : Combat.LeftPlayerStatus;
            RoundResults roundResults = null;

            for (int usedPillzByDefender = 0; usedPillzByDefender <= defender.Pillz; usedPillzByDefender++)
            {
                roundResults = (attackerSide == PlayerSide.Left)
                    ? Combat.PreviewRound(cardAttackerNumber, cardDefenderNumber, attackerUsedFury, false,            attackerUsedPillz,   usedPillzByDefender)
                    : Combat.PreviewRound(cardDefenderNumber, cardAttackerNumber, false,            attackerUsedFury, usedPillzByDefender, attackerUsedPillz);

                if (roundResults.roundWinner != attackerSide)
                    break;
            }

            return roundResults;
        }

        private void DecodeSelectedCard(object param, out bool isLeft, out int selectedCard)
        {
            string parameter = param.ToString();
            isLeft = parameter[0] == 'L';
            selectedCard = int.Parse(parameter[1].ToString());
        }
        private bool IsPosiviteOrZero(int? value)
        {
            if (value == null || value < 0)
                return false;
            return true;
        }

        private void ResetSelectedCards()
        {
            AvoidPreviewRound = true;
            LeftSelectedCard = null;
            AvoidPreviewRound = false;
            RightSelectedCard = null;
        }
        private void ResetInteractionResults()
        {
            InteractionResults.Clear();
        }
        private void SetInteractionResults(RoundResults results)
        {
            ResetInteractionResults();
            InteractionResults.Add(results);
        }
        private void SetInteractionResults(IEnumerable<RoundResults> results)
        {
            ResetInteractionResults();
            foreach (RoundResults item in results)
                InteractionResults.Add(item);
        }
    }
}
