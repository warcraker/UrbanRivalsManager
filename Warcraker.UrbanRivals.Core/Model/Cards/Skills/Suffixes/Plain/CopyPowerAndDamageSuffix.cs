using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Plain
{
    public class CopyPowerAndDamageSuffix : Suffix
    {
        public CopyPowerAndDamageSuffix() : base(-1, -1) { }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Suffixes.Plain.COPY_POWER_AND_DAMAGE;
            }
        }
    }
}
