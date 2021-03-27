using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Double
{
    public class DopeAndRegenXMaxYSuffix : Suffix
    {
        public DopeAndRegenXMaxYSuffix(int x, int y) : base(x, y) { }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Suffixes.Double.DOPE_AND_REGEN_X_MAX_Y;
            }
        }
    }
}