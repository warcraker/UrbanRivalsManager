using System.Text.RegularExpressions;

namespace UrbanRivalsCore.Model.Cards.Skills.Prefixes
{
    public class CouragePrefix : Prefix
    {
        private const string PRV_COURAGE_PREFIX = "Courage: ";
        private static readonly Regex PRV_COURAGE_REGEX = new Regex("^Courage[:;] ");

        public override bool isMatch(string text)
        {
            return text.StartsWith(PRV_COURAGE_PREFIX);
        }
        public override string removePrefixFromText(string text)
        {
            return PRV_COURAGE_REGEX.Replace(text, "");
        }
        public override string ToString()
        {
            return PRV_COURAGE_PREFIX;
        }
    }
}
