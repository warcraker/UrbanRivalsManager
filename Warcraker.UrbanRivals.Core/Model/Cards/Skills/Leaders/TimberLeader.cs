using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Leaders
{
    public class TimberLeader : Leader
    {
        public TimberLeader(int damageIncrease)
            : base(damageIncrease, -1)
        {
            ;
        }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Leaders.TIMBER;
            }
        }
    }
}
