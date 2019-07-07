
using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.Plain
{
    public class CopyPowerSuffix : Suffix
    {
        private readonly static PlainSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static CopyPowerSuffix()
        {
            Regex regex = new Regex(@"^Copy:OppPower|Power=PowerOpp$"); 

            PRV_PARSER = new PlainSuffixParser(regex, new CopyPowerSuffix());
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_copy_power;
        }

        public CopyPowerSuffix() : base()
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
