using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Leaders
{
    public class EkloreLeader : Leader
    {
        public EkloreLeader(int pillzDecrease, int minimum)
            : base(pillzDecrease, minimum)
        {
            ;
        }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Leaders.EKLORE;
            }
        }
    }
}
