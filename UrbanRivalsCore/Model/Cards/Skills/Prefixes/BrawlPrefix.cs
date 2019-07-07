using System.Text.RegularExpressions;

namespace UrbanRivalsCore.Model.Cards.Skills.Prefixes
{
    public class BrawlPrefix : Prefix
    {
        private static readonly string TEXT_REPRESENTATION = Properties.GameStrings.skill_prefix_brawl;
        private static readonly Regex REGEX = new Regex("^Brawl:", RegexOptions.Compiled);

        public override bool isMatch(string text)
        {
            return REGEX.IsMatch(text);
        }
        public override string removePrefixFromText(string text)
        {
            return REGEX.Replace(text, "");
        }
        public override string ToString()
        {
            return TEXT_REPRESENTATION;
        }
    }
}
