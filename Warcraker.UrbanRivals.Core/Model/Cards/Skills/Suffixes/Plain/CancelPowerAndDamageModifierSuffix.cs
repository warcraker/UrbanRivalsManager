using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Plain
{
    public class CancelPowerAndDamageModifierSuffix : Suffix
    {
        public CancelPowerAndDamageModifierSuffix() : base(-1, -1) { }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Suffixes.Plain.CANCEL_POWER_AND_DAMAGE_MODIFIER;
            }
        }
    }
}
