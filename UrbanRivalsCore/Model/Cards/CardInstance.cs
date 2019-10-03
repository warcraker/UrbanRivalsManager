using System;
using Warcraker.Utils;

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
        public readonly OldSkill ability;
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
            OldSkill abilityFromBase;
            OldSkill ability;
            CardStats cardStats;

            AssertArgument.CheckIsNotNull(cardDefinition, nameof(cardDefinition));
            // TODO modify fake instances generator not to return negative IDs, and then reinstate assert "instanceId > 0"            
            minLevel = cardDefinition.minLevel;
            maxLevel = cardDefinition.maxLevel;
            AssertArgument.CheckIntegerRange(minLevel <= level && level <= maxLevel, $"Must be between {minLevel} and {maxLevel} inclusive", level, nameof(level));
            AssertArgument.CheckIntegerRange(experience >= 0, "Must be greater or equal to 0", experience, nameof(experience));
            if (level == maxLevel)
            {
                AssertArgument.Check(experience == 1, $"When {nameof(level)} is max, experience must be 1", nameof(experience));
            }

            abilityFromBase = cardDefinition.ability;
            if (abilityFromBase == OldSkill.NO_ABILITY)
            {
                ability = OldSkill.NO_ABILITY;
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
                            ability = OldSkill.UNLOCKED_AT_LEVEL_2;
                            break;
                        case 3:
                            ability = OldSkill.UNLOCKED_AT_LEVEL_3;
                            break;
                        case 4:
                            ability = OldSkill.UNLOCKED_AT_LEVEL_4;
                            break;
                        case 5:
                            ability = OldSkill.UNLOCKED_AT_LEVEL_5;
                            break;
                        default:
                            ability = null;
                            Asserts.Fail("Invalid unlock level");
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
            return $"[{this.cardInstanceId}] {this.name}";
        }
    }
}