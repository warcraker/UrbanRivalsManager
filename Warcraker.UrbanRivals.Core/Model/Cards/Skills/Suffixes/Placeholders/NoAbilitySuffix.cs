using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Placeholders
{
    public class NoAbilitySuffix : PlaceholderSuffix
    {
        public static readonly Suffix INSTANCE = new NoAbilitySuffix();
        private NoAbilitySuffix() : base() { }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.NO_ABILITY;
            }
        }
    }
}
