using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes
{
    public class IncreaseAttackXSuffix : SingleValueSuffix
    {
        private readonly static SingleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static IncreaseAttackXSuffix()
        {
            Regex regex = new Regex(@""); // ^Attack \+(?<x>[0-9]{1,2})$ @"^At(tac)?k[.]? [+](?<x>[0-9]+)$"

            PRV_PARSER = new SingleValueSuffixParser(regex, (x) => new IncreaseAttackXSuffix(x));
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
