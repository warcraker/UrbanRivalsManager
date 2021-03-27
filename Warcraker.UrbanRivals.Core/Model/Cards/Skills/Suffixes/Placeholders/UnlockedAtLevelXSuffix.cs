using System;
using System.Collections.Generic;
using System.Text;
using Warcraker.UrbanRivals.Localization;
using Warcraker.Utils;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Placeholders
{
    public class UnlockedAtLevelXSuffix : PlaceholderSuffix
    {
        private readonly int unlockLevel;

        public static readonly Suffix UNLOCKED_AT_LEVEL_2 = new UnlockedAtLevelXSuffix(2);
        public static readonly Suffix UNLOCKED_AT_LEVEL_3 = new UnlockedAtLevelXSuffix(3);
        public static readonly Suffix UNLOCKED_AT_LEVEL_4 = new UnlockedAtLevelXSuffix(4);
        public static readonly Suffix UNLOCKED_AT_LEVEL_5 = new UnlockedAtLevelXSuffix(5);
        private UnlockedAtLevelXSuffix(int unlockLevel) : base()
        {
            switch (unlockLevel)
            {
                case 2:
                case 3:
                case 4:
                case 5:
                    this.unlockLevel = unlockLevel;
                    break;
                default:
                    this.unlockLevel = -1;
                    Asserts.Fail($"Invalid unlock level: {unlockLevel}");
                    break;
            }
        }

        public override string LocalizationKey
        {
            get
            {
                switch (unlockLevel)
                {
                    case 2:
                        return LocalizationKeys.Skills.ABILITY_UNLOCKED_AT_LEVEL_2;
                    case 3:
                        return LocalizationKeys.Skills.ABILITY_UNLOCKED_AT_LEVEL_3;
                    case 4:
                        return LocalizationKeys.Skills.ABILITY_UNLOCKED_AT_LEVEL_4;
                    case 5:
                        return LocalizationKeys.Skills.ABILITY_UNLOCKED_AT_LEVEL_5;
                    default:
                        return LocalizationKeys.NON_LOCALIZABLE;
                }
            }
        }
    }
}
