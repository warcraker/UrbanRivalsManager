using Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Placeholders;
using Warcraker.Utils;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills
{
    public class LockedSkill : Skill
    {
        public static readonly Skill UNLOCKED_AT_LEVEL_2 = new LockedSkill(2);
        public static readonly Skill UNLOCKED_AT_LEVEL_3 = new LockedSkill(3);
        public static readonly Skill UNLOCKED_AT_LEVEL_4 = new LockedSkill(4);
        public static readonly Skill UNLOCKED_AT_LEVEL_5 = new LockedSkill(5);
        private LockedSkill(int unlockLevel) : base(new Prefix[] { }, NoAbilitySuffix.INSTANCE)
        {
            switch (unlockLevel)
            {
                case 2:
                    this.Suffix = UnlockedAtLevelXSuffix.UNLOCKED_AT_LEVEL_2;
                    break;
                case 3:
                    this.Suffix = UnlockedAtLevelXSuffix.UNLOCKED_AT_LEVEL_3;
                    break;
                case 4:
                    this.Suffix = UnlockedAtLevelXSuffix.UNLOCKED_AT_LEVEL_4;
                    break;
                case 5:
                    this.Suffix = UnlockedAtLevelXSuffix.UNLOCKED_AT_LEVEL_5;
                    break;
                default:
                    Asserts.Fail($"Invalid {nameof(unlockLevel)}: {unlockLevel}");
                    this.Suffix = UnknownSuffix.INSTANCE;
                    break;
            }
        }
    }
}
