using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes
{
    public class DecreaseDamageXMinYSuffix : DoubleValueSuffix
    {
        private readonly static DoubleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static DecreaseDamageXMinYSuffix()
        {
            Regex regex = new Regex(@""); // ^-(?<x>[1-9]) Opp Damage, Min (?<y>[1-9])$     @"^- ?(?<x>[0-9]+) (Opp[.]? )?D(?:amage|mg),? Min (?<y>[0-9]+)$"
            PRV_PARSER = new DoubleValueSuffixParser(regex, (x, y) => new DecreaseDamageXMinYSuffix(x, y));
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_decrease_damage_x_min_y;
        }

        public DecreaseDamageXMinYSuffix(int x, int y) : base(x, y)
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