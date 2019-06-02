using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.SingleValue
{
    public class IncreasePillzXPerDamageSuffix : SingleValueSuffix
    {
        private readonly static SingleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static IncreasePillzXPerDamageSuffix()
        {
            Regex regex = new Regex(@"^\+(?<x>[1-9]) Pillz Per Damage$"); 

            PRV_PARSER = new SingleValueSuffixParser(regex, (x) => new IncreasePillzXPerDamageSuffix(x));
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_increase_pillz_x_per_damage;
        }

        public IncreasePillzXPerDamageSuffix(int x) : base(x)
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
