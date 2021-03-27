using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Placeholders
{
    public class InactiveBonusSuffix : PlaceholderSuffix
    {
        public static readonly Suffix INSTANCE = new InactiveBonusSuffix();
        private InactiveBonusSuffix() : base() { }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.INACTIVE_BONUS;
            }
        }
    }
}
