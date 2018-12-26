using System.Text.RegularExpressions;

namespace UrbanRivalsCore.Model.Cards.Skills.Prefixes
{
    public class DegrowthPrefix : Prefix
    {
        private const string PRV_DEGROWTH_PREFIX = "Degrowth: ";
        private static readonly Regex PRV_DEGROWTH_REGEX = new Regex("^Degrowth: ");

        public override bool isMatch(string text)
        {
            return text.StartsWith(PRV_DEGROWTH_PREFIX);
        }
        public override string removePrefixFromText(string text)
        {
            return PRV_DEGROWTH_REGEX.Replace(text, "");
        }
        public override string ToString()
        {
            return PRV_DEGROWTH_PREFIX;
        }
    }
}
