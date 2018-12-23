using System.Text.RegularExpressions;

namespace UrbanRivalsCore.Model.Cards.Prefixes
{
    public class DefeatPrefix : Prefix
    {
        private static readonly Regex PRV_DEFEAT_REGEX = new Regex("^Defeat: ");

        public override bool isMatch(string text)
        {
            const string PRV_DEFEAT_PREFIX = "Defeat";

            return text.StartsWith(PRV_DEFEAT_PREFIX);
        }

        public override string removePrefixFromText(string text)
        {
            return PRV_DEFEAT_REGEX.Replace(text, "");
        }

        public override bool isSkillActiveAfterWinnerIsDecided(bool isThisPlayerTheRoundWinner, int thisPlayerFinalAttack, int enemyPlayerFinalAttack)
        {
            return !isThisPlayerTheRoundWinner;
        }
    }
}
