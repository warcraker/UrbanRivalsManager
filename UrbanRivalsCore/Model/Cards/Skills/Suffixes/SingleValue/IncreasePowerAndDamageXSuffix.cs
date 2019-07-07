using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.SingleValue
{
    public class IncreasePowerAndDamageXSuffix : SingleValueSuffix
    {
        private readonly static SingleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static IncreasePowerAndDamageXSuffix()
        {
            Regex regex = new Regex(@"^Power(?:And|&)Damage\+(?<x>[1-9])$", RegexOptions.Compiled); 

            PRV_PARSER = new SingleValueSuffixParser(regex, (x) => new IncreasePowerAndDamageXSuffix(x), 39);
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_increase_power_and_damage_x;
        }

        public IncreasePowerAndDamageXSuffix(int x) : base(x)
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
