namespace UrbanRivalsCore.Model
{
    public class RoundStatistics
    {
        public readonly int usedCardIndex;
        public readonly int usedPillzAmount;
        public readonly bool isFuryUsed;
        public readonly int initialPillzAmount;
        public readonly int initialLivesAmount;
        public readonly int attackValue;
        public readonly int damageValue;
        public readonly int powerValue;
        public readonly bool isAbilityActivated;
        public readonly bool isBonusActivated;
        public readonly bool isAbilityActivatedAtEnd;
        public readonly bool isBonusActivatedAtEnd;
        public readonly bool isFinisherMoveTriggered;
        public readonly bool isPoisonModified;
        public readonly bool isHealModified;
        public readonly int winProbability;

        public RoundStatistics(int usedCardIndex, int usedPillzAmount, bool isFuryUsed, int initialPillzAmount, int initialLivesAmount,
            int attackValue, int damageValue, int powerValue, bool isAbilityActivated, bool isBonusActivated, bool isAbilityActivatedAtEnd,
            bool isBonusActivatedAtEnd, bool isFinisherMoveTriggered, bool isPoisonModified, bool isHealModified, int winProbability)
        {
            this.usedCardIndex = usedCardIndex;
            this.usedPillzAmount = usedPillzAmount;
            this.isFuryUsed = isFuryUsed;
            this.initialPillzAmount = initialPillzAmount;
            this.initialLivesAmount = initialLivesAmount;
            this.attackValue = attackValue;
            this.damageValue = damageValue;
            this.powerValue = powerValue;
            this.isAbilityActivated = isAbilityActivated;
            this.isBonusActivated = isBonusActivated;
            this.isAbilityActivatedAtEnd = isAbilityActivatedAtEnd;
            this.isBonusActivatedAtEnd = isBonusActivatedAtEnd;
            this.isFinisherMoveTriggered = isFinisherMoveTriggered;
            this.isPoisonModified = isPoisonModified;
            this.isHealModified = isHealModified;
            this.winProbability = winProbability;
        }
    }
}