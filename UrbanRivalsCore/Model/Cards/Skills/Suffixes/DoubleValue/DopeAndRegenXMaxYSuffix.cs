using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.DoubleValue
{
    public class DopeAndRegenXMaxYSuffix : DoubleValueSuffix
    {
        private readonly static DoubleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static DopeAndRegenXMaxYSuffix()
        {
            Regex regex = new Regex(@"^Dope\+Regen(?<x>[1-9])Max(?<y>[1-9][0-9]?)$");

            PRV_PARSER = new DoubleValueSuffixParser(regex, (x, y) => new DopeAndRegenXMaxYSuffix(x, y));
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_dope_and_regen_x_max_y;
        }

        public DopeAndRegenXMaxYSuffix(int x, int y) : base(x, y)
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
