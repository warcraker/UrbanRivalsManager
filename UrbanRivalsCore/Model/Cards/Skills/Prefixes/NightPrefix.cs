using System.Text.RegularExpressions;

namespace UrbanRivalsCore.Model.Cards.Skills.Prefixes
{
    public class NightPrefix : Prefix
    {
        private static readonly Regex PRV_NIGHT_REGEX = new Regex("^Night: ");

        public override bool isMatch(string text)
        {
            const string PRV_NIGHT_PREFIX = "Night: ";

            return text.StartsWith(PRV_NIGHT_PREFIX);
        }

        public override string removePrefixFromText(string text)
        {
            return PRV_NIGHT_REGEX.Replace(text, "");
        }
    }
}
