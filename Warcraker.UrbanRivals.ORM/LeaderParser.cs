using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills;

namespace Warcraker.UrbanRivals.ORM
{
    public class LeaderParser
    {
        private readonly Func<int, int, Leader> builder;
        private readonly Regex pattern;

        public LeaderParser(Func<int, int, Leader> builder, Regex pattern)
        {
            this.builder = builder;
            this.pattern = pattern;
        }

        public bool IsMatch(string text)
        {
            return this.pattern.IsMatch(text);
        }

        public Leader ParseLeader(string suffixAsText)
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
