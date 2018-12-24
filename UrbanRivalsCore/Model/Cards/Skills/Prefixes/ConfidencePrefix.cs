using System.Text.RegularExpressions;

namespace UrbanRivalsCore.Model.Cards.Skills.Prefixes
{
    public class ConfidencePrefix : Prefix
    {
        private static readonly Regex PRV_CONFIDENCE_REGEX = new Regex("^Confidence: ");

        public override bool isMatch(string text)
        {
            const string PRV_CONFIDENCE_WITH_COLON_PREFIX = "Confidence:";
            const string PRV_CONFIDENCE_WITH_SEMICOLON_PREFIX = "Confidence;";

            return text.StartsWith(PRV_CONFIDENCE_WITH_COLON_PREFIX) || text.StartsWith(PRV_CONFIDENCE_WITH_SEMICOLON_PREFIX);
        }

        public override string removePrefixFromText(string text)
        {
            return PRV_CONFIDENCE_REGEX.Replace(text, "");
        }
    }
}
