using System.Text.RegularExpressions;

namespace UrbanRivalsCore.Model.Cards.Skills.Prefixes
{
    public class KillshotPrefix : Prefix
    {
        private static readonly Regex PRV_KILLSHOT_REGEX = new Regex("^Killshot: ");

        public override bool isMatch(string text)
        {
            const string PRV_KILLSHOT_PREFIX = "Killshot: ";

            return text.StartsWith(PRV_KILLSHOT_PREFIX);
        }

        public override string removePrefixFromText(string text)
        {
            return PRV_KILLSHOT_REGEX.Replace(text, "");
        }

        public override bool isSkillActiveAfterWinnerIsDecided(bool isThisPlayerTheRoundWinner, int thisPlayerFinalAttack, int enemyPlayerFinalAttack)
        {
            return thisPlayerFinalAttack >= enemyPlayerFinalAttack * 2;
        }
    }
}
