using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.SingleValue
{
    public class IncreaseAttackXSuffix : SingleValueSuffix
    {
        private readonly static SingleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static IncreaseAttackXSuffix()
        {
            Regex regex = new Regex(@"^At(?:tac)?k\+(?<x>[1-9][0-9]?)$", RegexOptions.Compiled); 

            PRV_PARSER = new SingleValueSuffixParser(regex, (x) => new IncreaseAttackXSuffix(x), 70);
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_increase_attack_x;
        }

        public IncreaseAttackXSuffix(int x) : base(x)
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
