using Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Placeholders;
using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills
{
    public class NoAbility : Skill
    {
        public static readonly Skill INSTANCE = new NoAbility();
        private NoAbility() : base(new Prefix[] { }, NoAbilitySuffix.INSTANCE) { }
    }
}
