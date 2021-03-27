using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Plain
{
    public class CopyPowerSuffix : Suffix
    {
        public CopyPowerSuffix() : base(-1, -1) { }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Suffixes.Plain.COPY_POWER;
            }
        }
    }
}
