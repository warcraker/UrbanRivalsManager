using System;
using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.Suffixes;
using UrbanRivalsCore.Model.Cards.Skills.Suffixes.DoubleValue;
using Warcraker.Utils;

namespace UrbanRivalsCore.Model.Cards.Skills.SuffixParsers
{
    public class DoubleValueSuffixParser : SuffixParser
    {
        private readonly Func<int, int, DoubleValueSuffix> funcSuffixFactory;

        public DoubleValueSuffixParser(Regex regex, Func<int, int, DoubleValueSuffix> funcSuffixFactory) : base(regex)
        {
            AssertArgument.CheckIsNotNull(funcSuffixFactory, nameof(funcSuffixFactory));

            this.funcSuffixFactory = funcSuffixFactory;
        }

        public override Suffix getSuffix(string suffixText)
        {
            Match match = this.regex.Match(suffixText);
            int x = prv_getCapturedGroupAsInteger(match, "x");
            int y = prv_getCapturedGroupAsInteger(match, "y");
            return this.funcSuffixFactory.Invoke(x, y);
        }
    }
}
