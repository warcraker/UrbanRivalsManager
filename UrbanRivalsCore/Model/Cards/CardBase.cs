using System;
using System.Collections.Generic;

namespace UrbanRivalsCore.Model
{
    /// <summary>
    /// Represents the data that share all the copies of the same card. 
    /// </summary>
    public class CardBase
    {
        private List<CardLevel> CardLevels;

        /// <summary>
        /// Gets the identifier that UR uses to identify the card.
        /// <example>
        /// Vansaar has 273 as its unique identifier. Every Vansaar copy has that same id.
        /// </example>
        /// </summary>
        public int CardBaseId { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the clan.
        /// </summary>
        public Clan Clan { get; private set; }

        /// <summary>
        /// Gets the ability. <remark>Keep in mind that the bonus is defined by the clan.</remark>
        /// </summary>
        public Skill Ability { get; private set; }

        /// <summary>
        /// Gets the level at which the card starts.
        /// </summary>
        public int MinLevel { get; private set; }

        /// <summary>
        /// Gets the maximum level the card can achieve.
        /// </summary>
        public int MaxLevel { get; private set; }

        /// <summary>
        /// Gets the level at which the card unlocks its ability. Zero if the ability is "No Ability".
        /// </summary>
        public int AbilityUnlockLevel { get; private set; }

        /// <summary>
        /// Gets the rarity.
        /// </summary>
        public CardRarity Rarity { get; private set; }

        /// <summary>
        /// Gets the publishing date.
        /// </summary>
        public DateTime PublishedDate { get; private set; }

        // Derived info

        /// <summary>
        /// Gets the power value of the card at its maximum level.
        /// </summary>
        public int MaxPower { get { return this[MaxLevel].Power; } }

        /// <summary>
        /// Gets the damage value of the card at its maximum level.
        /// </summary>
        public int MaxDamage { get { return this[MaxLevel].Damage; } }

        // Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CardBase"/> class. 
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <param name="name">Name.</param>
        /// <param name="clan">Clan.</param>
        /// <param name="ability">Ability.</param>
        /// <param name="minLevel">Minimum level.</param>
        /// <param name="maxLevel">Maximum level.</param>
        /// <param name="cardLevels"><see cref="CardLevel"/>'s of the card.</param>
        /// <param name="abilityUnlockLevel">Level at which the ability is unlocked.<param>
        /// <param name="rarity">Rarity.</param>
        /// <param name="publishedDate">Publishing date.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> can't be null, empty or whitespace</exception>
        /// <exception cref="ArgumentNullException"><paramref name="clan"/> can't be null</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="minLevel"/> must be between 1 and 5 inclusive</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxLevel"/> must be between <paramref name="minLevel"/> and 5 inclusive</exception>
        /// <exception cref="ArgumentNullException"><paramref name="cardLevels"/> can't be null</exception>
        /// <exception cref="ArgumentException"><paramref name="cardLevels"/> must contain a definition for all valid levels</exception>
        /// <exception cref="ArgumentException"><paramref name="ability"/> must be a valid <see cref="Skill"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="rarity"/> must be a valid <see cref="CardRarity"/></exception>
        public CardBase(int id, String name, Clan clan, int minLevel, int maxLevel, List<CardLevel> cardLevels, Skill ability, int abilityUnlockLevel, CardRarity rarity, DateTime? publishedDate = null)
        {
            if (id <= 0)
                throw new ArgumentException("Must be greater than zero", nameof(id));
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));
            if (clan == null)
                throw new ArgumentNullException(nameof(clan));
            if (minLevel < 0 || minLevel > 5)
                throw new ArgumentOutOfRangeException(nameof(minLevel), minLevel, "Must be between 1 and 5 inclusive");
            if (maxLevel < minLevel || maxLevel > 5)
                throw new ArgumentOutOfRangeException(nameof(maxLevel), maxLevel, "Must be between " + minLevel + " (minLevel) and 5 inclusive");
            if (cardLevels == null)
                throw new ArgumentNullException(nameof(cardLevels));
            for (int level = minLevel; level <= maxLevel; level++)
                if (cardLevels.Find(item => item.Level == level) == null)
                    throw new ArgumentException("Must contain a definition for all valid levels. Doesn't contain a definition for level " + level, nameof(cardLevels));
            if (ability == Skill.NoBonus ||
                ability == Skill.UnlockedAtLevel2 ||
                ability == Skill.UnlockedAtLevel3 ||
                ability == Skill.UnlockedAtLevel4 ||
                ability == Skill.UnlockedAtLevel5)
                throw new ArgumentException("Must be a valid skill", nameof(ability));
            if ((int)rarity < 0 || (int)rarity > Constants.EnumMaxAllowedValues.CardRarity)
                throw new ArgumentOutOfRangeException(nameof(rarity), rarity, "Must be a valid " + nameof(CardRarity));

            CardBaseId = id;
            Name = name;
            Clan = clan;
            MinLevel = minLevel;
            MaxLevel = maxLevel;
            CardLevels = new List<CardLevel>(cardLevels);
            Ability = ability;
            AbilityUnlockLevel = abilityUnlockLevel;
            Rarity = rarity;
            PublishedDate = (publishedDate == null || publishedDate < Constants.UrbanRivalsReleaseDate) 
                ? PublishedDate = Constants.UrbanRivalsReleaseDate : (DateTime)publishedDate;
        }

        // Functions

        /// <summary>
        /// Gets the <see cref="CardLevel"/> of the specified level.
        /// </summary>
        /// <param name="level">Level.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">Must be between <see cref="MinLevel"/> and <see cref="MaxLevel"/> inclusive.</exception>
        public CardLevel this[int level]
        {
            get
            {
                if (level < MinLevel || level > MaxLevel)
                    throw new ArgumentOutOfRangeException(nameof(level), level, $"Must be between {MinLevel} ({nameof(MinLevel)}) and {MaxLevel} ({nameof(MaxLevel)}) inclusive");

                return CardLevels.Find(item => item.Level == level);
            }
        }

        /// <summary>
        /// Returns the string representation of the card. 
        /// </summary>
        /// <returns>Name of the card</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}