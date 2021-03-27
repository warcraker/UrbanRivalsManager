using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Leaders
{
    public class JonhDoomLeader : Leader
    {
        public JonhDoomLeader(int statsDecrease, int minimum)
            : base(statsDecrease,  minimum)
        {
            ;
        }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Leaders.JONH_DOOM;
            }
        }
    }
}
