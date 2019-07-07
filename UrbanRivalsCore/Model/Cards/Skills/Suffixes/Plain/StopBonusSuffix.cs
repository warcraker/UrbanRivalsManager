using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.Plain
{
    public class StopBonusSuffix : Suffix
    {
        private readonly static PlainSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static StopBonusSuffix()
        {
            Regex regex = new Regex(@"^Stop(?:Opp)?Bonus$", RegexOptions.Compiled); 

            PRV_PARSER = new PlainSuffixParser(regex, new StopBonusSuffix(), 70);
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_stop_bonus;
        }

        public StopBonusSuffix() : base()
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
