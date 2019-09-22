
using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.Plain
{
    public class CopyPowerAndDamageSuffix : Suffix
    {
        private readonly static PlainSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static CopyPowerAndDamageSuffix()
        {
            Regex regex = new Regex(@"^Copy:PowerAndDamageOpp$", RegexOptions.None); 

            PRV_PARSER = new PlainSuffixParser(regex, new CopyPowerAndDamageSuffix());
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_copy_power_and_damage;
        }

        public CopyPowerAndDamageSuffix() : base()
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
