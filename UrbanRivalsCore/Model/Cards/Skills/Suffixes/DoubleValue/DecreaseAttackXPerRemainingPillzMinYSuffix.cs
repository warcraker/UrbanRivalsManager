using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.DoubleValue
{
    public class DecreaseAttackXPerRemainingPillzMinYSuffix : DoubleValueSuffix
    {
        private readonly static DoubleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static DecreaseAttackXPerRemainingPillzMinYSuffix()
        {
            Regex regex = new Regex(@"^-(?<x>[1-9])OppAttPerPillzLeftMin(?<y>[1-9][0-9]?)$"); 

            PRV_PARSER = new DoubleValueSuffixParser(regex, (x, y) => new DecreaseAttackXPerRemainingPillzMinYSuffix(x, y));
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_decrease_attack_x_per_remaining_pillz_min_y;
        }

        public DecreaseAttackXPerRemainingPillzMinYSuffix(int x, int y) : base(x, y)
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
