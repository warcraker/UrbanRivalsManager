using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using UrbanRivalsCore.Model;
using UrbanRivalsCore.ViewModel;
using UrbanRivalsManager.Utils;
using UrbanRivalsManager.Model;
using UrbanRivalsManager.ViewModel.DataManagement;

namespace UrbanRivalsManager.ViewModel
{
    public class CombatCalculatorCreationModeViewModel : DependencyObject
    {
        public CombatCalculatorCreationModeViewModel() { }

        public InMemoryManager InMemoryManager
        {
            get { return (InMemoryManager)GetValue(InMemoryManagerProperty); }
            set { SetValue(InMemoryManagerProperty, value); }
        }
        public static readonly DependencyProperty InMemoryManagerProperty =
            DependencyProperty.Register("InMemoryManager", typeof(InMemoryManager), typeof(CombatCalculatorCreationModeViewModel), new PropertyMetadata(null));

        public GameMode GameMode
        {
            get { return (GameMode)GetValue(GameModeProperty); }
            set { SetValue(GameModeProperty, value); }
        }
        public static readonly DependencyProperty GameModeProperty =
            DependencyProperty.Register("GameMode", typeof(GameMode), typeof(CombatCalculatorCreationModeViewModel), new PropertyMetadata(GameMode.Classic));

        public bool HasInitiative
        {
            get { return (bool)GetValue(InitiativeProperty); }
            set { SetValue(InitiativeProperty, value); }
        }
        public static readonly DependencyProperty InitiativeProperty =
            DependencyProperty.Register("HasInitiative", typeof(bool), typeof(CombatCalculatorCreationModeViewModel), new PropertyMetadata(false));

        public bool IsRandom
        {
            get { return (bool)GetValue(IsRandomProperty); }
            set { SetValue(IsRandomProperty, value); }
        }
        public static readonly DependencyProperty IsRandomProperty =
            DependencyProperty.Register("IsRandom", typeof(bool), typeof(CombatCalculatorCreationModeViewModel), new PropertyMetadata(false));

        public ObservableCollection<CardBase> CreationLeftCards
        {
            get { return (ObservableCollection<CardBase>)GetValue(CreationLeftCardsProperty); }
            set { SetValue(CreationLeftCardsProperty, value); }
        }
        public static readonly DependencyProperty CreationLeftCardsProperty =
            DependencyProperty.Register("CreationLeftCards", typeof(ObservableCollection<CardBase>), typeof(CombatCalculatorCreationModeViewModel), new PropertyMetadata(new ObservableCollection<CardBase>() { null, null, null, null }));

        public ObservableCollection<CardBase> CreationRightCards
        {
            get { return (ObservableCollection<CardBase>)GetValue(CreationRightCardsProperty); }
            set { SetValue(CreationRightCardsProperty, value); }
        }
        public static readonly DependencyProperty CreationRightCardsProperty =
            DependencyProperty.Register("CreationRightCards", typeof(ObservableCollection<CardBase>), typeof(CombatCalculatorCreationModeViewModel), new PropertyMetadata(new ObservableCollection<CardBase>() { null, null, null, null }));

        public ObservableCollection<int?> CreationLeftLevels
        {
            get { return (ObservableCollection<int?>)GetValue(CreationLeftLevelsProperty); }
            set { SetValue(CreationLeftLevelsProperty, value); }
        }
        public static readonly DependencyProperty CreationLeftLevelsProperty =
            DependencyProperty.Register("CreationLeftLevels", typeof(ObservableCollection<int?>), typeof(CombatCalculatorCreationModeViewModel), new PropertyMetadata(new ObservableCollection<int?>() { null, null, null, null }));

        public ObservableCollection<int?> CreationRightLevels
        {
            get { return (ObservableCollection<int?>)GetValue(CreationRightLevelsProperty); }
            set { SetValue(CreationRightLevelsProperty, value); }
        }
        public static readonly DependencyProperty CreationRightLevelsProperty =
            DependencyProperty.Register("CreationRightLevels", typeof(ObservableCollection<int?>), typeof(CombatCalculatorCreationModeViewModel), new PropertyMetadata(new ObservableCollection<int?>() { null, null, null, null }));

        RelayCommand c_CreateCombat;
        public ICommand CreateCombat
        {
            get
            {
                if (c_CreateCombat == null)
                    c_CreateCombat = new RelayCommand(p => this.CreateCombatExecute(), p => this.CreateCombatCanExecute());

                return c_CreateCombat;
            }
        }
        private void CreateCombatExecute() 
        {
            Combat combat = null;
            RandomFactor isRandom = (IsRandom) ? RandomFactor.Random : RandomFactor.NonRandom;

            List<CardDrawed> leftCards = new List<CardDrawed>();
            for (int i = 0; i < 4; i++)
			{
                CardInstance instance = InMemoryManager.GetFakeCardInstance(CreationLeftCards[i].cardBaseId, CreationLeftLevels[i].Value);
                //CardInstance instance = new CardInstance(CreationLeftCards[i], 0, CreationLeftLevels[i].Value);
                leftCards.Add(new CardDrawed(instance));
			}
            Hand leftHand = new Hand(leftCards);
 
            List<CardDrawed> rightCards = new List<CardDrawed>();
            for (int i = 0; i < 4; i++)
			{
                CardInstance instance = InMemoryManager.GetFakeCardInstance(CreationRightCards[i].cardBaseId, CreationRightLevels[i].Value);
                //CardInstance instance = new CardInstance(CreationRightCards[i], 0, CreationRightLevels[i].Value);
                rightCards.Add(new CardDrawed(instance));
			}
            Hand rightHand = new Hand(rightCards);

            switch (GameMode)
            {
                case GameMode.Tourney:
                    combat = CombatFactory.GetTourneyCombat(leftHand, rightHand, HasInitiative, isRandom);
                    break;
                case GameMode.Classic:
                    combat = CombatFactory.GetClassicCombat(leftHand, rightHand, HasInitiative, isRandom);
                    break;
                case GameMode.Solo:
                    combat = CombatFactory.GetSoloCombat(leftHand, rightHand, HasInitiative, isRandom);
                    break;
                case GameMode.LeaderWars:
                    throw new NotImplementedException();
                    //break;
                case GameMode.ELO:
                    combat = CombatFactory.GetEloCombat(leftHand, rightHand, HasInitiative);
                    break;
                case GameMode.Survivor:
                    throw new NotImplementedException();
                    //break;
                case GameMode.Duel:
                    combat = CombatFactory.GetDuelCombat(leftHand, rightHand, HasInitiative, isRandom);
                    break;
                case GameMode.Custom:
                    throw new NotImplementedException();
                    //break;
            }

            ((CombatCalculatorCombatModeViewModel)ViewModelLocator.Locator["CombatCalculatorCombatMode"]).Combat = combat;
        }
        private bool CreateCombatCanExecute()
        {
            foreach (var card in CreationLeftCards)
                if (card == null)
                    return false;

            foreach (var card in CreationRightCards)
                if (card == null)
                    return false;

            return true;
        }
    }
}
