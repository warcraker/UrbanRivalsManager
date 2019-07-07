using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.Plain
{
    public class ProtectAttackSuffix : Suffix
    {
        private readonly static PlainSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static ProtectAttackSuffix()
        {
            Regex regex = new Regex(@"^Protection:Attack$", RegexOptions.Compiled); 

            PRV_PARSER = new PlainSuffixParser(regex, new ProtectAttackSuffix(), 8);
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_protect_attack;
        }

        public ProtectAttackSuffix() : base()
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
