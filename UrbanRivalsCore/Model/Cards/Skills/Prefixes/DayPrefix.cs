using System.Text.RegularExpressions;

namespace UrbanRivalsCore.Model.Cards.Skills.Prefixes
{
    public class DayPrefix : Prefix
    {
        private const string PRV_DAY_PREFIX = "Day: ";
        private static readonly Regex PRV_DAY_REGEX = new Regex("^Day: ");

        public override bool isMatch(string text)
        {
            return text.StartsWith(PRV_DAY_PREFIX);
        }
        public override string removePrefixFromText(string text)
        {
            return PRV_DAY_REGEX.Replace(text, "");
        }
        public override string ToString()
        {
            return PRV_DAY_PREFIX;
        }
    }
}
