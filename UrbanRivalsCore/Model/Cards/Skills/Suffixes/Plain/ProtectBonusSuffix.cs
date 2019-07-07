using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.Plain
{
    public class ProtectBonusSuffix : Suffix
    {
        private readonly static PlainSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static ProtectBonusSuffix()
        {
            Regex regex = new Regex(@"^BonusProtection|Protection:Bonus$", RegexOptions.Compiled); 

            PRV_PARSER = new PlainSuffixParser(regex, new ProtectBonusSuffix(), 19);
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_protect_bonus;
        }

        public ProtectBonusSuffix() : base()
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
