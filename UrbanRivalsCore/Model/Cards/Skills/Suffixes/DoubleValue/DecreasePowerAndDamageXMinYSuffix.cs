using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.DoubleValue
{
    public class DecreasePowerAndDamageXMinYSuffix : DoubleValueSuffix
    {
        private readonly static DoubleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static DecreasePowerAndDamageXMinYSuffix()
        {
            Regex regex = new Regex(@"^-(?<x>[1-9])OppPow(?:er)?(?:&|And)D(?:amageM|amM|mgm)in(?<y>[0-9])$", RegexOptions.Compiled); 

            PRV_PARSER = new DoubleValueSuffixParser(regex, (x, y) => new DecreasePowerAndDamageXMinYSuffix(x, y), 42);
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_decrease_power_and_damage_x_min_y;
        }

        public DecreasePowerAndDamageXMinYSuffix(int x, int y) : base(x, y)
        {
            ;
        }

        public static DoubleValueSuffixParser getParser()
        {
            return PRV_PARSER;
        }

        public override string ToString()
        {
            return getTextRepresentation(PRV_TEXT_REPRESENTATION);
        }
    }
}