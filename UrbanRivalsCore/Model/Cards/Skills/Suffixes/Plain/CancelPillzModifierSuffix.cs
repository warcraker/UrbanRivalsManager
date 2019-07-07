using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.Plain
{
    public class CancelPillzModifierSuffix : Suffix
    {
        private readonly static PlainSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static CancelPillzModifierSuffix()
        {
            Regex regex = new Regex(@"^CancelOppPillzModif$");

            PRV_PARSER = new PlainSuffixParser(regex, new CancelPillzModifierSuffix());
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_cancel_pillz_modifier;
        }

        public CancelPillzModifierSuffix() : base()
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
