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

        public static CardInstance createCardInstance(CardBase cardBase, int instanceId, int level, int experience)
        {
            CardInstance cardInstance;

            cardInstance = new CardInstance(cardBase, instanceId, level, experience);

            return cardInstance;
        }
        public static CardInstance createCardInstanceAtMaxLevel(CardBase cardBase, int instanceId)
        {
            CardInstance cardInstance;
            int level;
            int experience;

            level = cardBase.maxLevel;
            experience = 1;
            cardInstance = new CardInstance(cardBase, instanceId, level, experience);

            return cardInstance;
        }
        private CardInstance(CardBase cardBase, int instanceId, int level, int experience)
        {
            int minLevel;
            int maxLevel;
            Skill abilityFromBase;
            Skill ability;
            CardLevel cardStats;

            AssertArgument.isNotNull(cardBase, nameof(cardBase));
            AssertArgument.checkIntegerRange(instanceId > 0, "Must be greater than 0", instanceId, nameof(instanceId));
            minLevel = cardBase.minLevel;
            maxLevel = cardBase.maxLevel;
            AssertArgument.checkIntegerRange(minLevel <= level && level <= maxLevel, $"Must be between {minLevel} and {maxLevel} inclusive", level, nameof(level));
            AssertArgument.checkIntegerRange(experience >= 0, "Must be greater or equal to 0", experience, nameof(experience));
            if (level == maxLevel)
            {
                AssertArgument.check(experience == 1, $"When {nameof(level)} is max, experience must be 1", nameof(experience));
            }

            abilityFromBase = cardBase.ability;
            if (abilityFromBase == Skill.NoAbility)
            {
                ability = Skill.NoAbility;
            }
            else
            {
                int abilityUnlockLevel;

                abilityUnlockLevel = cardBase.abilityUnlockLevel;
                if (level >= abilityUnlockLevel)
                {
                    ability = abilityFromBase;
                }
                else
                {
                    switch (abilityUnlockLevel)
                    {
                        case 2:
                            ability = Skill.UnlockedAtLevel2;
                            break;
                        case 3:
                            ability = Skill.UnlockedAtLevel3;
                            break;
                        case 4:
                            ability = Skill.UnlockedAtLevel4;
                            break;
                        case 5:
                            ability = Skill.UnlockedAtLevel5;
                            break;
                        default:
                            ability = null;
                            Asserts.fail("Invalid unlock level");
                            break;
                    }
                }
            }

            cardStats = cardBase.getCardLevel(level);

            this.cardInstanceId = instanceId;
            this.cardBaseId = cardBase.cardBaseId;
            this.name = cardBase.name;
            this.level = level;
            this.experience = experience;
            this.minLevel = cardBase.minLevel;
            this.maxLevel = cardBase.maxLevel;
            this.rarity = cardBase.rarity;
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