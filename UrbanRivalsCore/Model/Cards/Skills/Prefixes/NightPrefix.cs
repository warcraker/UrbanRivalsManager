using System.Text.RegularExpressions;

namespace UrbanRivalsCore.Model.Cards.Skills.Prefixes
{
    public class NightPrefix : Prefix
    {
        private const string PRV_NIGHT_PREFIX = "Night: ";
        private static readonly Regex PRV_NIGHT_REGEX = new Regex("^Night: ");

        public override bool isMatch(string text)
        {
            return text.StartsWith(PRV_NIGHT_PREFIX);
        }
        public override string removePrefixFromText(string text)
        {
            return PRV_NIGHT_REGEX.Replace(text, "");
        }
        public override string ToString()
        {
            return PRV_NIGHT_PREFIX;
        }
    }
}
