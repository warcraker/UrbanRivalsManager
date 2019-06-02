using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.SingleValue
{
    public class IncreaseDamageXSuffix : SingleValueSuffix
    {
        private readonly static SingleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static IncreaseDamageXSuffix()
        {
            Regex regex = new Regex(@"^Damage \+ ?(?<x>[1-9])$"); 

            PRV_PARSER = new SingleValueSuffixParser(regex, (x) => new IncreaseDamageXSuffix(x));
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_increase_damage_x;
        }

        public IncreaseDamageXSuffix(int x) : base(x)
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
