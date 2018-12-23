using System.Text.RegularExpressions;

namespace UrbanRivalsCore.Model.Cards.Prefixes
{
    public class ReprisalPrefix : Prefix
    {
        private static readonly Regex PRV_REPRISAL_REGEX = new Regex("^Reprisal: ");

        public override bool isMatch(string text)
        {
            const string PRV_REPRISAL_PREFIX = "Reprisal";

            return text.StartsWith(PRV_REPRISAL_PREFIX);
        }

        public override string removePrefixFromText(string text)
        {
            return PRV_REPRISAL_REGEX.Replace(text, "");
        }
    }
}
