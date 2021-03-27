using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Plain
{
    public class CancelAttackModifierSuffix : Suffix
    {
        public CancelAttackModifierSuffix() : base(-1, -1) { }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Suffixes.Plain.CANCEL_ATTACK_MODIFIER;
            }
        }
    }
}
