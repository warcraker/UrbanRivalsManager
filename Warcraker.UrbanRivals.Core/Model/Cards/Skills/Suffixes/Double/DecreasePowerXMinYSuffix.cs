using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Double
{
    public class DecreasePowerXMinYSuffix : Suffix
    {
        public DecreasePowerXMinYSuffix(int x, int y) : base(x, y) { }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Suffixes.Double.DECREASE_POWER_X_MIN_Y;
            }
        }
    }
}