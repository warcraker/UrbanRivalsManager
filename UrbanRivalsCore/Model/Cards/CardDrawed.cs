using System;
using System.ComponentModel;
using UrbanRivalsUtils;

namespace UrbanRivalsCore.Model
{
    public class CardDrawed : IComparable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // TODO: Revise if set is needed
        public Skill ability
        {
            get
            {
                return this.m_ability;
            }
            set
            {
                if (value == null)
                    m_ability = Skill.NO_ABILITY;
                else
                    m_ability = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(ability)));
            }
        }
        // TODO: Revise if set is needed    
        public Skill bonus
        {
            get
            {
                return m_bonus;
            }
            set
            {
                if (value == null)
                    m_bonus = Skill.NO_BONUS;
                else
                    m_bonus = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(bonus)));
            }
        }

        public readonly int cardInstanceId;
        public readonly int cardBaseId;
        public readonly string name;
        public readonly Clan clan;
        public readonly int level;
        public readonly int experience;
        public readonly int maxLevel;
        public readonly int power;
        public readonly int damage;
        public readonly CardRarity rarity;

        private Skill m_bonus;
        private Skill m_ability;

        public CardDrawed(CardInstance cardInstance)
        {
            Clan clan;

            AssertArgument.isNotNull(cardInstance, nameof(cardInstance));

            this.cardInstanceId = cardInstance.cardInstanceId;
            this.cardBaseId = cardInstance.cardBaseId;
            this.name = cardInstance.name;
            this.clan = cardInstance.clan;
            this.level = cardInstance.level;
            this.experience = cardInstance.experience;
            this.maxLevel = cardInstance.maxLevel;
            this.damage = cardInstance.damage;
            this.power = cardInstance.power;
            this.rarity = cardInstance.rarity;

            this.ability = cardInstance.ability;
            clan = cardInstance.clan;
            this.bonus = clan.bonus.Copy();
        }

        public override string ToString()
        {
            return $"[@{this.cardInstanceId}] {this.name}";
        }
        public int CompareTo(object obj)
        {
            int result;

            if (obj == null)
            {
                result = 1;
            }
            else
            {
                CardDrawed a, b;

                a = this;
                try
                {
                    b = (CardDrawed)obj;
                }
                catch
                {
                    b = null;
                    AssertArgument.fail("Is not a CardDrawed instance", nameof(obj));
                }

                result = a.clan.name.CompareTo(b.clan.name);
                if (result == 0)
                {
                    result = a.name.CompareTo(b.name);
                    if (result == 0)
                    {
                        result = a.level.CompareTo(b.level);
                        if (result == 0)
                        {
                            result = a.experience.CompareTo(b.experience);
                        }
                    }
                }
            }

            return result;
        }
        public static int CompareTo(CardDrawed a, CardDrawed b)
        {
            int result;

            if (a == null) 
            {
                if (b == null)
                {
                    result = 0;
                }
                else
                {
                    result = -1;
                }
            }
            else
            {
                result = a.CompareTo(b);
            }

            return result;
        }
    }
}