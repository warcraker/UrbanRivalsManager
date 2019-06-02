using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.DoubleValue
{
    public class DecreaseLifeAndPillzXMinYSuffix : DoubleValueSuffix
    {
        private readonly static DoubleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static DecreaseLifeAndPillzXMinYSuffix()
        {
            Regex regex = new Regex(@"^-(?<x>[1-9]) (?:Life & Pillz,|Opp\.? (?:Life & Pillz|Pillz (?:And|&) Life,)) Min (?<y>[0-9])$"); 

            PRV_PARSER = new DoubleValueSuffixParser(regex, (x, y) => new DecreaseLifeAndPillzXMinYSuffix(x, y));
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_decrease_life_and_pillz_x_min_y;
        }

        public DecreaseLifeAndPillzXMinYSuffix(int x, int y) : base(x, y)
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
