using System.Text.RegularExpressions;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills.Prefixes;

namespace Warcraker.UrbanRivals.ORM
{
    public class PrefixParser
    {
        private readonly Regex pattern;

        public Prefix Prefix { get; }

        public PrefixParser(Prefix prefix, Regex pattern)
        {
            this.pattern = pattern;
            this.Prefix = prefix;
        }

        public bool IsMatch(string text)
        {
            return this.pattern.IsMatch(text);
        }
        public string RemovePrefixFromText(string textWithPrefix)
        {
            return this.pattern.Replace(textWithPrefix, "");
        }
    }
}
