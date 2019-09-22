using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.Suffixes;

namespace UrbanRivalsCore.Model.Cards.Skills.SuffixParsers
{
    public class PlainSuffixParser : SuffixParser
    {
        private readonly Suffix suffixInstance;

        public PlainSuffixParser(Regex regex, Suffix suffixInstance) : base(regex)
        {
            this.suffixInstance = suffixInstance;
        }

        public override Suffix getSuffix(string suffixText)
        {
            return this.suffixInstance;
        }
    }
}
