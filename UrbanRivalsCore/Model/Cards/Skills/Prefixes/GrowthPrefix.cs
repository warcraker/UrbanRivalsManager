using System.Text.RegularExpressions;

namespace UrbanRivalsCore.Model.Cards.Skills.Prefixes
{
    public class GrowthPrefix : Prefix
    {
        private const string PRV_GROWTH_PREFIX = "Growth: ";
        private static readonly Regex PRV_GROWTH_REGEX = new Regex("Growth ?: ");

        public override bool isMatch(string text)
        {
            const string PRV_GROWTH_PREFIX_WITH_SPACE = "Growth :";

            return text.Contains(PRV_GROWTH_PREFIX)
                || text.Contains(PRV_GROWTH_PREFIX_WITH_SPACE)
                ;
        }
        public override string removePrefixFromText(string text)
        {
            return PRV_GROWTH_REGEX.Replace(text, "");
        }
        public override string ToString()
        {
            return PRV_GROWTH_PREFIX;
        }
    }
}
