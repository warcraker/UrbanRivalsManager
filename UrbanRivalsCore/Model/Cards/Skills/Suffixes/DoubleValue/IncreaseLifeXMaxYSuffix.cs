using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.DoubleValue
{
    public class IncreaseLifeXMaxYSuffix : DoubleValueSuffix
    {
        private readonly static DoubleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static IncreaseLifeXMaxYSuffix()
        {
            Regex regex = new Regex(@"^\+(?<x>[1-9])LifeMax(?<y>[1-9][0-9]?)$");

            PRV_PARSER = new DoubleValueSuffixParser(regex, (x, y) => new IncreaseLifeXMaxYSuffix(x, y));
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_increase_life_x_max_y;
        }

        public IncreaseLifeXMaxYSuffix(int x, int y) : base(x, y)
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
