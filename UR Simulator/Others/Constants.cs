using System;

namespace UrbanRivalsManager
{
    public static class Constants
    {
        public static readonly int SleepWhenIdleInMiliseconds = 1500;
        public static readonly int TimeBetweenApiRequestsInMiliseconds = 310;

        internal static class EnumMaxAllowedValues
        {
            internal static readonly int CharacterImageFormat = GetMaxValue(typeof(ViewModel.DataManagement.CharacterImageFormat));

            private static int GetMaxValue(Type type)
            {
                return Enum.GetValues(type).Length - 1;
            }
        }
    }
}
