using System.Text.RegularExpressions;

namespace UrbanRivalsCore.Model.Cards.Skills.Prefixes
{
    public class DefeatPrefix : Prefix
    {
        private const string PRV_DEFEAT_PREFIX = "Defeat: ";
        private static readonly Regex PRV_DEFEAT_REGEX = new Regex("^Defeat ?: ");

        public override bool isMatch(string text)
        {
            const string PRV_DEFEAT_PREFIX_WITH_SPACE = "Defeat :";

            return text.StartsWith(PRV_DEFEAT_PREFIX)
                || text.StartsWith(PRV_DEFEAT_PREFIX_WITH_SPACE)
                ;
        }
        public override string removePrefixFromText(string text)
        {
            return PRV_DEFEAT_REGEX.Replace(text, "");
        }
        public override bool isSkillActiveAfterWinnerIsDecided(bool isThisPlayerTheRoundWinner, int thisPlayerFinalAttack, int enemyPlayerFinalAttack)
        {
            return !isThisPlayerTheRoundWinner;
        }
        public override string ToString()
        {
            return PRV_DEFEAT_PREFIX;
        }
    }
}
