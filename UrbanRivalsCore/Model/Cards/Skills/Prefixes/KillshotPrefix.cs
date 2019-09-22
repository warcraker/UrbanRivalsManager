using System.Text.RegularExpressions;

namespace UrbanRivalsCore.Model.Cards.Skills.Prefixes
{
    public class KillshotPrefix : Prefix
    {
        private static readonly string TEXT_REPRESENTATION = Properties.GameStrings.skill_prefix_killshot;
        private static readonly Regex REGEX = new Regex("^Killshot:", RegexOptions.None);

        public override bool isMatch(string text)
        {
            return REGEX.IsMatch(text);
        }
        public override string removePrefixFromText(string text)
        {
            return REGEX.Replace(text, "");
        }
        public override string ToString()
        {
            return TEXT_REPRESENTATION;
        }

        // TODO reuse?
        public override bool isSkillActiveAfterWinnerIsDecided(bool isThisPlayerTheRoundWinner, int thisPlayerFinalAttack, int enemyPlayerFinalAttack)
        {
            return thisPlayerFinalAttack >= enemyPlayerFinalAttack * 2;
        }
    }
}
