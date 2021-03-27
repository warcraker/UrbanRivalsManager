using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Leaders
{
    public class HugoLeader : Leader
    {
        public HugoLeader(int attackIncrease)
            : base(attackIncrease, -1)
        {
            ;
        }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Leaders.HUGO;
            }
        }
    }
}
