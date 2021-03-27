using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Plain
{
    public class CancelPillzAndLifeModifierSuffix : Suffix
    {
        public CancelPillzAndLifeModifierSuffix() : base(-1, -1) { }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Suffixes.Plain.CANCEL_PILLZ_AND_LIFE_MODIFIER;
            }
        }
    }
}
