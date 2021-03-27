using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Leaders
{
    public class VholtLeader : Leader
    {
        public VholtLeader(int damageDecrease, int minimum)
            : base(damageDecrease, minimum)
        {
            ;
        }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Leaders.VHOLT;
            }
        }
    }
}
