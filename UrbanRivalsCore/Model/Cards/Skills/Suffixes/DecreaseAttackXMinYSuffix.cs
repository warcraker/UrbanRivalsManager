using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes
{
    public class DecreaseAttackXMinYSuffix : DoubleValueSuffix
    {
        private readonly static DoubleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static DecreaseAttackXMinYSuffix()
        {
            Regex regex = new Regex(@"^-(?<x>[0-9]{1,2}) Opp\. Attack, Min (?<y>[0-9]{1,2})$"); //@"^- ?(?<x>[0-9]+) (Opp[.]? )?Attack,? Min (?<y>[0-9]+)$"

            PRV_PARSER = new DoubleValueSuffixParser(regex, (x, y) => new DecreaseAttackXMinYSuffix(x, y));
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
