using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.SingleValue
{
    public class IncreasePowerXSuffix : SingleValueSuffix
    {
        private readonly static SingleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static IncreasePowerXSuffix()
        {
            Regex regex = new Regex(@"^Power\+(?<x>[1-9])$"); 

            PRV_PARSER = new SingleValueSuffixParser(regex, (x) => new IncreasePowerXSuffix(x));
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_increase_power_x;
        }

        public IncreasePowerXSuffix(int x) : base(x)
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
