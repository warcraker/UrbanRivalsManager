using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.Plain
{
    public class ProtectPowerSuffix : Suffix
    {
        private readonly static PlainSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static ProtectPowerSuffix()
        {
            Regex regex = new Regex(@"^Protection:Power$", RegexOptions.None); 

            PRV_PARSER = new PlainSuffixParser(regex, new ProtectPowerSuffix());
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_protect_power;
        }

        public ProtectPowerSuffix() : base()
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
