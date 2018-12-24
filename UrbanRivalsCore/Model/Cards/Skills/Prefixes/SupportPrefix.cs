using System.Text.RegularExpressions;

namespace UrbanRivalsCore.Model.Cards.Skills.Prefixes
{
    public class SupportPrefix : Prefix
    {
        private static readonly Regex PRV_SUPPORT_REGEX = new Regex("^Support: ");

        public override bool isMatch(string text)
        {
            const string PRV_SUPPORT_PREFIX = "Support";

            return text.StartsWith(PRV_SUPPORT_PREFIX);
        }

        public override string removePrefixFromText(string text)
        {
            return PRV_SUPPORT_REGEX.Replace(text, "");
        }
    }
}
