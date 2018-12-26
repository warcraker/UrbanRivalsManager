using System.Text.RegularExpressions;

namespace UrbanRivalsCore.Model.Cards.Skills.Prefixes
{
    public class ReprisalPrefix : Prefix
    {
        private const string PRV_REPRISAL_PREFIX = "Reprisal: ";
        private static readonly Regex PRV_REPRISAL_REGEX = new Regex("^Reprisal: ");

        public override bool isMatch(string text)
        {
            return text.StartsWith(PRV_REPRISAL_PREFIX);
        }
        public override string removePrefixFromText(string text)
        {
            return PRV_REPRISAL_REGEX.Replace(text, "");
        }
        public override string ToString()
        {
            return PRV_REPRISAL_PREFIX;
        }
    }
}
