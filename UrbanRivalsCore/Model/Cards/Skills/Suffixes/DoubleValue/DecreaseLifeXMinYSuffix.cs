using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.DoubleValue
{
    public class DecreaseLifeXMinYSuffix : DoubleValueSuffix
    {
        private readonly static DoubleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static DecreaseLifeXMinYSuffix()
        {
            Regex regex = new Regex(@"^-(?<x>[1-9])OppLifeMin(?<y>[0-9])$", RegexOptions.Compiled); 

            PRV_PARSER = new DoubleValueSuffixParser(regex, (x, y) => new DecreaseLifeXMinYSuffix(x, y), 88);
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_decrease_life_x_min_y;
        }

        public DecreaseLifeXMinYSuffix(int x, int y) : base(x, y)
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
