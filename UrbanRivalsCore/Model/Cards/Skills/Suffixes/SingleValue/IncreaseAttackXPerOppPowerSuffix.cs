using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.SingleValue
{
    public class IncreaseAttackXPerOppPowerSuffix : SingleValueSuffix
    {
        private readonly static SingleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static IncreaseAttackXPerOppPowerSuffix()
        {
            Regex regex = new Regex(@"^\+(?<x>[1-9])AttackPerOppPower$", RegexOptions.None); 

            PRV_PARSER = new SingleValueSuffixParser(regex, (x) => new IncreaseAttackXPerOppPowerSuffix(x));
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_increase_attack_x_per_opp_power;
        }

        public IncreaseAttackXPerOppPowerSuffix(int x) : base(x)
        {
            ;
        }

        public static SingleValueSuffixParser getParser()
        {
            return PRV_PARSER;
        }

        public override string ToString()
        {
            return getTextRepresentation(PRV_TEXT_REPRESENTATION);
        }
    }
}
