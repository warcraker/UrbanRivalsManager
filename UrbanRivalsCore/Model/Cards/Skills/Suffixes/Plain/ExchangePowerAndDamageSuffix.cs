
using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.Plain
{
    public class ExchangePowerAndDamageSuffix : Suffix
    {
        private readonly static PlainSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static ExchangePowerAndDamageSuffix()
        {
            Regex regex = new Regex(@"^PowerAndDamageExchange$", RegexOptions.Compiled); 

            PRV_PARSER = new PlainSuffixParser(regex, new ExchangePowerAndDamageSuffix(), 1);
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_exchange_power_and_damage;
        }

        public ExchangePowerAndDamageSuffix() : base()
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
