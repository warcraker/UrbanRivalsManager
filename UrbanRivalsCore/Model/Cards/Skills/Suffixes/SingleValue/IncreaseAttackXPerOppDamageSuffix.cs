using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.SingleValue
{
    public class IncreaseAttackXPerOppDamageSuffix : SingleValueSuffix
    {
        private readonly static SingleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static IncreaseAttackXPerOppDamageSuffix()
        {
            Regex regex = new Regex(@"^\+(?<x>[1-9])AttackPerOppDamage$"); 

            PRV_PARSER = new SingleValueSuffixParser(regex, (x) => new IncreaseAttackXPerOppDamageSuffix(x));
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_increase_attack_x_per_remaining_life;
        }

        public IncreaseAttackXPerOppDamageSuffix(int x) : base(x)
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
