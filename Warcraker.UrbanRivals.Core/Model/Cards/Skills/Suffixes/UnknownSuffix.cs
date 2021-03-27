using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes
{
    public sealed class UnknownSuffix : Suffix
    {
        public static readonly Suffix INSTANCE = new UnknownSuffix();
        private UnknownSuffix() : base(-1, -1)
        {
        }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.NON_LOCALIZABLE;
            }
        }
    }
}
