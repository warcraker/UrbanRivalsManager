using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Leaders
{
    public class BridgetLeader : Leader
    {
        public BridgetLeader(int lifeIncrease)
            : base(lifeIncrease, -1)
        {
            ;
        }
        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Leaders.BRIDGET;
            }
        }
    }
}
