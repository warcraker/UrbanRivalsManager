using System;
using System.Collections.Generic;
using System.Linq;
using UrbanRivalsUtils;

namespace UrbanRivalsCore.Model
{
    public class CardDefinition
    {
        private static readonly int PRV_MAX_CARD_RARITY_VALUE = Enum.GetValues(typeof(CardRarity)).Length - 1;

        public readonly int id;
        public readonly string name;
        public readonly Clan clan;
        public readonly OldSkill ability;
        public readonly int minLevel;
        public readonly int maxLevel;
        public readonly int abilityUnlockLevel;
        public readonly CardRarity rarity;

        private readonly List<CardStats> cardStatsPerLevel;

        public static CardDefinition createCardWithoutAbility(int id, string name, Clan clan, List<CardStats> cardStatsPerLevel, CardRarity rarity)
        {
            CardDefinition cardDefinition;

            cardDefinition = new CardDefinition(id, name, clan, cardStatsPerLevel, rarity, OldSkill.NO_ABILITY, 0);

            return cardDefinition;
        }
        public static CardDefinition createCardWithAbility(int id, string name, Clan clan, List<CardStats> cardStatsPerLevel, CardRarity rarity, OldSkill ability, int abilityUnlockLevel)
        {
            CardDefinition cardDefinition;
            bool abilityIsUnlockable;
            int minLevel;
            int maxLevel;

            AssertArgument.isNotNull(ability, nameof(ability));

            abilityIsUnlockable = (ability != OldSkill.NO_BONUS && ability != OldSkill.NO_ABILITY && ability != OldSkill.UNLOCKED_AT_LEVEL_2 && ability != OldSkill.UNLOCKED_AT_LEVEL_3
                && ability != OldSkill.UNLOCKED_AT_LEVEL_4 && ability != OldSkill.UNLOCKED_AT_LEVEL_5);

            AssertArgument.check(abilityIsUnlockable, "Must be an unlockable ability", nameof(ability));

            cardDefinition = new CardDefinition(id, name, clan, cardStatsPerLevel, rarity, ability, abilityUnlockLevel);

            minLevel = cardDefinition.minLevel;
            maxLevel = cardDefinition.maxLevel;

            AssertArgument.checkIntegerRange(minLevel <= abilityUnlockLevel && abilityUnlockLevel <= maxLevel,
                $"Must be between {minLevel} and {maxLevel} inclusive", abilityUnlockLevel, nameof(abilityUnlockLevel));

            return cardDefinition;
        }
        private CardDefinition(int id, String name, Clan clan, List<CardStats> cardStatsPerLevel, CardRarity rarity, OldSkill ability, int abilityUnlockLevel)
        {
            int minLevel;
            int maxLevel;
            int amountOfLevels;

            AssertArgument.checkIntegerRange(id > 0, "Must be greater than 0", id, nameof(id));
            AssertArgument.stringIsFilled(name, nameof(name));
            AssertArgument.isNotNull(clan, nameof(clan));
            AssertArgument.isNotNull(cardStatsPerLevel, nameof(cardStatsPerLevel));
            AssertArgument.checkIntegerRange(0 <= rarity && (int)rarity <= PRV_MAX_CARD_RARITY_VALUE, "Must be a valid " + nameof(CardRarity), (int)rarity, nameof(rarity));

            minLevel = cardStatsPerLevel.Min(item => item.level);
            maxLevel = cardStatsPerLevel.Max(item => item.level);
            AssertArgument.checkIntegerRange(1 <= minLevel && minLevel <= 5, "Minimum level must be between 1 and 5 inclusive", minLevel, nameof(cardStatsPerLevel));
            AssertArgument.checkIntegerRange(1 <= maxLevel && maxLevel <= 5, "Maximum level must be between 1 and 5 inclusive", maxLevel, nameof(cardStatsPerLevel));
            AssertArgument.check(minLevel < maxLevel, $"Minimum level ({minLevel}) must be lower than Maximum level ({maxLevel})", nameof(minLevel));

            amountOfLevels = maxLevel - minLevel + 1;
            AssertArgument.checkIntegerRange(cardStatsPerLevel.Count == amountOfLevels, $"There must be {amountOfLevels} level definitions", cardStatsPerLevel.Count, nameof(cardStatsPerLevel));

            for (int level = minLevel; level <= maxLevel; level++)
            {
                CardStats cardStats;

                try
                {
                    cardStats = cardStatsPerLevel.Single(item => item.level == level);
                }
                catch (InvalidOperationException)
                {
                    Asserts.fail($"There must be a definition for level and it must be unique. Level {level} fails this");
                }
            }

            this.id = id;
            this.name = name;
            this.clan = clan;
            this.minLevel = minLevel;
            this.maxLevel = maxLevel;
            this.rarity = rarity;
            this.cardStatsPerLevel = cardStatsPerLevel;
            this.ability = ability;
            this.abilityUnlockLevel = abilityUnlockLevel;
        }

        public CardStats getCardStatsByLevel(int level)
        {
            CardStats cardStats;

            AssertArgument.checkIntegerRange(this.minLevel <= level && level <= this.maxLevel, $"Must be between {this.minLevel} and {this.maxLevel} inclusive", level, nameof(level));

            cardStats = this.cardStatsPerLevel.Single(item => item.level == level);
            return cardStats;
        }
        public override string ToString()
        {
            return this.name;
        }
    }
}