using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Leaders
{
    public class VansarLeader : Leader
    {
        public VansarLeader(int experienceBonus)
            : base(experienceBonus, -1)
        {
            ;
        }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Leaders.VANSAR;
            }
        }
    }
}
