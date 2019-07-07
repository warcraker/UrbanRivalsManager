using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.SingleValue
{
    public class ReanimateXSuffix : SingleValueSuffix
    {
        private readonly static SingleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static ReanimateXSuffix()
        {
            Regex regex = new Regex(@"^Reanimate:\+(?<x>[1-9])Life$", RegexOptions.Compiled); 

            PRV_PARSER = new SingleValueSuffixParser(regex, (x) => new ReanimateXSuffix(x), 1);
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_reanimate_x;
        }

        public ReanimateXSuffix(int x) : base(x)
        {
            ;
        }

        public static SingleValueSuffixParser getParser()
        {
            return PRV_PARSER;
        }

        public override string ToString()
        {
            return getTextRepresentation(PRV_TEXT_REPRESENTATION);
        }
    }
}
