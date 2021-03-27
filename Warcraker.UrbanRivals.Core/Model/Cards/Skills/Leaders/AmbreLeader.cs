using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Leaders
{
    public class AmbreLeader : Leader
    {
        public AmbreLeader(int powerIncrease)
            : base(powerIncrease, -1)
        {
            ;
        }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Leaders.AMBRE;
            }
        }
    }
}
