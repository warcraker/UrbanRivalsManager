using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.Plain
{
    public class ProtectAbilitySuffix : Suffix
    {
        private readonly static PlainSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static ProtectAbilitySuffix()
        {
            Regex regex = new Regex(@"^Protection:Ability$", RegexOptions.None); 

            PRV_PARSER = new PlainSuffixParser(regex, new ProtectAbilitySuffix());
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_protect_ability;
        }

        public ProtectAbilitySuffix() : base()
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
