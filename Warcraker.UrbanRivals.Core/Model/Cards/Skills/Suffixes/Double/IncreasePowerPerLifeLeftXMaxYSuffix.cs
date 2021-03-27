using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Double
{
    public class IncreasePowerPerLifeLeftXMaxYSuffix : Suffix
    {
        public IncreasePowerPerLifeLeftXMaxYSuffix(int x, int y) : base(x, y) { }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Suffixes.Double.INCREASE_POWER_PER_LIFE_LEFT_X_MAX_Y;
            }
        }
    }
}