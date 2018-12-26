using System.Text.RegularExpressions;

namespace UrbanRivalsCore.Model.Cards.Skills.Prefixes
{
    public class EqualizerPrefix : Prefix
    {
        private const string PRV_EQUALIZER_PREFIX = "Equalizer: ";
        private static readonly Regex PRV_EQUALIZER_REGEX = new Regex("^Equalizer: ");

        public override bool isMatch(string text)
        {
            return text.StartsWith(PRV_EQUALIZER_PREFIX);
        }
        public override string removePrefixFromText(string text)
        {
            return PRV_EQUALIZER_REGEX.Replace(text, "");
        }
        public override string ToString()
        {
            return PRV_EQUALIZER_PREFIX;
        }
    }
}
