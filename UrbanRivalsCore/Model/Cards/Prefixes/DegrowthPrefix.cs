using System.Text.RegularExpressions;

namespace UrbanRivalsCore.Model.Cards.Prefixes
{
    public class DegrowthPrefix : Prefix
    {
        private static readonly Regex PRV_DEGROWTH_REGEX = new Regex("^Degrowth: ");

        public override bool isMatch(string text)
        {
            const string PRV_DEGROWTH_PREFIX = "Degrowth";

            return text.StartsWith(PRV_DEGROWTH_PREFIX);
        }

        public override string removePrefixFromText(string text)
        {
            return PRV_DEGROWTH_REGEX.Replace(text, "");
        }
    }
}
