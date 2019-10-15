using System;

using UrbanRivalsCore.Model;

namespace UrbanRivalsCore
{
    public static class OldConstants
    {
        internal static class EnumMaxAllowedValues
        {
            // Public ones, if this class is ever set to public
            public static readonly int SkillLeader = GetMaxValue(typeof(SkillLeader));
            public static readonly int SkillPrefix = GetMaxValue(typeof(SkillPrefix));
            public static readonly int SkillSuffix = GetMaxValue(typeof(SkillSuffix));
            public static readonly int SurvivorStage = GetMaxValue(typeof(SurvivorStage));
            public static readonly int PlayerSide = GetMaxValue(typeof(PlayerSide));

            private static int GetMaxValue(Type type)
            {
                return Enum.GetValues(type).Length - 1;
            }
        }
    }
}
