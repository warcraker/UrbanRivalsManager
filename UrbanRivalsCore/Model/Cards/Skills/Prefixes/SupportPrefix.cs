using System.Text.RegularExpressions;

namespace UrbanRivalsCore.Model.Cards.Skills.Prefixes
{
    public class SupportPrefix : Prefix
    {
        private const string PRV_SUPPORT_PREFIX = "Support: ";
        private static readonly Regex PRV_SUPPORT_REGEX = new Regex("^Support: ");

        public override bool isMatch(string text)
        {
            return text.StartsWith(PRV_SUPPORT_PREFIX);
        }
        public override string removePrefixFromText(string text)
        {
            return PRV_SUPPORT_REGEX.Replace(text, "");
        }
        public override string ToString()
        {
            return PRV_SUPPORT_PREFIX;
        }
    }
}
