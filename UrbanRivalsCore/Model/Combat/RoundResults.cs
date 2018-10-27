namespace UrbanRivalsCore.Model
{
    public class RoundResults
    {
        public readonly PlayerSide roundWinner;
        public readonly CombatWinner combatWinner;
        public readonly PlayerStatus leftPlayerStatus;
        public readonly PlayerStatus rightPlayerStatus;
        public readonly RoundStatistics leftPlayerStatistics;
        public readonly RoundStatistics rightPlayerStatistics;

        public RoundResults(PlayerSide roundWinner, CombatWinner combatWinner, PlayerStatus leftPlayerStatus, PlayerStatus rightPlayerStatus, RoundStatistics leftPlayerStatistics, RoundStatistics rightPlayerStatistics)
        {
            this.combatWinner = combatWinner;
            this.roundWinner = roundWinner;
            this.leftPlayerStatus = leftPlayerStatus;
            this.rightPlayerStatus = rightPlayerStatus;
            this.leftPlayerStatistics = leftPlayerStatistics;
            this.rightPlayerStatistics = rightPlayerStatistics;
        }
    }
}