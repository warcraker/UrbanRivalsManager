using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Leaders
{
    public class AshigaruLeader : Leader
    {
        public AshigaruLeader()
            : base(-1, -1)
        {
            ;
        }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Leaders.ASHIGARU;
            }
        }
    }
}
