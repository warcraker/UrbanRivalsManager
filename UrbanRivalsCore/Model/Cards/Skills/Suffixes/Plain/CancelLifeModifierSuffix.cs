using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.Plain
{
    public class CancelLifeModifierSuffix : Suffix
    {
        private readonly static PlainSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static CancelLifeModifierSuffix()
        {
            Regex regex = new Regex(@"^Cancel Opp\. Life Modif\.$");

            PRV_PARSER = new PlainSuffixParser(regex, new CancelLifeModifierSuffix());
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_cancel_life_modifier;
        }

        public CancelLifeModifierSuffix() : base()
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
