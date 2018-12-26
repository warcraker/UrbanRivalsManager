namespace UrbanRivalsCore.Model.Cards.Skills.Prefixes
{
    public abstract class Prefix
    {
        public abstract bool isMatch(string text);
        public abstract string removePrefixFromText(string text);
        public virtual bool isSkillActiveAfterWinnerIsDecided(bool isThisPlayerTheRoundWinner, int thisPlayerFinalAttack, int enemyPlayerFinalAttack)
        {
            return isThisPlayerTheRoundWinner;
        }
    }
}
