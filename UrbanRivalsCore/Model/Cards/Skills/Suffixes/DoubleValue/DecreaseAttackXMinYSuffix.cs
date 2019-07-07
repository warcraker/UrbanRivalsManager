using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.DoubleValue
{
    public class DecreaseAttackXMinYSuffix : DoubleValueSuffix
    {
        private readonly static DoubleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static DecreaseAttackXMinYSuffix()
        {
            Regex regex = new Regex(@"^-(?<x>[1-9][0-9]?)OppAttackMin(?<y>[1-9][0-9]?)$", RegexOptions.Compiled); 

            PRV_PARSER = new DoubleValueSuffixParser(regex, (x, y) => new DecreaseAttackXMinYSuffix(x, y), 75);
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_decrease_attack_x_min_y;
        }

        public DecreaseAttackXMinYSuffix(int x, int y) : base(x, y)
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
