using Warcraker.Utils;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills
{
    public sealed class PlaceholderSkill : Skill
    {
        private class PlaceholderSuffix : Suffix
        {
            public PlaceholderSuffix() : base(0, 0) { }
        }

        private enum EPlaceholder
        {
            NoAbility,
            NotActiveBonus,
            AbilityUnlockedAtLevel2,
            AbilityUnlockedAtLevel3,
            AbilityUnlockedAtLevel4,
            AbilityUnlockedAtLevel5,
        }

        public static readonly Skill NO_ABILITY = new PlaceholderSkill(EPlaceholder.NoAbility);
        public static readonly Skill NO_BONUS = new PlaceholderSkill(EPlaceholder.NotActiveBonus);
        public static readonly Skill ABILITY_UNLOCKED_AT_LEVEL_2 = new PlaceholderSkill(EPlaceholder.AbilityUnlockedAtLevel2);
        public static readonly Skill ABILITY_UNLOCKED_AT_LEVEL_3 = new PlaceholderSkill(EPlaceholder.AbilityUnlockedAtLevel3);
        public static readonly Skill ABILITY_UNLOCKED_AT_LEVEL_4 = new PlaceholderSkill(EPlaceholder.AbilityUnlockedAtLevel4);
        public static readonly Skill ABILITY_UNLOCKED_AT_LEVEL_5 = new PlaceholderSkill(EPlaceholder.AbilityUnlockedAtLevel5);

        private readonly EPlaceholder placeholder;

        private PlaceholderSkill(EPlaceholder placeholder)
            : base(new Prefix[0], new PlaceholderSuffix())
        {
            this.placeholder = placeholder;
        }

        public override string ToString()
        {
            string text;

            switch (this.placeholder)
            {
                case EPlaceholder.NotActiveBonus:
                    text = ""; // TODO // Properties.GameStrings.skill_no_bonus;
                    break;
                case EPlaceholder.NoAbility:
                    text = ""; // TODO // Properties.GameStrings.skill_no_ability;
                    break;
                case EPlaceholder.AbilityUnlockedAtLevel2:
                    text = GetUnlockedAtLevelStringRepresentation(2);
                    break;
                case EPlaceholder.AbilityUnlockedAtLevel3:
                    text = GetUnlockedAtLevelStringRepresentation(3);
                    break;
                case EPlaceholder.AbilityUnlockedAtLevel4:
                    text = GetUnlockedAtLevelStringRepresentation(4);
                    break;
                case EPlaceholder.AbilityUnlockedAtLevel5:
                    text = GetUnlockedAtLevelStringRepresentation(5);
                    break;
                default:
                    text = "Invalid";
                    Asserts.Fail("Invalid placeholder value state");
                    break;
            }

            return text;
        }

        private static string GetUnlockedAtLevelStringRepresentation(int level)
        {
            return "" + level; // TODO // String.Format(Properties.GameStrings.skill_not_unlocked, level);
        }
    }
}
