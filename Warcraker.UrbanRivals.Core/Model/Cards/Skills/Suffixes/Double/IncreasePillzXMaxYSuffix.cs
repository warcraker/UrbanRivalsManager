using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Double
{
    public class IncreasePillzXMaxYSuffix : Suffix
    {
        public IncreasePillzXMaxYSuffix(int x, int y) : base(x, y) { }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Suffixes.Double.INCREASE_PILLZ_X_MAX_Y;
            }
        }
    }
}