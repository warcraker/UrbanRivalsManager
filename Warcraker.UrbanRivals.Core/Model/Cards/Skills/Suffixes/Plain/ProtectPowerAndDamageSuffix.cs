using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Plain
{
    public class ProtectPowerAndDamageSuffix : Suffix
    {
        public ProtectPowerAndDamageSuffix() : base(-1, -1) { }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Suffixes.Plain.PROTECT_POWER_AND_DAMAGE;
            }
        }
    }
}
