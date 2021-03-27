using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Double
{
    public class RecoverXPillzOutOfYSuffix : Suffix
    {
        public RecoverXPillzOutOfYSuffix(int x, int y) : base(x, y) { }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Suffixes.Double.RECOVER_X_PILLZ_OUT_OF_Y;
            }
        }
    }
}