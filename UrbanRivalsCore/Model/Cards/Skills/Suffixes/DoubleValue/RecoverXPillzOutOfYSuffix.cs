using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.DoubleValue
{
    public class RecoverXPillzOutOfYSuffix : DoubleValueSuffix
    {
        private readonly static DoubleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static RecoverXPillzOutOfYSuffix()
        {
            Regex regex = new Regex(@"^Recover(?<x>[1-9])PillzOutOf(?<y>[1-9])$", RegexOptions.Compiled);

            PRV_PARSER = new DoubleValueSuffixParser(regex, (x, y) => new RecoverXPillzOutOfYSuffix(x, y), 27);
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_recover_x_pillz_out_of_y;
        }

        public RecoverXPillzOutOfYSuffix(int x, int y) : base(x, y)
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
