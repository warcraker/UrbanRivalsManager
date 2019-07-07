using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.Plain
{
    public class CancelAttackModifierSuffix : Suffix
    {
        private readonly static PlainSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static CancelAttackModifierSuffix()
        {
            Regex regex = new Regex(@"^CancelOppAttackModif$");

            PRV_PARSER = new PlainSuffixParser(regex, new CancelAttackModifierSuffix());
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_cancel_attack_modifier;
        }

        public CancelAttackModifierSuffix() : base()
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
