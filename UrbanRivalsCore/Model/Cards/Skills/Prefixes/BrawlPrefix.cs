using System.Text.RegularExpressions;

namespace UrbanRivalsCore.Model.Cards.Skills.Prefixes
{
    public class BrawlPrefix : Prefix
    {
        private const string PRV_BRAWL_PREFIX = "Brawl: ";
        private static readonly Regex PRV_BRAWL_REGEX = new Regex("^Brawl: ");

        public override bool isMatch(string text)
        {
            return text.StartsWith(PRV_BRAWL_PREFIX);
        }
        public override string removePrefixFromText(string text)
        {
            return PRV_BRAWL_REGEX.Replace(text, "");
        }
        public override string ToString()
        {
            return PRV_BRAWL_PREFIX;
        }
    }
}
