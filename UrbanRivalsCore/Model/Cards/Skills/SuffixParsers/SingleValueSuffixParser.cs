using System;
using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.Suffixes;
using UrbanRivalsCore.Model.Cards.Skills.Suffixes.SingleValue;
using Warcraker.Utils;

namespace UrbanRivalsCore.Model.Cards.Skills.SuffixParsers
{
    public class SingleValueSuffixParser : SuffixParser
    {
        private readonly Func<int, SingleValueSuffix> funcSuffixFactory;

        public SingleValueSuffixParser(Regex regex, Func<int, SingleValueSuffix> funcSuffixFactory) : base(regex)
        {
            AssertArgument.CheckIsNotNull(funcSuffixFactory, nameof(funcSuffixFactory));

            this.funcSuffixFactory = funcSuffixFactory;
        }

        public override Suffix getSuffix(string suffixText)
        {
            Match match = this.regex.Match(suffixText);
            int x = prv_getCapturedGroupAsInteger(match, "x");
            return this.funcSuffixFactory.Invoke(x);
        }
    }
}
