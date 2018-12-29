using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes
{
    public class CancelDamageModifierSuffix : Suffix
    {
        private readonly static PlainSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static CancelDamageModifierSuffix()
        {
            Regex regex = new Regex(@""); // ^Cancel Opp[.] Damage Modif[.]$

            PRV_PARSER = new PlainSuffixParser(regex, new CancelDamageModifierSuffix());
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_cancel_damage_modifier;
        }

        public CancelDamageModifierSuffix() : base()
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
