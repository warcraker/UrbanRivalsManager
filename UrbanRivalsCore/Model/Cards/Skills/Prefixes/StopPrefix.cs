using System.Text.RegularExpressions;

namespace UrbanRivalsCore.Model.Cards.Skills.Prefixes
{
    public class StopPrefix : Prefix
    {
        private static readonly Regex PRV_STOP_REGEX = new Regex("^Stop: ");

        public override bool isMatch(string text)
        {
            const string PRV_STOP_PREFIX = "Stop:";

            return text.StartsWith(PRV_STOP_PREFIX);
        }

        public override string removePrefixFromText(string text)
        {
            return PRV_STOP_REGEX.Replace(text, "");
        }
    }
}
