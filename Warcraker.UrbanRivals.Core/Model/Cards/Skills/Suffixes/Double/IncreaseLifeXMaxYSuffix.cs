using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Double
{
    public class IncreaseLifeXMaxYSuffix : Suffix
    {
        public IncreaseLifeXMaxYSuffix(int x, int y) : base(x, y) { }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Suffixes.Double.INCREASE_LIFE_X_MAX_Y;
            }
        }
    }
}