using System.Text.RegularExpressions;

namespace UrbanRivalsCore.Model.Cards.Skills.Prefixes
{
    public class BacklashPrefix : Prefix
    {
        private const string PRV_BACKLASH_PREFIX = "Backlash: ";
        private static readonly Regex PRV_BACKLASH_REGEX = new Regex("^Backlash: ");

        public override bool isMatch(string text)
        {
            return text.StartsWith(PRV_BACKLASH_PREFIX);
        }
        public override string removePrefixFromText(string text)
        {
            return PRV_BACKLASH_REGEX.Replace(text, "");
        }
        public override string ToString()
        {
            return PRV_BACKLASH_PREFIX;
        }
    }
}
