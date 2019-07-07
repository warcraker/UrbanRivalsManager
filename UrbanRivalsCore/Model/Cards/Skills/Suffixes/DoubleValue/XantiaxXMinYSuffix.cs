using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.DoubleValue
{
    public class XantiaxXMinYSuffix : DoubleValueSuffix
    {
        private readonly static DoubleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static XantiaxXMinYSuffix()
        {
            Regex regex = new Regex(@"^Xantiax:-(?<x>[1-9])LifeMin(?<y>[0-9])$"); 

            PRV_PARSER = new DoubleValueSuffixParser(regex, (x, y) => new XantiaxXMinYSuffix(x, y));
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_xantiax_x_min_y;
        }

        public XantiaxXMinYSuffix(int x, int y) : base(x, y)
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
