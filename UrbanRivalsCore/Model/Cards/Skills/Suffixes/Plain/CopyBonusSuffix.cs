using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.Plain
{
    public class CopyBonusSuffix : Suffix
    {
        private readonly static PlainSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static CopyBonusSuffix()
        {
            Regex regex = new Regex(@"^Copy:OppBonus$", RegexOptions.Compiled); 

            PRV_PARSER = new PlainSuffixParser(regex, new CopyBonusSuffix(), 27);
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_copy_bonus;
        }

        public CopyBonusSuffix() : base()
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
