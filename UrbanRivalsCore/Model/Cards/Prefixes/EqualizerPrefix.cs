using System.Text.RegularExpressions;

namespace UrbanRivalsCore.Model.Cards.Prefixes
{
    public class EqualizerPrefix : Prefix
    {
        private static readonly Regex PRV_EQUALIZER_REGEX = new Regex("^Equalizer: ");

        public override bool isMatch(string text)
        {
            const string PRV_EQUALIZER_PREFIX = "Equalizer";

            return text.StartsWith(PRV_EQUALIZER_PREFIX);
        }

        public override string removePrefixFromText(string text)
        {
            return PRV_EQUALIZER_REGEX.Replace(text, "");
        }
    }
}
