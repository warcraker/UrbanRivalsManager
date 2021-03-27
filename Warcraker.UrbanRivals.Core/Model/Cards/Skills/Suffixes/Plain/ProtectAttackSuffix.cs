using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Plain
{
    public class ProtectAttackSuffix : Suffix
    {
        public ProtectAttackSuffix() : base(-1, -1) { }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Suffixes.Plain.PROTECT_ATTACK;
            }
        }
    }
}
