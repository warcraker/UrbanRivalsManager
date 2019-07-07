using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.Plain
{
    public class CancelPowerAndDamageModifierSuffix : Suffix
    {
        private readonly static PlainSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static CancelPowerAndDamageModifierSuffix()
        {
            Regex regex = new Regex(@"^Canc(?:PowerDamMod|elOppPower(?:&DamageMod|AndDamageModif))$", RegexOptions.Compiled);

            PRV_PARSER = new PlainSuffixParser(regex, new CancelPowerAndDamageModifierSuffix(), 8);
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_cancel_power_and_damage_modifier;
        }

        public CancelPowerAndDamageModifierSuffix() : base()
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
