using System;

namespace UrbanRivalsCore.Model
{
    /// <summary>
    /// Represents one specific copy of a card. This copy (instance) can be owned by a single user, or none. 
    /// </summary>
    public class CardInstance
    {
        private CardBase CardBase;

        /// <summary>
        /// Gets the identifier that UR uses to identify this instance of the card.
        /// </summary>
        public int CardInstanceId { get; private set; }

        /// <summary>
        /// Gets the current level of the card.
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// Gets the current experience of the card.
        /// </summary>
        public int Experience { get; private set; }

        // From CardBase

        /// <summary>
        /// Gets the unique identifier of the card.
        /// </summary>
        public int CardBaseId { get { return CardBase.CardBaseId; } }

        /// <summary>
        /// Gets the name of the card.
        /// </summary>
        public string Name { get { return CardBase.Name; } }

        /// <summary>
        /// Gets the clan of the card.
        /// </summary>
        public Clan Clan { get { return CardBase.Clan; } }

        /// <summary>
        /// Gets the rarity of the card.
        /// </summary>
        public CardRarity Rarity { get { return CardBase.Rarity; } }

        /// <summary>
        /// Gets the publishing date of the card.
        /// </summary>
        public DateTime PublishedDate { get { return CardBase.PublishedDate; } }

        /// <summary>
        /// Gets the level at which the card starts.
        /// </summary>
        public int MinLevel { get { return CardBase.MinLevel; } }

        /// <summary>
        /// Gets the maximum level that the card can achieve.
        /// </summary>
        public int MaxLevel { get { return CardBase.MaxLevel; } }

        /// <summary>
        /// Gets the level at which the card unlocks its ability. Zero if the ability is "No Ability".
        /// </summary>
        public int AbilityUnlockLevel { get { return CardBase.AbilityUnlockLevel; } }

        /// <summary>
        /// Gets the bonus of the card.
        /// </summary>
        public Skill Bonus { get { return CardBase.Clan.Bonus; } }

        // From level

        /// <summary>
        /// Gets the power of the card at the current level.
        /// </summary>
        public int Power { get { return CardBase[Level].Power; } }

        /// <summary>
        /// Gets the damage of the card at the current level.
        /// </summary>
        public int Damage { get { return CardBase[Level].Damage; } }

        /// <summary>
        /// Gets the ability (if unlocked) at current level. Gets its "Unlocked at level X" value otherwise.
        /// </summary>
        public Skill Ability
        {
            get
            {
                if (Level >= AbilityUnlockLevel)
                    return CardBase.Ability;
                else
                {
                    switch (AbilityUnlockLevel)
                    {
                        case 2:
                            return Skill.UnlockedAtLevel2;
                        case 3:
                            return Skill.UnlockedAtLevel3;
                        case 4:
                            return Skill.UnlockedAtLevel4;
                        case 5:
                            return Skill.UnlockedAtLevel5;
                    }
                }
                throw new Exception("Ability get failed"); // Sanity check
            }
        }

        // Constructors

        private CardInstance() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CardInstance"/> class. 
        /// </summary>
        /// <param name="cardBase">Base card from which it is instantiated.</param>
        /// <param name="instanceId">Identifier of the instance.</param>
        /// <param name="level">Level of the instance.</param>
        /// <param name="experience">Experience of the instance. Overridden to 1 if <paramref name="level"/> is maxed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="cardBase"/> can't be null</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="level"/> must be a valid level</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="experience"/> must be greater or equal to 0</exception>
        public CardInstance(CardBase cardBase, int instanceId, int level = 0, int experience = 0)
        {
            if (cardBase == null)
                throw new ArgumentNullException(nameof(cardBase));

            CardBase = cardBase;

            if (level != 0 && (level < MinLevel || level > MaxLevel))
                throw new ArgumentOutOfRangeException(nameof(level), level, $"Must be between {MinLevel} ({nameof(MinLevel)}) and {MaxLevel} ({nameof(MaxLevel)}) inclusive, or be 0");
            if (experience < 0)
                throw new ArgumentOutOfRangeException(nameof(experience), experience, "Must be greater or equal to 0");

            CardInstanceId = instanceId;

            if (level == 0)
                Level = CardBase.MaxLevel;
            else
                Level = level;

            if (level == MaxLevel)
                Experience = 1; // In-game, when level is maxed, experience = 1.
            else
                Experience = experience;
        }

        // Functions

        /// <summary>
        /// Returns the string representation of the card.
        /// </summary>
        /// <returns>[CardInstance Id] Name of the card</returns>
        public override string ToString()
        {
            return $"[{CardInstanceId}] {Name}";
        }
    }
}