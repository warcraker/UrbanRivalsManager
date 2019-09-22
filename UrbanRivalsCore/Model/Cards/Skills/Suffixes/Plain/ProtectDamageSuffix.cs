using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.Plain
{
    public class ProtectDamageSuffix : Suffix
    {
        private readonly static PlainSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static ProtectDamageSuffix()
        {
            Regex regex = new Regex(@"^Protection:Damage$", RegexOptions.None); 

            PRV_PARSER = new PlainSuffixParser(regex, new ProtectDamageSuffix());
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_protect_damage;
        }

        public ProtectDamageSuffix() : base()
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
