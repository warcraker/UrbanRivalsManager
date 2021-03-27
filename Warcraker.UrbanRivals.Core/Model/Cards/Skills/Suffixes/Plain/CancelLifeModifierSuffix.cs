using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Plain
{
    public class CancelLifeModifierSuffix : Suffix
    {
        public CancelLifeModifierSuffix() : base(-1, -1) { }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Suffixes.Plain.CANCEL_LIFE_MODIFIER;
            }
        }
    }
}
