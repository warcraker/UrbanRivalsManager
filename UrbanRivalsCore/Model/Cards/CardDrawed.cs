using System;
using System.ComponentModel;

namespace UrbanRivalsCore.Model
{
    /// <summary>
    /// Represents a copy of a <see cref="CardInstance"/> that is ready to be used in <see cref="UrbanRivalsCore.ViewModel.Combat"/>.
    /// </summary>
    public class CardDrawed : IComparable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private CardInstance CardInstance;

        private Skill m_Ability;
        /// <summary>
        /// Gets or sets the ability.
        /// </summary>
        public Skill Ability
        {
            get { return m_Ability; }
            set
            {
                if (value == null)
                    m_Ability = Skill.NoAbility;
                else
                    m_Ability = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Ability)));
            }
        }

        private Skill m_Bonus;
        /// <summary>
        /// Gets or sets the bonus.
        /// </summary>
        public Skill Bonus
        {
            get { return m_Bonus; }
            set
            {
                if (value == null)
                    m_Bonus = Skill.NoBonus;
                else
                    m_Bonus = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Bonus)));
            }
        }

        // Derived info

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get { return CardInstance.Name; } }

        /// <summary>
        /// Get the CardBase identifier.
        /// </summary>
        public int CardBaseId { get { return CardInstance.CardBaseId; } }

        /// <summary>
        /// Gets the clan.
        /// </summary>
        public Clan Clan { get { return CardInstance.Clan; } }

        /// <summary>
        /// Gets the level.
        /// </summary>
        public int Level { get { return CardInstance.Level; } }

        /// <summary>
        /// Gets the experience.
        /// </summary>
        public int Experience { get { return CardInstance.Experience; } }

        /// <summary>
        /// Gets the maximum level the card can achieve.
        /// </summary>
        public int MaxLevel { get { return CardInstance.MaxLevel; } }

        /// <summary>
        /// Gets the power.
        /// </summary>
        public int Power { get { return CardInstance.Power; } }

        /// <summary>
        /// Gets the damage.
        /// </summary>
        public int Damage { get { return CardInstance.Damage; } }

        /// <summary>
        /// Gets the rarity.
        /// </summary>
        public CardRarity Rarity { get { return CardInstance.Rarity; } }

        // Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CardDrawed"/> class.
        /// </summary>
        /// <param name="cardInstance">Instance from which it will take the card values.</param>
        /// <exception cref="ArgumentNullException"><paramref name="cardInstance"/> can't be null</exception>
        public CardDrawed(CardInstance cardInstance)
        {
            if (cardInstance == null)
                throw new ArgumentNullException(nameof(cardInstance));

            CardInstance = cardInstance;
            Ability = CardInstance.Ability.Copy();
            Bonus = CardInstance.Bonus.Copy();
        }

        // Functions

        /// <summary>
        /// Returns the string representation of the card.
        /// </summary>
        /// <returns>[@CardInstance Id] Name of the card</returns>
        public override string ToString()
        {
            return $"[@{CardInstance.CardInstanceId}] {Name}";
        }

        /// <summary>
        /// Compares this instance with another instance of <see cref="CardDrawed"/> and returns an integer that represents 
        /// which one goes to the left, to the right or if they have equivalent position on the game interface.
        /// </summary>
        /// <param name="obj"><see cref="CardDrawed"/> instance to compare with this one.</param>
        /// <returns>1 if this instance goes to the left. 0 if their positions are equivalent. -1 if this instance goes to the right.
        /// <remark>Null instances go to the right.</remark>
        /// </returns>
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            CardDrawed a, b;
            a = this;
            try
            {
                b = (CardDrawed)obj;
            }
            catch
            {
                throw new ArgumentException("Is not a CardDrawed instance", nameof(obj));
            }

            int comparation = a.Clan.Name.CompareTo(b.Clan.Name);
            if (comparation != 0)
                return comparation;

            comparation = a.Name.CompareTo(b.Name);
            if (comparation != 0)
                return comparation;

            comparation = a.Level.CompareTo(b.Level);
            if (comparation != 0)
                return comparation;

            return a.Experience.CompareTo(b.Experience);
        }

        /// <summary>
        /// Compares two instances of <see cref="CardDrawed"/> and returns an integer that represents 
        /// which one goes to the left, to the right or if they have equivalent position on the game interface.
        /// </summary>
        /// <param name="obj"><see cref="CardDrawed"/> instance to compare with this one.</param>
        /// <returns>1 if <paramref name="a"/> goes to the left. 0 if both <paramref name="a"/> and <paramref name="b"/> positions are equivalent. -1 if <paramref name="a"/> goes to the right.
        /// <remark>Null instances go to the right. If both are null then return 0</remark>
        /// </returns>
        public static int CompareTo(CardDrawed a, CardDrawed b)
        {
            if (a == null && b == null)
                return 0;
            if (a == null)
                return -1;
            if (b == null)
                return 1;
            return a.CompareTo(b);
        }
    }
}