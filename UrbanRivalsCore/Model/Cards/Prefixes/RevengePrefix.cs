using System.Text.RegularExpressions;

namespace UrbanRivalsCore.Model.Cards.Prefixes
{
    public class RevengePrefix : Prefix
    {
        private static readonly Regex PRV_REVENGE_REGEX = new Regex("^Revenge: ");

        public override bool isMatch(string text)
        {
            const string PRV_REVENGE_PREFIX = "Revenge";

            return text.StartsWith(PRV_REVENGE_PREFIX);
        }

        public override string removePrefixFromText(string text)
        {
            return PRV_REVENGE_REGEX.Replace(text, "");
        }
    }
}
