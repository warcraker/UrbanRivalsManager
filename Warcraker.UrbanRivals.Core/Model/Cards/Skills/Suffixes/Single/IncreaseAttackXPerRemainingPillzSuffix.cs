using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Single
{
    public class IncreaseAttackXPerRemainingPillzSuffix : Suffix
    {
        public IncreaseAttackXPerRemainingPillzSuffix(int x) : base(x, -1) { }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Suffixes.Single.INCREASE_ATTACK_X_PER_REMAINING_PILLZ;
            }
        }
    }
}
