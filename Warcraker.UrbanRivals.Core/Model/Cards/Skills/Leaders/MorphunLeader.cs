using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Leaders
{
    public class MorphunLeader : Leader
    {
        public MorphunLeader(int pillzIncrease, int maximum)
            : base(pillzIncrease, maximum)
        {
            ;
        }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Leaders.MORPHUN;
            }
        }
    }
}
