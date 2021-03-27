using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Double
{
    public class IncreasePillzPerDamageXMaxYSuffix : Suffix
    {
        public IncreasePillzPerDamageXMaxYSuffix(int x, int y) : base(x, y) { }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Suffixes.Double.INCREASE_PILLZ_X_PER_DAMAGE_MAX_Y;
            }
        }
    }
}