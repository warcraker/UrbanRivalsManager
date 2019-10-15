using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills.Leaders;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills.Prefixes;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes;
using Warcraker.Utils;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills
{
    public class Skill
    {
        public IReadOnlyCollection<Prefix> Prefixes { get; protected set; }
        public Suffix Suffix { get; protected set; }

        public Skill(IEnumerable<Prefix> prefixes, Suffix suffix)
        {
            AssertArgument.CheckIsNotNull(prefixes, nameof(prefixes));
            AssertArgument.CheckIsNotNull(suffix, nameof(suffix));

            this.Prefixes = prefixes.ToArray();
            this.Suffix = suffix;
        }
    }
}
