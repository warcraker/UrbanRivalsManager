using System;
using UrbanRivalsUtils;

namespace UrbanRivalsCore.Model
{
    public class CardInstance
    {
        public readonly int cardInstanceId;
        public readonly int cardBaseId;
        public readonly string name;
        public readonly int level;
        public readonly int experience;
        public readonly int minLevel;
        public readonly int maxLevel;
        public readonly Clan clan;
        public readonly CardRarity rarity;
        public readonly Skill ability;
        public readonly int power;
        public readonly int damage;

        public static CardInstance createCardInstance(CardDefinition cardDefinition, int instanceId, int level, int experience)
        {
            CardInstance cardInstance;

            cardInstance = new CardInstance(cardDefinition, instanceId, level, experience);

            return cardInstance;
        }
        public static CardInstance createCardInstanceAtMaxLevel(CardDefinition cardDefinition, int instanceId)
        {
            CardInstance cardInstance;
            int level;
            int experience;

            level = cardDefinition.maxLevel;
            experience = 1;
            cardInstance = new CardInstance(cardDefinition, instanceId, level, experience);

            return cardInstance;
        }
        private CardInstance(CardDefinition cardDefinition, int instanceId, int level, int experience)
        {
            int minLevel;
            int maxLevel;
            Skill abilityFromBase;
            Skill ability;
            CardStats cardStats;

            AssertArgument.isNotNull(cardDefinition, nameof(cardDefinition));
            AssertArgument.checkIntegerRange(instanceId > 0, "Must be greater than 0", instanceId, nameof(instanceId));
            minLevel = cardDefinition.minLevel;
            maxLevel = cardDefinition.maxLevel;
            AssertArgument.checkIntegerRange(minLevel <= level && level <= maxLevel, $"Must be between {minLevel} and {maxLevel} inclusive", level, nameof(level));
            AssertArgument.checkIntegerRange(experience >= 0, "Must be greater or equal to 0", experience, nameof(experience));
            if (level == maxLevel)
            {
                AssertArgument.check(experience == 1, $"When {nameof(level)} is max, experience must be 1", nameof(experience));
            }

            abilityFromBase = cardDefinition.ability;
            if (abilityFromBase == Skill.NO_ABILITY)
            {
                ability = Skill.NO_ABILITY;
            }
            else
            {
                int abilityUnlockLevel;

                abilityUnlockLevel = cardDefinition.abilityUnlockLevel;
                if (level >= abilityUnlockLevel)
                {
                    ability = abilityFromBase;
                }
                else
                {
                    switch (abilityUnlockLevel)
                    {
                        case 2:
                            ability = Skill.UNLOCKED_AT_LEVEL_2;
                            break;
                        case 3:
                            ability = Skill.UNLOCKED_AT_LEVEL_3;
                            break;
                        case 4:
                            ability = Skill.UNLOCKED_AT_LEVEL_4;
                            break;
                        case 5:
                            ability = Skill.UNLOCKED_AT_LEVEL_5;
                            break;
                        default:
                            ability = null;
                            Asserts.fail("Invalid unlock level");
                            break;
                    }
                }
            }

            cardStats = cardDefinition.getCardStatsByLevel(level);

            this.cardInstanceId = instanceId;
            this.cardBaseId = cardDefinition.id;
            this.name = cardDefinition.name;
            this.level = level;
            this.experience = experience;
            this.minLevel = cardDefinition.minLevel;
            this.maxLevel = cardDefinition.maxLevel;
            this.rarity = cardDefinition.rarity;
            this.ability = ability;
            this.power = cardStats.power;
            this.damage = cardStats.damage;
        }

        public override string ToString()
        {
            return $"[{cardInstanceId}] {name}";
        }
    }
}