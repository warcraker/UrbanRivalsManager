using System;
using System.Text.RegularExpressions;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills;

namespace Warcraker.UrbanRivals.SkillParser
{
    public class SuffixParser
    {
        private readonly Func<int, int, Suffix> builder;
        private readonly Regex pattern;

        public SuffixParser(Func<int, int, Suffix> builder, Regex pattern)
        {
            this.builder = builder;
            this.pattern = pattern;
        }

        public bool IsMatch(string text)
        {
            return this.pattern.IsMatch(text);
        }

        public Suffix ParseSuffix(string suffixAsText)
        {
            Match match = this.pattern.Match(suffixAsText);
            int x = GetRegexValue(match, "x");
            int y = GetRegexValue(match, "y");

            return builder.Invoke(x, y);
        }

        private static int GetRegexValue(Match match, string groupName)
        {
            string valueAsText = match.Groups[groupName].Value;
            if (valueAsText == "")
                return -1;

            return int.Parse(valueAsText);
        }
    }
}
