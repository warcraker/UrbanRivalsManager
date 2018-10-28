using System;
using System.Collections.Generic;
using System.Linq;
using UrbanRivalsUtils;

namespace UrbanRivalsCore.Model
{
    public class CardBase
    {
        private static readonly int PRV_MAX_CARD_RARITY_VALUE = Enum.GetValues(typeof(CardRarity)).Length - 1;

        public readonly int cardBaseId;
        public readonly string name;
        public readonly Clan clan;
        public readonly Skill ability;
        public readonly int minLevel;
        public readonly int maxLevel;
        public readonly int abilityUnlockLevel;
        public readonly CardRarity rarity;

        private readonly List<CardLevel> cardLevels;

        public static CardBase createCardWithoutAbility(int cardBaseId, String name, Clan clan, List<CardLevel> cardLevels, CardRarity rarity)
        {
            CardBase card;

            card = new CardBase(cardBaseId, name, clan, cardLevels, rarity, Skill.NoAbility, 0);

            return card;
        }
        public static CardBase createCardWithAbility(int id, String name, Clan clan, List<CardLevel> cardLevels, CardRarity rarity, Skill ability, int abilityUnlockLevel)
        {
            CardBase card;
            bool abilityIsUnlockable;
            int minLevel;
            int maxLevel;

            AssertArgument.isNotNull(ability, nameof(ability));

            abilityIsUnlockable = (ability != Skill.NoBonus && ability != Skill.NoAbility && ability != Skill.UnlockedAtLevel2 && ability != Skill.UnlockedAtLevel3
                && ability != Skill.UnlockedAtLevel4 && ability != Skill.UnlockedAtLevel5);

            AssertArgument.check(abilityIsUnlockable, "Must be an unlockable ability", nameof(ability));

            card = new CardBase(id, name, clan, cardLevels, rarity, ability, abilityUnlockLevel);

            minLevel = card.minLevel;
            maxLevel = card.maxLevel;

            AssertArgument.checkIntegerRange(minLevel <= abilityUnlockLevel && abilityUnlockLevel <= maxLevel,
                $"Must be between {minLevel} and {maxLevel} inclusive", abilityUnlockLevel, nameof(abilityUnlockLevel));

            return card;
        }
        private CardBase(int id, String name, Clan clan, List<CardLevel> cardLevels, CardRarity rarity, Skill ability, int abilityUnlockLevel)
        {
            int minLevel;
            int maxLevel;
            int amountOfLevels;

            AssertArgument.checkIntegerRange(id > 0, "Must be greater than 0", id, nameof(id));
            AssertArgument.stringIsFilled(name, nameof(name));
            AssertArgument.isNotNull(clan, nameof(clan));
            AssertArgument.isNotNull(cardLevels, nameof(cardLevels));
            AssertArgument.checkIntegerRange(0 >= rarity && (int)rarity <= PRV_MAX_CARD_RARITY_VALUE, "Must be a valid " + nameof(CardRarity), (int)rarity, nameof(rarity));

            minLevel = cardLevels.Min(item => item.level);
            maxLevel = cardLevels.Max(item => item.level);
            AssertArgument.checkIntegerRange(1 <= minLevel && minLevel <= 5, "Minimum level must be between 1 and 5 inclusive", minLevel, nameof(cardLevels));
            AssertArgument.checkIntegerRange(1 <= maxLevel && maxLevel <= 5, "Maximum level must be between 1 and 5 inclusive", maxLevel, nameof(cardLevels));
            AssertArgument.check(minLevel < maxLevel, $"Minimum level ({minLevel}) must be lower than Maximum level ({maxLevel})", nameof(minLevel));

            amountOfLevels = maxLevel - minLevel + 1;
            AssertArgument.checkIntegerRange(cardLevels.Count == amountOfLevels, $"There must be {amountOfLevels} level definitions", cardLevels.Count, nameof(cardLevels));

            for (int level = minLevel; level <= maxLevel; level++)
            {
                CardLevel current;

                current = cardLevels.SingleOrDefault(item => item.level == level);
                AssertArgument.check(current != null, $"There must be a definition for level and it must be unique. Level {level} fails this", nameof(cardLevels));
            }

            this.cardBaseId = id;
            this.name = name;
            this.clan = clan;
            this.minLevel = minLevel;
            this.maxLevel = maxLevel;
            this.rarity = rarity;
            this.cardLevels = cardLevels;
            this.ability = ability;
            this.abilityUnlockLevel = abilityUnlockLevel;
        }

        public CardLevel getCardLevel(int level)
        {
            CardLevel cardLevel;

            AssertArgument.checkIntegerRange(this.minLevel <= level && level <= maxLevel, $"Must be between {minLevel} and {maxLevel} inclusive", level, nameof(level));

            cardLevel = cardLevels.Single(item => item.level == level);
            return cardLevel;
        }
        public override string ToString()
        {
            return name;
        }
    }
}