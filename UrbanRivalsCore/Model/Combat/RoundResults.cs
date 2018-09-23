namespace UrbanRivalsCore.Model
{
    /// <summary>
    /// Contains data about the result of a round.
    /// </summary>
    public class RoundResults
    {
        /// <summary>
        /// Gets the winner of the round.
        /// </summary>
        public PlayerSide RoundWinner { get; private set; }

        /// <summary>
        /// Gets the winner of the combat, if any.
        /// </summary>
        public CombatWinner CombatWinner { get; private set; }

        /// <summary>
        /// Gets the left player status after the round ends.
        /// </summary>
        public PlayerStatus LeftPlayerStatus { get; private set; }

        /// <summary>
        /// Gets the right player status after the round ends.
        /// </summary>
        public PlayerStatus RightPlayerStatus { get; private set; }

        /// <summary>
        /// Gets the left player statistics of the round.
        /// </summary>
        public RoundStatistics LeftPlayerStatistics { get; private set; }

        /// <summary>
        /// Gets the right player statistics of the round.
        /// </summary>
        public RoundStatistics RightPlayerStatistics { get; private set; }

        internal RoundResults(PlayerSide roundWinner, CombatWinner combatWinner, PlayerStatus leftPlayerStatus, PlayerStatus rightPlayerStatus, RoundStatistics leftPlayerStatistics, RoundStatistics rightPlayerStatistics)
        {
            CombatWinner = combatWinner;
            RoundWinner = roundWinner;
            LeftPlayerStatus = leftPlayerStatus;
            RightPlayerStatus = rightPlayerStatus;
            LeftPlayerStatistics = leftPlayerStatistics;
            RightPlayerStatistics = rightPlayerStatistics;
        }
    }
}