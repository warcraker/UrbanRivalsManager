using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes
{
    public class CopyBonusSuffix : Suffix
    {
        private readonly static PlainSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static CopyBonusSuffix()
        {
            Regex regex = new Regex(@"^Copy Opp Bonus$"); // @"^Copy:? Opp[.] Bonus$"

            PRV_PARSER = new PlainSuffixParser(regex, new CopyBonusSuffix());
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
