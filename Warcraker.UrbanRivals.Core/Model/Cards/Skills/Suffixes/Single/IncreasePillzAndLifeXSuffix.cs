using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Single
{
    public class IncreasePillzAndLifeXSuffix : Suffix
    {
        public IncreasePillzAndLifeXSuffix(int x) : base(x, -1) { }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Suffixes.Single.INCREASE_PILLZ_AND_LIFE_X;
            }
        }
    }
}
