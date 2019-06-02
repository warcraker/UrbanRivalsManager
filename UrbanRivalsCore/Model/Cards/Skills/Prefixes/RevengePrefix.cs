using System.Text.RegularExpressions;

namespace UrbanRivalsCore.Model.Cards.Skills.Prefixes
{
    public class RevengePrefix : Prefix
    {
        private const string PRV_REVENGE_PREFIX = "Revenge: ";
        private static readonly Regex PRV_REVENGE_REGEX = new Regex("^Revenge ?: ");

        public override bool isMatch(string text)
        {
            const string PRV_REVENGE_PREFIX_WITH_SPACE = "Revenge :";

            return text.StartsWith(PRV_REVENGE_PREFIX)
                || text.StartsWith(PRV_REVENGE_PREFIX_WITH_SPACE)
                ;
        }
        public override string removePrefixFromText(string text)
        {
            return PRV_REVENGE_REGEX.Replace(text, "");
        }
        public override string ToString()
        {
            return PRV_REVENGE_PREFIX;
        }
    }
}
