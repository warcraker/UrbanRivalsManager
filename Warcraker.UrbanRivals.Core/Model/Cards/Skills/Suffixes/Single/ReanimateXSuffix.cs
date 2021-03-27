using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Single
{
    public class ReanimateXSuffix : Suffix
    {
        public ReanimateXSuffix(int x) : base(x, -1) { }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Suffixes.Single.REANIMATE_X;
            }
        }
    }
}
