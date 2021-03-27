using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Double
{
    public class DecreaseLifeAndPillzXMinYSuffix : Suffix
    {
        public DecreaseLifeAndPillzXMinYSuffix(int x, int y) : base(x, y) { }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Suffixes.Double.DECREASE_LIFE_AND_PILLZ_X_MIN_Y;
            }
        }
    }
}