using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Double
{
    public class DecreaseDamageXMinYSuffix : Suffix
    {
        public DecreaseDamageXMinYSuffix(int x, int y) : base(x, y) { }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Suffixes.Double.DECREASE_DAMAGE_X_MIN_Y;
            }
        }
    }
}