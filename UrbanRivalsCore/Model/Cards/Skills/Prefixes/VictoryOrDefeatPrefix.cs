using System.Text.RegularExpressions;

namespace UrbanRivalsCore.Model.Cards.Skills.Prefixes
{
    public class VictoryOrDefeatPrefix : Prefix
    {
        private const string PRV_VICTORY_OR_DEFEAT_LONG_PREFIX = "Victory Or Defeat: ";
        private static readonly Regex PRV_VICTORY_OR_DEFEAT_REGEX = new Regex(@"^Victory Or Defeat: |^Vict\. Or Def\.: ");

        public override bool isMatch(string text)
        {
            const string PRV_VICTORY_OR_DEFEAT_SHORT_PREFIX = "Vict. Or Def.: ";

            return text.StartsWith(PRV_VICTORY_OR_DEFEAT_LONG_PREFIX) || text.StartsWith(PRV_VICTORY_OR_DEFEAT_SHORT_PREFIX);
        }
        public override string removePrefixFromText(string text)
        {
            return PRV_VICTORY_OR_DEFEAT_REGEX.Replace(text, "");
        }
        public override bool isSkillActiveAfterWinnerIsDecided(bool isThisPlayerTheRoundWinner, int thisPlayerFinalAttack, int enemyPlayerFinalAttack)
        {
            return true;
        }
        public override string ToString()
        {
            return PRV_VICTORY_OR_DEFEAT_LONG_PREFIX;
        }
    }
}
