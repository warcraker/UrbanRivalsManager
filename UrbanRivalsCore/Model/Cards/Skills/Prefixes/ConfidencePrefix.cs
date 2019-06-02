using System.Text.RegularExpressions;

namespace UrbanRivalsCore.Model.Cards.Skills.Prefixes
{
    public class ConfidencePrefix : Prefix
    {
        private const string PRV_CONFIDENCE_STANDARD_PREFIX = "Confidence: ";
        private static readonly Regex PRV_CONFIDENCE_REGEX = new Regex("^Confidence(?: :|:|;) ");

        public override bool isMatch(string text)
        {
            const string PRV_CONFIDENCE_WITH_SEMICOLON_PREFIX = "Confidence; ";
            const string PRV_CONFIDENCE_WITH_EXTRA_SPACE_PREFIX = "Confidence : ";

            return text.StartsWith(PRV_CONFIDENCE_STANDARD_PREFIX) 
                || text.StartsWith(PRV_CONFIDENCE_WITH_SEMICOLON_PREFIX)
                || text.StartsWith(PRV_CONFIDENCE_WITH_EXTRA_SPACE_PREFIX)
                ;
        }
        public override string removePrefixFromText(string text)
        {
            return PRV_CONFIDENCE_REGEX.Replace(text, "");
        }
        public override string ToString()
        {
            return PRV_CONFIDENCE_STANDARD_PREFIX;
        }
    }
}
