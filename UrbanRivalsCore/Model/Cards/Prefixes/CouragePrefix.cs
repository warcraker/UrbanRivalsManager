using System.Text.RegularExpressions;

namespace UrbanRivalsCore.Model.Cards.Prefixes
{
    public class CouragePrefix : Prefix
    {
        private static readonly Regex PRV_COURAGE_REGEX = new Regex("^Courage[:;] ");

        public override bool isMatch(string text)
        {
            const string PRV_COURAGE_PREFIX = "Courage";

            return text.StartsWith(PRV_COURAGE_PREFIX);
        }

        public override string removePrefixFromText(string text)
        {
            return PRV_COURAGE_REGEX.Replace(text, "");
        }
    }
}
