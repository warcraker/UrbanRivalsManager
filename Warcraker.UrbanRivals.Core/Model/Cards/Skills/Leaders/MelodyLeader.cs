using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Leaders
{
    public class MelodyLeader : Leader
    {
        public MelodyLeader(int pillzRecovered, int outOf)
            : base(pillzRecovered,  outOf)
        {
            ;
        }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Leaders.MELODY;
            }
        }
    }
}
