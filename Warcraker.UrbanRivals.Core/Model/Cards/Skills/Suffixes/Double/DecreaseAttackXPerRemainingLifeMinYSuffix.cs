using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Double
{
    public class DecreaseAttackXPerRemainingLifeMinYSuffix : Suffix
    {
        public DecreaseAttackXPerRemainingLifeMinYSuffix(int x, int y) : base(x, y) { }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Suffixes.Double.DECREASE_ATTACK_X_PER_REMAINING_LIFE_MIN_Y;
            }
        }
    }
}