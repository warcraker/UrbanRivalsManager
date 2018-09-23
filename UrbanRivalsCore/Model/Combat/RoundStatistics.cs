namespace UrbanRivalsCore.Model
{
    /// <summary>
    /// Contains data not reflected on the <see cref="PlayerStatus"/> of a player for a given round.
    /// </summary>
    public class RoundStatistics
    {
        /// <summary>
        /// Gets the index of the used card. Ranges from 0 to 3.
        /// </summary>
        public int UsedCard { get; internal set; }

        /// <summary>
        /// Gets the number of used pillz. Does not include Fury.
        /// </summary>
        public int UsedPillz { get; internal set; }

        /// <summary>
        /// Gets <c>true</c> if the player used fury this round. <c>false</c> otherwise.
        /// </summary>
        public bool UsedFury { get; internal set; }

        /// <summary>
        /// Gets the amount of pillz the player had at the start of the round.
        /// </summary>
        public int InitialPillz { get; internal set; }

        /// <summary>
        /// Gets the amount of lives the player had at the start of the round.
        /// </summary>
        public int InitialLives { get; internal set; }

        /// <summary>
        /// Gets the final attack value of the player.
        /// </summary>
        public int FinalAttack { get; internal set; }

        /// <summary>
        /// Gets the final damage value of the player, even if losing the round.
        /// </summary>
        public int FinalDamage { get; internal set; } 

        /// <summary>
        /// Gets the final power value of the player.
        /// </summary>
        public int FinalPower { get; internal set; }

        /// <summary>
        /// Gets <c>true</c> if the ability wasn't stopped by a stop opponent ability. <c>false</c> otherwise.
        /// </summary>
        public bool AbilityActivated { get; internal set; }

        /// <summary>
        /// Gets <c>true</c> if the bonus wasn't stopped by a stop opponent bonus. <c>false</c> otherwise.
        /// </summary>
        public bool BonusActivated { get; internal set; }

        /// <summary>
        /// Gets <c>true</c> if the ability was active at the end of the round. <c>false</c> otherwise.
        /// </summary>
        public bool AbilityActivatedEnd { get; internal set; }

        /// <summary>
        /// Gets <c>true</c> if the bonus was active at the end of the round. <c>false</c> otherwise.
        /// </summary>
        public bool BonusActivatedEnd { get; internal set; }

        /// <summary>
        /// Gets <c>true</c> if the player triggers the clan finisher move. <c>false</c> otherwise.
        /// <remark>The finisher move triggers when winning the round by having at least double the enemy's attack value.</remark>
        /// </summary>
        public bool FinisherMoveTriggers { get; internal set; }

        /// <summary>
        /// Gets <c>true</c> if the player's poison has changed this round. <c>false</c> otherwise.
        /// </summary>
        public bool PoisonChanges { get; internal set; }

        /// <summary>
        /// Gets <c>true</c> if the player's heal has changed this round. <c>false</c> otherwise.
        /// </summary>
        public bool HealChanges { get; internal set; }

        /// <summary>
        /// Gets the probability of winning the round. 100 means 100% win ratio, 0 means 0% win ratio.
        /// </summary>
        public int ProbabilityToWin { get; internal set; }

        // Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RoundStatistics"/> class. 
        /// </summary>
        public RoundStatistics() { }
    }
}