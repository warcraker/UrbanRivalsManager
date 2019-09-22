using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.SingleValue
{
    public class IncreaseLifeXPerDamage : SingleValueSuffix
    {
        private readonly static SingleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static IncreaseLifeXPerDamage()
        {
            Regex regex = new Regex(@"^\+(?<x>[1-9])LifePerD(?:amage|mg)$", RegexOptions.None); 

            PRV_PARSER = new SingleValueSuffixParser(regex, (x) => new IncreaseLifeXPerDamage(x));
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_increase_life_x_per_damage;
        }

        public IncreaseLifeXPerDamage(int x) : base(x)
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
