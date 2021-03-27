using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Leaders
{
    public class RobertCobbLeader : Leader
    {
        public RobertCobbLeader()
            : base(-1, -1)
        {
            ;
        }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Leaders.ROBERT_COBB;
            }
        }
    }
}
