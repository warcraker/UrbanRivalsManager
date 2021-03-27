using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Single
{
    public class IncreaseAttackXSuffix : Suffix
    {
        public IncreaseAttackXSuffix(int x) : base(x, -1) { }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Suffixes.Single.INCREASE_ATTACK_X;
            }
        }
    }
}
