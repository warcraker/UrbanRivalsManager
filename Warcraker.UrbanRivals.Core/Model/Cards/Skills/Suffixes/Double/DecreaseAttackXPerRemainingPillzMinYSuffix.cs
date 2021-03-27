using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Double
{
    public class DecreaseAttackXPerRemainingPillzMinYSuffix : Suffix
    {
        public DecreaseAttackXPerRemainingPillzMinYSuffix(int x, int y) : base(x, y) { }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Suffixes.Double.DECREASE_ATTACK_X_PER_REMAINING_PILLZ_MIN_Y;
            }
        }
    }
}