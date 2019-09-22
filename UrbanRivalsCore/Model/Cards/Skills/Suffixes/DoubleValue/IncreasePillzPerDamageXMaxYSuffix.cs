using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.DoubleValue
{
    public class IncreasePillzPerDamageXMaxYSuffix : DoubleValueSuffix
    {
        private readonly static DoubleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static IncreasePillzPerDamageXMaxYSuffix()
        {
            Regex regex = new Regex(@"^\+(?<x>[1-9])PillzPerDamageMax(?<y>[1-9]?[0-9])$", RegexOptions.None);

            PRV_PARSER = new DoubleValueSuffixParser(regex, (x, y) => new IncreasePillzPerDamageXMaxYSuffix(x, y));
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_increase_pillz_x_per_damage_max_y;
        }

        public IncreasePillzPerDamageXMaxYSuffix(int x, int y) : base(x, y)
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
