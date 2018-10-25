using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UrbanRivalsCore.ViewModel;

namespace UrbanRivalsCore.Model
{
    public class Hand : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<CardDrawed> Cards;
        private int[] Supports;


        /// <summary>
        /// Gets the card indicated.
        /// </summary>
        /// <param name="index">Position of the card on the hand. Must be a value between 0 and 3 inclusive.</param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException">Must be between 0 and 3 inclusive.</exception>
        public CardDrawed this[int index]
        {
            get
            {
                if (index < 0 || index > 3)
                    throw new IndexOutOfRangeException("Must be a value between 0 and 3 inclusive");

                return Cards[index];
            }
        }

        /// <summary>
        /// Gets the active leader skill.
        /// </summary>
        public SkillLeader Leader { get; private set; }

        /// <summary>
        /// Gets the status of played cards, pillz, rounds and fury.
        /// </summary>
        public HandStatus Status { get; private set; }

        // Constructors

        private Hand() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hand"/> class.
        /// </summary>
        /// <param name="cards">Cards drawn by the player. Must have 4 cards.</param>
        public Hand(List<CardDrawed> cards)
        {
            if (cards == null)
                throw new ArgumentNullException(nameof(cards));
            if (cards.Count != 4)
                throw new ArgumentException("Must have exactly 4 cards", nameof(cards));
            for (int i = 0; i <= 3; i++)
                if (cards[i] == null)
                    throw new ArgumentException($"No card can be null. Card #{i} is null", nameof(cards));

            Cards = new List<CardDrawed>(cards);
            Supports = CalculateSupports(Cards);
            if (Supports[(int)SupportIndex.Leader] > 1)
                ApplyCancelLeader(Cards);
            Leader = CalculateLeader(Cards);
            Status = new HandStatus();
            ClearNonActiveBonuses(Cards);
        }

        internal int GetSupportMultiplier(ClanId clanId)
        {
            return Supports[(int)ConvertClanIdToSupportIndex(clanId)];
        }

        internal Hand Copy()
        {
            Hand copy = new Hand();
            copy.Cards = this.Cards;
            copy.Leader = this.Leader;
            copy.Supports = this.Supports;
            copy.Status = this.Status.Copy();
            return copy;
        }

        private static SkillLeader CalculateLeader(List<CardDrawed> cards)
        {
            int numberOfLeaders = 0;
            SkillLeader result = SkillLeader.None;
            foreach (CardDrawed card in cards)
            {
                if (card.Ability.Leader != SkillLeader.None)
                {
                    result = card.Ability.Leader;
                    numberOfLeaders++;
                }
            }
            if (numberOfLeaders == 1)
                return result;
            return SkillLeader.None;
        }
        private static int[] CalculateSupports(List<CardDrawed> cards)
        {
            int[] supportsArray;
            int numberOfClans;

            numberOfClans = Clan.getNumberOfClans();
            supportsArray = new int[numberOfClans];
            foreach (CardDrawed card in cards)
                supportsArray[(int)ConvertClanIdToSupportIndex(card.clan.id)]++;
            return supportsArray;
        }
        private static void ApplyCancelLeader(List<CardDrawed> cards)
        {
            foreach (CardDrawed card in cards)
            {
                if (card.Ability.Leader != SkillLeader.None)
                    card.Ability = Skill.NoAbility;
            }
        }
        private static void ClearNonActiveBonuses(List<CardDrawed> cards)
        {
            for (int i = 0; i < 4; i++)
            {
                bool active = false;
                for (int j = 0; j < 4; j++)
                {
                    if (i == j)
                        continue;
                    if (cards[i].clan.id == cards[j].clan.id
                        && cards[i].cardBaseId != cards[j].cardBaseId)
                    {
                        active = true;
                        break;
                    }
                }
                if (!active)
                    cards[i].Bonus = Skill.NoBonus;
            }
        }

        private static int[] CreateLeaderWarsSupportsArray()
        {
            int[] result = new int[Clan.NumberOfClans];
            for (int i = 0; i < Clan.NumberOfClans; i++)
                result[i] = 3;
            result[(int)SupportIndex.Leader] = 0;
            return result;
        }

        private static SupportIndex ConvertClanIdToSupportIndex(ClanId clanId)
        {
            switch (clanId)
            {
                case ClanId.AllStars:
                    return SupportIndex.AllStars;
                case ClanId.Bangers:
                    return SupportIndex.Bangers;
                case ClanId.Berzerk:
                    return SupportIndex.Berzerk;
                case ClanId.Dominion:
                    return SupportIndex.Dominion;
                case ClanId.FangPiClang:
                    return SupportIndex.FangPiClang;
                case ClanId.Freaks:
                    return SupportIndex.Freaks;
                case ClanId.Frozn:
                    return SupportIndex.Frozn;
                case ClanId.GHEIST:
                    return SupportIndex.GHEIST;
                case ClanId.GhosTown:
                    return SupportIndex.GhosTown;
                case ClanId.Hive:
                    return SupportIndex.Hive;
                case ClanId.Huracan:
                    return SupportIndex.Huracan;
                case ClanId.Jungo:
                    return SupportIndex.Jungo;
                case ClanId.Junkz:
                    return SupportIndex.Junkz;
                case ClanId.LaJunta:
                    return SupportIndex.LaJunta;
                case ClanId.Leader:
                    return SupportIndex.Leader;
                case ClanId.Montana:
                    return SupportIndex.Montana;
                case ClanId.Nightmare:
                    return SupportIndex.Nightmare;
                case ClanId.Piranas:
                    return SupportIndex.Piranas;
                case ClanId.Pussycats:
                    return SupportIndex.Pussycats;
                case ClanId.Raptors:
                    return SupportIndex.Raptors;
                case ClanId.Rescue:
                    return SupportIndex.Rescue;
                case ClanId.Riots:
                    return SupportIndex.Riots;
                case ClanId.Roots:
                    return SupportIndex.Roots;
                case ClanId.Sakrohm:
                    return SupportIndex.Sakrohm;
                case ClanId.Sentinel:
                    return SupportIndex.Sentinel;
                case ClanId.Skeelz:
                    return SupportIndex.Skeelz;
                case ClanId.UluWatu:
                    return SupportIndex.UluWatu;
                case ClanId.Uppers:
                    return SupportIndex.Uppers;
                case ClanId.Vortex:
                    return SupportIndex.Vortex;
                default:
                    return SupportIndex.None;
            }
        }
    }
}
