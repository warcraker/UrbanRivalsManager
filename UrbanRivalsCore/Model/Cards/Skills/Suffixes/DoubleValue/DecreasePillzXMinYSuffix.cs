using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.DoubleValue
{
    public class DecreasePillzXMinYSuffix : DoubleValueSuffix
    {
        private readonly static DoubleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static DecreasePillzXMinYSuffix()
        {
            Regex regex = new Regex(@"^-(?<x>[1-9])(?:OppPillz|Pillz(?:Opp)?)Min(?<y>[0-9])$", RegexOptions.None);

            PRV_PARSER = new DoubleValueSuffixParser(regex, (x, y) => new DecreasePillzXMinYSuffix(x, y));
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_decrease_pillz_x_min_y;
        }

        public DecreasePillzXMinYSuffix(int x, int y) : base(x, y)
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
