
using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.Plain
{
    public class ExchangeDamageSuffix : Suffix
    {
        private readonly static PlainSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static ExchangeDamageSuffix()
        {
            Regex regex = new Regex(@"^Damage Exchange$"); 

            PRV_PARSER = new PlainSuffixParser(regex, new ExchangeDamageSuffix());
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_exchange_damage;
        }

        public ExchangeDamageSuffix() : base()
        {
            ;
        }

        public static PlainSuffixParser getParser()
        {
            return PRV_PARSER;
        }

        public override string ToString()
        {
            return PRV_TEXT_REPRESENTATION;
        }
    }
}
