using Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills
{
    public class NoAbility : Skill
    {
        public NoAbility() : base(new Prefix[] { }, new PlaceholderSuffix())
        {
        }
    }
}
