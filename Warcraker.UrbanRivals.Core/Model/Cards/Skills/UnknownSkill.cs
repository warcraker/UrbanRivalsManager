using System;
using System.Collections.Generic;
using System.Text;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills
{
    public sealed class UnknownSkill : Skill
    {
        public static readonly Skill INSTANCE = new UnknownSkill();
        private UnknownSkill() : base(new Prefix[] { }, new UnknownSuffix())
        {
        }
    }
}
