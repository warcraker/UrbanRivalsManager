using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Leaders
{
    public class EyrikLeader : Leader
    {
        public EyrikLeader(int powerDecrease, int minimum)
            : base(powerDecrease, minimum)
        {
            ;
        }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Leaders.EYRIK;
            }
        }
    }
}
