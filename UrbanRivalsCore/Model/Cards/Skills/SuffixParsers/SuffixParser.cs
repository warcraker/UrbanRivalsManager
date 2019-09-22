using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.Suffixes;
using UrbanRivalsUtils;

namespace UrbanRivalsCore.Model.Cards.Skills.SuffixParsers
{
    public abstract class SuffixParser
    {
        protected readonly Regex regex;

        public SuffixParser(Regex regex)
        {
            AssertArgument.isNotNull(regex, nameof(regex));

            this.regex = regex;
        }

        public abstract Suffix getSuffix(string suffixText);
        public bool isMatch(string suffixText)
        {
            return this.regex.IsMatch(suffixText);
        }

        public int Weight { get; }


        protected static int prv_getCapturedGroupAsInteger(Match match, string groupName)
        {
            Group group = match.Groups[groupName];
            Capture capture = group.Captures[0];
            string valueAsText = capture.Value;
            int value = int.Parse(valueAsText);
            return value;
        }
    }
}
