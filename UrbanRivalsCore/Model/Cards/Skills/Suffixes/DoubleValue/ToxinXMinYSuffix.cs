using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.DoubleValue
{
    public class ToxinXMinYSuffix : DoubleValueSuffix
    {
        private readonly static DoubleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static ToxinXMinYSuffix()
        {
            Regex regex = new Regex(@"^Toxin(?<x>[1-9])Min(?<y>[0-9])$", RegexOptions.Compiled); 

            PRV_PARSER = new DoubleValueSuffixParser(regex, (x, y) => new ToxinXMinYSuffix(x, y), 9);
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_toxin_x_min_y;
        }

        public ToxinXMinYSuffix(int x, int y) : base(x, y)
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
