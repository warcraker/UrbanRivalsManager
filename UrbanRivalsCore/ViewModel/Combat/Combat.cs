using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using UrbanRivalsCore.Model;

namespace UrbanRivalsCore.ViewModel
{
    /// <summary>
    /// Keeps track of the status of a combat.
    /// <remarks>Use <see cref="CombatFactory"/> to create instances of this class.</remarks>
    /// </summary>
    public class Combat : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the random factor.
        /// </summary>
        public RandomFactor RandomFactor { get; private set; }

        private PlayerStatus m_LeftPlayerStatus;
        /// <summary>
        /// Gets the status of the left player. That is you.
        /// </summary>
        public PlayerStatus LeftPlayerStatus {
            get { return m_LeftPlayerStatus; }
            private set
            {
                m_LeftPlayerStatus = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LeftPlayerStatus)));
            }
        }

        private PlayerStatus m_RightPlayerStatus;
        /// <summary>
        /// Gets the status of the right player. That is the enemy player.
        /// </summary>
        public PlayerStatus RightPlayerStatus
        {
            get { return m_RightPlayerStatus; }
            private set
            {
                m_RightPlayerStatus = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RightPlayerStatus)));
            }
        }

        private int m_RoundCounter;
        /// <summary>
        /// Gets the number of the current round.
        /// </summary>
        public int RoundCounter
        {
            get { return m_RoundCounter; }
            private set
            {
                m_RoundCounter = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RoundCounter)));
            }
        }
        
        private CombatWinner m_CombatWinner;
        /// <summary>
        /// Gets the winner of the combat, if any.
        /// </summary>
        public CombatWinner CombatWinner
        {
            get { return m_CombatWinner; }
            private set
            {
                m_CombatWinner = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CombatWinner)));
            }
        }

        private Stack<PlayerStatus> HistoricLeftPlayerStatus = new Stack<PlayerStatus>();
        private Stack<PlayerStatus> HistoricRightPlayerStatus = new Stack<PlayerStatus>();

        // Constructors

        internal Combat(Hand leftHand, Hand rightHand, int initialLife, int initialPillz, bool isLeftFirstPlayer, RandomFactor randomFactor)
        {
            if (initialLife <= 0)
                throw new ArgumentOutOfRangeException(nameof(initialLife), initialLife, "Must be greater than 0");
            if (initialPillz < 0)
                throw new ArgumentOutOfRangeException(nameof(initialPillz), initialPillz, "Must be positive or 0");

            Init(leftHand, rightHand, initialLife, initialLife, initialPillz, initialPillz, isLeftFirstPlayer, randomFactor);
        }
        internal Combat(Hand leftHand, Hand rightHand, int initialLeftLife, int initialRightLife, int initialLeftPillz, int initialRightPillz, bool isLeftFirstPlayer, RandomFactor randomFactor)
        {
            if (initialLeftLife <= 0)
                throw new ArgumentOutOfRangeException(nameof(initialLeftLife), initialLeftLife, "Must be greater than 0");
            if (initialRightLife <= 0)
                throw new ArgumentOutOfRangeException(nameof(initialRightLife), initialRightLife, "Must be greater than 0");
            if (initialLeftPillz < 0)
                throw new ArgumentOutOfRangeException(nameof(initialLeftPillz), initialLeftPillz, "Must be positive or 0");
            if (initialRightPillz < 0)
                throw new ArgumentOutOfRangeException(nameof(initialRightPillz), initialRightPillz, "Must be positive or 0");

            Init(leftHand, rightHand, initialLeftLife, initialRightLife, initialLeftPillz, initialRightPillz, isLeftFirstPlayer, randomFactor);
        }

        internal static Combat CreateLeaderWarsCombat(Hand leftHand, Hand rightHand, int initialLife, int initialPillz, bool isLeftFirstPlayer)
        {
            if (leftHand == null)
                throw new ArgumentNullException(nameof(leftHand));
            if (leftHand.Leader == SkillLeader.None)
                throw new ArgumentException("Must have a single leader card at maximum level", nameof(leftHand));
            if (rightHand == null)
                throw new ArgumentNullException(nameof(rightHand));
            if (rightHand.Leader == SkillLeader.None)
                throw new ArgumentException("Must have a single leader card at maximum level", nameof(rightHand));

            Combat combat = new Combat(leftHand, rightHand, initialLife, initialPillz, isLeftFirstPlayer, RandomFactor.NonRandom)
            {
                LeftPlayerStatus = null //TODO .ConfigureForLeaderWars();
            };

            throw new NotImplementedException();
        }
        private void Init(Hand leftHand, Hand rightHand, int initialLeftLife, int initialRightLife, int initialLeftPillz, int initialRightPillz, bool isLeftFirstPlayer, RandomFactor randomFactor)
        {
            if (leftHand == null)
                throw new ArgumentNullException(nameof(leftHand));
            if (rightHand == null)
                throw new ArgumentNullException(nameof(rightHand));
            if (randomFactor != RandomFactor.NonRandom && randomFactor != RandomFactor.Random)
                throw new ArgumentOutOfRangeException(nameof(randomFactor), randomFactor, "Must be a valid " + nameof(RandomFactor));

            RoundCounter = 0;
            RandomFactor = randomFactor;

            LeftPlayerStatus = new PlayerStatus(initialLeftLife, initialLeftPillz, leftHand, isLeftFirstPlayer);
            RightPlayerStatus = new PlayerStatus(initialRightLife, initialRightPillz, rightHand, !isLeftFirstPlayer);

            LeftPlayerStatus.PropertyChanged += LeftPlayerStatus_PropertyChanged;
            RightPlayerStatus.PropertyChanged += RightPlayerStatus_PropertyChanged;
        }

        // Functions

        /// <summary>
        /// Simulates a round being played and shows the results. The state of the <see cref="Combat"/> does not change. Can't be executed if the combat already ended.
        /// </summary>
        /// <param name="leftUsedCard">Left played card. Must be between 0 and 3 inclusive.</param>
        /// <param name="rightUsedCard">Right played card. Must be between 0 and 3 inclusive.</param>
        /// <param name="leftUsedFury">Left player used fury. Only if remaining pillz allow it.</param>
        /// <param name="rightUsedFury">Right player used fury. Only if remaining pillz allow it.</param>
        /// <param name="leftUsedPillz">Left player used pillz. Must be positive or zero. Only remaining pillz can be used.</param>
        /// <param name="rightUsedPillz">Right player used pillz. Must be positive or zero. Only remaining pillz can be used.</param>
        /// <param name="forceWinnerOnRandom">If <seealso cref="RandomFactor"/> is set to <seealso cref="RandomFactor.Random"/> and win ratio is not 100% for either side, this determines who wins the round. Otherwise it has no effect.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">The combat already finished.</exception>
        public RoundResults PreviewRound(int leftUsedCard, int rightUsedCard, bool leftUsedFury, bool rightUsedFury, int leftUsedPillz, int rightUsedPillz, PlayerSide forceWinnerOnRandom = PlayerSide.None)
        {
            throw new NotImplementedException("Implement Equalizer Prefix"); // TODO
            throw new NotImplementedException("Implement Day/Night Prefix"); // TODO
            throw new NotImplementedException("Implement Brawl Prefix"); // TODO
            throw new NotImplementedException("Implement Degrowth Prefix"); // TODO

            /* DO NOT REFACTOR THIS METHOD !!!
             * Use the "PreviewRound() Steps.txt" document to orient yourself.
             * Each time new abilities appear in the game, this method has to be changed.
             * Should this be refactored, the complexity for changing the code would rise.
             */

            // Check for invalid calls and parameters 

            if (CombatWinner != CombatWinner.None)
                throw new InvalidOperationException("The combat already finished.");
            ValidatePreviewRoundThrowingOnInvalid(leftUsedCard, rightUsedCard, leftUsedFury, rightUsedFury, leftUsedPillz, rightUsedPillz, forceWinnerOnRandom);

            // --- PREPARATION STEP: Initial arrangements ---

            CardDrawed leftCard = LeftPlayerStatus.Hand[leftUsedCard];
            CardDrawed rightCard = RightPlayerStatus.Hand[rightUsedCard];
            SkillLeader leftLeader = LeftPlayerStatus.Hand[leftUsedCard].Ability.Leader;
            SkillLeader rightLeader = RightPlayerStatus.Hand[rightUsedCard].Ability.Leader;
            int initialLeftLife = LeftPlayerStatus.Life;
            int initialRightLife = RightPlayerStatus.Life;
            int initialLeftPillz = LeftPlayerStatus.Pillz;
            int initialRightPillz = RightPlayerStatus.Pillz;

            PlayerStatus finalLeftPlayerStatus = LeftPlayerStatus.Copy();
            PlayerStatus finalRightPlayerStatus = RightPlayerStatus.Copy();

            finalLeftPlayerStatus.Pillz -= CalculateUsedPillz(leftUsedPillz, leftUsedFury);
            finalRightPlayerStatus.Pillz -= CalculateUsedPillz(rightUsedPillz, rightUsedFury);

            CombatRoundSkillsHandler skills = new CombatRoundSkillsHandler(
                LeftPlayerStatus.Hand[leftUsedCard].Ability,    RightPlayerStatus.Hand[rightUsedCard].Ability,
                LeftPlayerStatus.Hand[leftUsedCard].Bonus,      RightPlayerStatus.Hand[rightUsedCard].Bonus);

            // There is one exceptional case where the card has a double prefix (DJ Korps, Id=1260) so we treat that one before
            skills.FixDJCorpsDoublePrefix(RoundCounter);

            // --- STEP 1: Apply Confidence, Courage, Growth, Reprisal, Revenge and Support Prefixes ---

            skills.ApplyCourageReprisalConfidenceRevengePrefixes(LeftPlayerStatus, RightPlayerStatus);
            skills.ApplyGrowthPrefix(RoundCounter);
            skills.ApplySupportPrefix(LeftPlayerStatus, RightPlayerStatus, leftCard, rightCard);

            // --- STEP 2: Apply Copy Bonus Suffix ---

            skills.ApplyCopyBonusSuffix();

            // --- STEP 3: Apply Stop and Protect Ability/Bonus Suffixes ---

            skills.ApplyStopProtectSuffixes();

            // --- STEP 4: Apply Stop Prefix ---

            skills.ApplyStopPrefix();

            // --- STEP 5: Apply Cancel and Protect Suffixes ---

            CanceledModifiers leftCanceledModifiers = CalculateCanceledModifiers(skills[SkillIndex.RA], skills[SkillIndex.RB]);
            CanceledModifiers rightCanceledModifiers = CalculateCanceledModifiers(skills[SkillIndex.LA], skills[SkillIndex.LB]);

            ProtectedStats leftProtectedStats = CalculatedProtectedStats(skills[SkillIndex.LA], skills[SkillIndex.LB]);
            ProtectedStats rightProtectedStats = CalculatedProtectedStats(skills[SkillIndex.RA], skills[SkillIndex.RB]);

            // --- STEP 6: Calculate Power (Ambre, Eyrik) ---

            // --- STEP 6.1: Calculate Base Power ---

            int leftPower = CalculateBasePower(LeftPlayerStatus, leftUsedCard);
            int rightPower = CalculateBasePower(RightPlayerStatus, rightUsedCard);

            // --- STEP 6.2: Apply Copy Power ---

            skills.ApplyCopyPowerSuffix(ref leftPower, ref rightPower, leftCanceledModifiers.Power(), rightCanceledModifiers.Power());

            // --- STEP 6.3: Calculate Power Increments ---
            
            BatchAdder increaseLeftPowerBatch = new BatchAdder();
            BatchAdder increaseRightPowerBatch = new BatchAdder();

            // --- STEP 6.3.1: Ambre ---

            ApplyLeaderAmbre(leftLeader, LeftPlayerStatus, increaseLeftPowerBatch);
            ApplyLeaderAmbre(rightLeader, RightPlayerStatus, increaseRightPowerBatch);

            // --- STEP 6.3.2: Normal Power Increment Skills ---

            skills.ApplyIncreasePowerSuffixes(increaseLeftPowerBatch, increaseRightPowerBatch);

            // --- STEP 6.3.3: Apply Power Increments ---

            leftPower = CalculatePowerIncrements(leftPower, leftCanceledModifiers.Power(), increaseLeftPowerBatch);
            rightPower = CalculatePowerIncrements(rightPower, rightCanceledModifiers.Power(), increaseRightPowerBatch);

            // --- STEP 6.4: Calculate Power Decrements ---

            BatchSubtracter decreaseLeftPowerBatch = new BatchSubtracter(); 
            BatchSubtracter decreaseRightPowerBatch = new BatchSubtracter();

            // --- STEP 6.4.1: Eyrik ---

            ApplyLeaderEyrik(leftLeader, decreaseRightPowerBatch);
            ApplyLeaderEyrik(rightLeader, decreaseLeftPowerBatch);

            // --- STEP 6.4.2: Normal Power Decrement Skills ---

            skills.ApplyDecreasePowerSuffixes(decreaseLeftPowerBatch, decreaseRightPowerBatch);

            // --- STEP 6.4.3: Apply Power Decrements ---

            leftPower = CalculatePowerDecrements(leftPower, rightCanceledModifiers.Power(), leftProtectedStats.Power(), decreaseLeftPowerBatch);
            rightPower = CalculatePowerDecrements(rightPower, leftCanceledModifiers.Power(), rightProtectedStats.Power(), decreaseRightPowerBatch);

            // --- STEP 7: Calculate Attack (Hugo) ---

            // --- STEP 7.1: Calculate Base Attack ---

            int leftAttack = CalculateBaseAttack(leftPower, leftUsedPillz);
            int rightAttack = CalculateBaseAttack(rightPower, rightUsedPillz);

            // --- STEP 7.2: Calculate Attack Increments ---

            BatchAdder increaseLeftAttackBatch = new BatchAdder();
            BatchAdder increaseRightAttackBatch = new BatchAdder();

            // --- STEP 7.2.1: Hugo ---

            ApplyLeaderHugo(leftLeader, increaseLeftAttackBatch);
            ApplyLeaderHugo(rightLeader, increaseRightAttackBatch);

            // --- STEP 7.2.2: Normal Attack Increment Skills ---

            skills.ApplyIncreaseAttackSuffixes(LeftPlayerStatus, RightPlayerStatus, increaseLeftAttackBatch, increaseRightAttackBatch);

            // --- STEP 7.2.3: Apply Attack Increments ---

            leftAttack = CalculateAttackIncrements(leftAttack, leftCanceledModifiers.Attack(), increaseLeftAttackBatch);
            rightAttack = CalculateAttackIncrements(rightAttack, rightCanceledModifiers.Attack(), increaseRightAttackBatch);

            // --- STEP 7.3: Calculate Attack Decrements ---

            BatchSubtracter decreaseLeftAttackBatch = new BatchSubtracter();
            BatchSubtracter decreaseRightAttackBatch = new BatchSubtracter();

            // --- STEP 7.3.1: Normal Attack Decrement Skills

            skills.ApplyDecreaseAttackSuffixes(decreaseLeftAttackBatch, decreaseRightAttackBatch, LeftPlayerStatus, RightPlayerStatus);

            // --- STEP 7.3.2: Apply Attack Decrements ---

            leftAttack = CalculateAttackDecrements(leftAttack, rightCanceledModifiers.Attack(), leftProtectedStats.Attack(), decreaseLeftAttackBatch);
            rightAttack = CalculateAttackDecrements(rightAttack, leftCanceledModifiers.Attack(), rightProtectedStats.Attack(), decreaseRightAttackBatch);

            // --- STEP 8: Determine Round Winner ---

            bool finisherMoveTriggers = false;
            PlayerSide whoHasSolomonActive = DetermineWhoHasSolomonActive(leftLeader, rightLeader);

            PlayerSide roundWinner = (RandomFactor == RandomFactor.NonRandom)
                ? DetermineRoundWinnerWithoutRandom(leftAttack, rightAttack, leftCard.Level, rightCard.Level, LeftPlayerStatus.Courage, whoHasSolomonActive)
                : DetermineRoundWinnerWithRandom(leftAttack, rightAttack, forceWinnerOnRandom, out finisherMoveTriggers);

            /* activationStatusBeforeDuel is used to track skills that we know are stopped before knowing who wins the round.
             * If the in-game UI would show a red X over the Skill, here the Skill will be shown stopped.
             * - Will show stopped status if stopped by other Skill.
             * - Will show stopped status if wrong timing for Confidence, Revenge, Courage, Reprisal or Stop prefixes.
             * - Leader skills will be treated as NoAbility. This doesn't change their behavior at all.
             * */
            var activationStatusBeforeDuel = skills.GetStatusArray();

            // --- STEP 9: Calculate Killshot Prefix ---

            skills.ApplyKillshotPrefix(leftAttack, rightAttack);

            // --- STEP 10: Calculate Damage (Timber, Vholt) ---

            // --- STEP 10.1: Calculate Base Damage ---

            int leftDamage = CalculateBaseDamage(LeftPlayerStatus, leftUsedCard);
            int rightDamage = CalculateBaseDamage(RightPlayerStatus, rightUsedCard);

            // --- STEP 10.2: Apply Copy Damage ---

            skills.ApplyCopyDamageSuffix(ref leftDamage, ref rightDamage, leftProtectedStats.Damage(), rightProtectedStats.Damage());

            // --- STEP 10.3: Calculate Damage Increments ---

            BatchAdder increaseLeftDamageBatch = new BatchAdder();
            BatchAdder increaseRightDamageBatch = new BatchAdder();

            // --- STEP 10.3.1: Fury ---

            ApplyFury(leftUsedFury, increaseLeftDamageBatch);
            ApplyFury(rightUsedFury, increaseRightDamageBatch);

            // --- STEP 10.3.2: Timber ---

            ApplyLeaderTimber(leftLeader, increaseLeftDamageBatch);
            ApplyLeaderTimber(rightLeader, increaseRightDamageBatch);

            // --- STEP 10.3.3: Normal Damage Increment Skills ---

            skills.ApplyIncreaseDamageSuffixes(increaseLeftDamageBatch, increaseRightDamageBatch);

            // --- STEP 10.3.4: Apply Damage Increments ---

            leftDamage = CalculateDamageIncrements(leftDamage, leftCanceledModifiers.Damage(), increaseLeftDamageBatch, leftUsedFury);
            rightDamage = CalculateDamageIncrements(rightDamage, rightCanceledModifiers.Damage(), increaseRightDamageBatch, rightUsedFury);

            // --- STEP 10.4: Calculate Damage Decrements ---

            BatchSubtracter decreaseLeftDamageBatch = new BatchSubtracter();
            BatchSubtracter decreaseRightDamageBatch = new BatchSubtracter();

            // --- STEP 10.4.1: Vholt ---

            ApplyLeaderVholt(leftLeader, decreaseRightDamageBatch);
            ApplyLeaderVholt(rightLeader, decreaseLeftDamageBatch);

            // --- STEP 10.4.2: Normal Damage Decrement Skills ---

            skills.ApplyDecreaseDamageSuffixees(decreaseLeftDamageBatch, decreaseRightDamageBatch);

            // --- STEP 10.4.3: Apply Decrements ---

            leftDamage = CalculateDamageDecrements(leftDamage, rightCanceledModifiers.Damage(), leftProtectedStats.Damage(), decreaseLeftDamageBatch);
            rightDamage = CalculateDamageDecrements(rightDamage, leftCanceledModifiers.Damage(), rightProtectedStats.Damage(), decreaseRightDamageBatch);

            // --- STEP 11: Apply Damage ---
            if (roundWinner == PlayerSide.Left)
                ApplyDamage(finalRightPlayerStatus, leftDamage);
            else
                ApplyDamage(finalLeftPlayerStatus, rightDamage);

            // --- STEP 12: Calculate Backlash, Defeat and VictoryOrDefeat Prefixes ---

            skills.ApplyBacklashDefeatVictoryOrDefeatPrefixes(roundWinner);

            // --- STEP 13: After Duel Skills (Melody) ---

            Poison poisonToLeft = null;
            Poison poisonToRight = null;
            Heal healToLeft = null;
            Heal healToRight = null;
            BatchAdder increaseLeftPillzBatch = new BatchAdder();
            BatchAdder increaseRightPillzBatch = new BatchAdder();
            BatchAdder increaseLeftLifeBatch = new BatchAdder();
            BatchAdder increaseRightLifeBatch = new BatchAdder();
            BatchSubtracter decreaseLeftLifeBatch = new BatchSubtracter();
            BatchSubtracter decreaseRightLifeBatch = new BatchSubtracter();
            BatchSubtracter decreaseLeftPillzBatch = new BatchSubtracter();
            BatchSubtracter decreaseRightPillzBatch = new BatchSubtracter();

            // --- STEP 13.1: Melody ---

            ApplyLeaderMelody(leftLeader, roundWinner, leftUsedPillz, leftUsedFury, increaseLeftPillzBatch);
            ApplyLeaderMelody(rightLeader, roundWinner, rightUsedPillz, rightUsedFury, increaseRightPillzBatch);

            // --- STEP 13.2: After Duel Skills ---

            skills.ApplyBacklashVariantSuffixes(decreaseLeftLifeBatch, decreaseRightLifeBatch, decreaseLeftPillzBatch, decreaseRightPillzBatch, 
                poisonToLeft, poisonToRight, leftCanceledModifiers, rightCanceledModifiers);

            skills.ApplyAfterDuelNormalSuffixes(decreaseLeftLifeBatch, decreaseRightLifeBatch,
            decreaseLeftPillzBatch, decreaseRightPillzBatch,
            increaseLeftLifeBatch, increaseRightLifeBatch,
            increaseLeftPillzBatch, increaseRightPillzBatch,
            healToLeft, healToRight, poisonToLeft, poisonToRight,
            leftDamage, rightDamage,
            leftUsedPillz, rightUsedPillz,
            leftUsedFury, rightUsedFury,
            leftCanceledModifiers, rightCanceledModifiers);

            // --- STEP 14: Apply Pillz Changes ---

            // --- STEP 14.1: Apply Pillz Increments ---

            if (!leftCanceledModifiers.Pillz())
                finalLeftPlayerStatus.Pillz = increaseLeftPillzBatch.CalculateAdditionsAndReset(finalLeftPlayerStatus.Pillz);
            if (!rightCanceledModifiers.Pillz())
                finalRightPlayerStatus.Pillz = increaseRightPillzBatch.CalculateAdditionsAndReset(finalRightPlayerStatus.Pillz);

            // --- STEP 14.2: Apply Pillz Decrements ---

            if (!rightCanceledModifiers.Pillz())
                finalLeftPlayerStatus.Pillz = decreaseLeftPillzBatch.CalculateSubstractionsAndReset(finalLeftPlayerStatus.Pillz);
            if (!leftCanceledModifiers.Pillz())
                finalRightPlayerStatus.Pillz = decreaseRightPillzBatch.CalculateSubstractionsAndReset(finalRightPlayerStatus.Pillz);

            // --- STEP 15: Apply Life Changes --- (Pun not intended =)

            // --- STEP 15.1: +Life ---

            if (finalLeftPlayerStatus.Life > 0 && !leftCanceledModifiers.Life())
                finalLeftPlayerStatus.Life = increaseLeftLifeBatch.CalculateAdditionsAndReset(finalLeftPlayerStatus.Life);
            if (finalRightPlayerStatus.Life > 0 && !rightCanceledModifiers.Life())
                finalRightPlayerStatus.Life = increaseRightLifeBatch.CalculateAdditionsAndReset(finalRightPlayerStatus.Life);

            // --- STEP 15.2: -Life ---

            if (!rightCanceledModifiers.Life())
                finalLeftPlayerStatus.Life = decreaseLeftLifeBatch.CalculateSubstractionsAndReset(finalLeftPlayerStatus.Life);
            if (!leftCanceledModifiers.Life())
                finalRightPlayerStatus.Life = decreaseRightLifeBatch.CalculateSubstractionsAndReset(finalRightPlayerStatus.Life);


            // --- STEP 15.3: Heal ---

            // For Heal we use the values from the previous round, that is, initial-PlayerStatus values

            if (!leftCanceledModifiers.Life())
                finalLeftPlayerStatus.Life += CalculateHeal(finalLeftPlayerStatus.Life, LeftPlayerStatus.Heal);

            if (!rightCanceledModifiers.Life())
                finalRightPlayerStatus.Life += CalculateHeal(finalRightPlayerStatus.Life, RightPlayerStatus.Heal);

            // --- STEP 15.4: Poison ---

            // Same as Heal, for Poison we use the values from the previous round, that is, initial-PlayerStatus values
            // If "Cancel Opp Life Modifier" is active, we don't check who applied poison (e.g. Backlash) but apply the cancels anyway

            if (!rightCanceledModifiers.Life())
                finalLeftPlayerStatus.Life -= CalculatePoison(finalLeftPlayerStatus.Life, LeftPlayerStatus.Poison);

            if (!leftCanceledModifiers.Life())
                finalRightPlayerStatus.Life -= CalculatePoison(finalRightPlayerStatus.Life, RightPlayerStatus.Poison);

            // --- STEP 16 Apply Post-Round Leader effects (Bridget, Morphun, Eklore) ---

            /* Although Bridget, Morphun and Eklore effects happen at the beginning of the rounds 2, 3 and 4,
             * because how the game works, it is the same they be happening at the very end of rounds 1, 2 and 3
             * */

            if (RoundCounter < 3)
            {
                switch (leftLeader)
                {
                    case SkillLeader.Bridget:
                        if (!leftCanceledModifiers.Life() && finalLeftPlayerStatus.Life > 0)
                            finalLeftPlayerStatus.Life++;
                        break;
                    case SkillLeader.Morphun:
                        if (!leftCanceledModifiers.Pillz() && finalLeftPlayerStatus.Pillz < 10)
                            finalLeftPlayerStatus.Pillz++;
                        break;
                }
                switch (rightLeader)
                {
                    case SkillLeader.Bridget:
                        if (!rightCanceledModifiers.Life() && finalRightPlayerStatus.Life > 0)
                            finalRightPlayerStatus.Life++;
                        break;
                    case SkillLeader.Morphun:
                        if (!rightCanceledModifiers.Pillz() && finalRightPlayerStatus.Pillz < 10)
                            finalRightPlayerStatus.Pillz++;
                        break;
                }

                // Morphun and Eklore can overlap, so we calculate first the increment, and then the decrement

                if (leftLeader == SkillLeader.Eklore && !leftCanceledModifiers.Pillz() && finalRightPlayerStatus.Pillz > 3)
                    finalRightPlayerStatus.Pillz--;
                if (rightLeader == SkillLeader.Eklore && !rightCanceledModifiers.Pillz() && finalLeftPlayerStatus.Pillz > 3)
                    finalLeftPlayerStatus.Pillz--; 
            }

            // --- LAST STEP: Final arrangements ---

            /* Used to track not stopped skills where the timing depends on who won the round.
             * Unlike activationStatusBeforeDuel, this status is not shown on the in-game UI.
             * - Skills already stopped on previous stages will be shown stopped here. This rule has priority.
             * - Will show stopped status if wrong timing for Backlash or Defeat prefixes. 
             * - VictoryOrDefeat prefix will not be marked as stopped here, timing is always correct.
             * - Skills without prefix (SkillPrefix.None) will show stopped if the round has been lost. 
             * - Leader skills will be treated as NoAbility. This doesn't change their behavior at all.
             * */

            var activationStatusAfterDuel = skills.GetStatusArray();

            // Set Courage, Reprisal, Confidence and Revenge

            ApplyTurnBasedStatus(finalLeftPlayerStatus, roundWinner == PlayerSide.Left);
            ApplyTurnBasedStatus(finalRightPlayerStatus, roundWinner == PlayerSide.Right);

            // If this round didn't changed, use previous Heal and Poison values

            finalLeftPlayerStatus.Heal = healToLeft ?? finalLeftPlayerStatus.Heal;
            finalRightPlayerStatus.Heal = healToRight ?? finalRightPlayerStatus.Heal;
            finalLeftPlayerStatus.Poison = poisonToLeft ?? finalLeftPlayerStatus.Poison;
            finalRightPlayerStatus.Poison = poisonToRight ?? finalRightPlayerStatus.Poison;

            // Set HandStatus data

            finalLeftPlayerStatus.Hand.Status.SetRoundData(RoundCounter, leftUsedCard, leftUsedFury, leftUsedPillz, roundWinner == PlayerSide.Left);
            finalRightPlayerStatus.Hand.Status.SetRoundData(RoundCounter, rightUsedCard, rightUsedFury, rightUsedPillz, roundWinner == PlayerSide.Right);

            // Fill Statistics

            int leftProbabilityToWin = CalculateWinProbability(leftAttack, rightAttack);
            int rightProbabilityToWin = 100 - leftProbabilityToWin;

            RoundStatistics leftStatistics = new RoundStatistics()
            {
                UsedCard = leftUsedCard,
                UsedPillz = leftUsedPillz,
                UsedFury = leftUsedFury,

                InitialPillz = initialLeftPillz,
                InitialLives = initialLeftLife,

                FinalAttack = leftAttack,
                FinalDamage = leftDamage,
                FinalPower = leftPower,

                AbilityActivated = (activationStatusBeforeDuel[(int)SkillIndex.LA] == ActivationStatus.Normal),
                BonusActivated = (activationStatusBeforeDuel[(int)SkillIndex.LB] == ActivationStatus.Normal),
                AbilityActivatedEnd = (activationStatusAfterDuel[(int)SkillIndex.LA] == ActivationStatus.Normal),
                BonusActivatedEnd = (activationStatusAfterDuel[(int)SkillIndex.LB] == ActivationStatus.Normal),

                PoisonChanges = (poisonToLeft != null),
                HealChanges = (healToLeft != null),

                ProbabilityToWin = leftProbabilityToWin,
            };

            RoundStatistics rightStatistics = new RoundStatistics()
            {
                UsedCard = rightUsedCard,
                UsedPillz = rightUsedPillz,
                UsedFury = rightUsedFury,

                InitialPillz = initialRightPillz,
                InitialLives = initialRightLife,

                FinalAttack = rightAttack,
                FinalDamage = rightDamage,
                FinalPower = rightPower,

                AbilityActivated = (activationStatusBeforeDuel[(int)SkillIndex.RA] == ActivationStatus.Normal),
                BonusActivated = (activationStatusBeforeDuel[(int)SkillIndex.RB] == ActivationStatus.Normal),
                AbilityActivatedEnd = (activationStatusAfterDuel[(int)SkillIndex.RA] == ActivationStatus.Normal),
                BonusActivatedEnd = (activationStatusAfterDuel[(int)SkillIndex.RB] == ActivationStatus.Normal),

                PoisonChanges = (poisonToRight != null),
                HealChanges = (healToRight != null),

                ProbabilityToWin = rightProbabilityToWin,
            };

            if (roundWinner == PlayerSide.Left)
                leftStatistics.FinisherMoveTriggers = finisherMoveTriggers;
            else
                rightStatistics.FinisherMoveTriggers = finisherMoveTriggers;

            // Return Round Results

            return new RoundResults(
                roundWinner,
                DetermineCombatWinner(finalLeftPlayerStatus.Life, finalRightPlayerStatus.Life, RoundCounter >= 3, whoHasSolomonActive), 
                finalLeftPlayerStatus, finalRightPlayerStatus, 
                leftStatistics, rightStatistics);
        }

        /// <summary>
        /// Plays a round and shows the results. The state of the <see cref="Combat"/> is updated accordingly.
        /// </summary>
        /// <param name="leftUsedCard">Left played card. Must be between 0 and 3 inclusive.</param>
        /// <param name="rightUsedCard">Right played card. Must be between 0 and 3 inclusive.</param>
        /// <param name="leftUsedFury">Left player used fury. Only if remaining pillz allow it.</param>
        /// <param name="rightUsedFury">Right player used fury. Only if remaining pillz allow it.</param>
        /// <param name="leftUsedPillz">Left player used pillz. Must be positive or zero. Only remaining pillz can be used.</param>
        /// <param name="rightUsedPillz">Right player used pillz. Must be positive or zero. Only remaining pillz can be used.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">The combat already finished.</exception>
        public RoundResults PlayRound(int leftUsedCard, int rightUsedCard, bool leftUsedFury, bool rightUsedFury, int leftUsedPillz, int rightUsedPillz, PlayerSide forceWinnerOnRandom = PlayerSide.None)
        {
            if (CombatWinner != CombatWinner.None)
                throw new InvalidOperationException("The combat already finished.");

            HistoricLeftPlayerStatus.Push(LeftPlayerStatus);
            HistoricRightPlayerStatus.Push(RightPlayerStatus);

            RoundResults RoundResultsThisRound = PreviewRound(leftUsedCard, rightUsedCard, leftUsedFury, rightUsedFury, leftUsedPillz, rightUsedPillz, forceWinnerOnRandom);

            LeftPlayerStatus = RoundResultsThisRound.LeftPlayerStatus;
            RightPlayerStatus = RoundResultsThisRound.RightPlayerStatus;

            CombatWinner = RoundResultsThisRound.CombatWinner;
            RoundCounter++; // Even if the combat ended, the counter is increased.

            return RoundResultsThisRound;
        }

        /// <summary>
        /// Puts the <see cref="Combat"/> in the state it was one round before.
        /// </summary>
        /// <exception cref="InvalidOperationException">Can't rewind on first round.</exception>
        public void RewindRound()
        {
            if (RoundCounter == 0)
                throw new InvalidOperationException("Can't rewind on first round.");

            LeftPlayerStatus = HistoricLeftPlayerStatus.Pop();
            RightPlayerStatus = HistoricRightPlayerStatus.Pop();

            RoundCounter--;
            CombatWinner = CombatWinner.None;
        }

        private void LeftPlayerStatus_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Courage" || e.PropertyName == "Reprisal")
            {
                if (LeftPlayerStatus.Courage)
                    RightPlayerStatus.Reprisal = true;
                else // LeftPlayerStatus.Reprisal
                    RightPlayerStatus.Courage = true;
            }
            else if (e.PropertyName == "Confidence" || e.PropertyName == "Revenge")
            {
                if (LeftPlayerStatus.Confidence)
                    RightPlayerStatus.Revenge = true;
                else if (LeftPlayerStatus.Revenge)
                    RightPlayerStatus.Confidence = true;
                else // Both false
                {
                    RightPlayerStatus.Confidence = false;
                    RightPlayerStatus.Revenge = false;
                }
            }
        }
        private void RightPlayerStatus_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Courage" || e.PropertyName == "Reprisal")
            {
                if (RightPlayerStatus.Courage)
                    LeftPlayerStatus.Reprisal = true;
                else // RightPlayerStatus.Reprisal
                    LeftPlayerStatus.Courage = true;
            }
            else if (e.PropertyName == "Confidence" || e.PropertyName == "Revenge")
            {
                if (RightPlayerStatus.Confidence)
                    LeftPlayerStatus.Revenge = true;
                else if (RightPlayerStatus.Revenge)
                    LeftPlayerStatus.Confidence = true;
                else // Both false
                {
                    LeftPlayerStatus.Confidence = false;
                    LeftPlayerStatus.Revenge = false;
                }
            }
        }

        private void ValidatePreviewRoundThrowingOnInvalid(int leftUsedCard, int rightUsedCard, bool leftUsedFury, bool rightUsedFury, int leftUsedPillz, int rightUsedPillz, PlayerSide forceWinnerOnRandom)
        {
            if (leftUsedCard < 0 || leftUsedCard > 3)
                throw new ArgumentOutOfRangeException(nameof(leftUsedCard), leftUsedCard, "Must be between 0 and 3 inclusive");
            if (LeftPlayerStatus.Hand.Status.HasCardBeingPlayed[leftUsedCard])
                throw new ArgumentException("It must be a card that hasn't been played yet", nameof(leftUsedCard));

            if (rightUsedCard < 0 || rightUsedCard > 3)
                throw new ArgumentOutOfRangeException(nameof(rightUsedCard), rightUsedCard, "Must be between 0 and 3 inclusive");
            if (RightPlayerStatus.Hand.Status.HasCardBeingPlayed[rightUsedCard])
                throw new ArgumentException("It must be a card that hasn't been played yet", nameof(rightUsedCard));

            if (leftUsedFury && LeftPlayerStatus.Pillz < 3)
                throw new ArgumentException($"The player needs a minimum of 3 pillz to use fury, it has {LeftPlayerStatus.Pillz} pillz left", nameof(leftUsedFury));
            if (rightUsedFury && RightPlayerStatus.Pillz < 3)
                throw new ArgumentException($"The player needs a minimum of 3 pillz to use fury, it has {RightPlayerStatus.Pillz} pillz left", nameof(rightUsedFury));

            if (leftUsedPillz < 0)
                throw new ArgumentException("Can't be a negative number", nameof(leftUsedPillz));
            int auxPillz = leftUsedPillz + (leftUsedFury ? 3 : 0);
            if (auxPillz > LeftPlayerStatus.Pillz)
                throw new ArgumentException($"Can't use more pillz that the available. Fury uses 3 extra pillz. Available = {LeftPlayerStatus.Pillz}. Used = {auxPillz}", nameof(leftUsedPillz));

            if (rightUsedPillz < 0)
                throw new ArgumentException("Can't be a negative number", nameof(rightUsedPillz));
            auxPillz = rightUsedPillz + (rightUsedFury ? 3 : 0);
            if (auxPillz > RightPlayerStatus.Pillz)
                throw new ArgumentException($"Can't use more pillz that the available. Fury uses 3 extra pillz. Available = {RightPlayerStatus.Pillz}. Used = {auxPillz}", nameof(rightUsedPillz));

            if (forceWinnerOnRandom < 0 || (int)forceWinnerOnRandom > Constants.EnumMaxAllowedValues.PlayerSide)
                throw new ArgumentException("It must be a valid " + nameof(PlayerSide), nameof(forceWinnerOnRandom));
        }

        /* Round Functions */

        private static int CalculateWinProbability(int leftAttack, int rightAttack)
        {
            PlayerSide whoDoubles = DetermineWhoDoublesAttack(leftAttack, rightAttack);
            if (whoDoubles == PlayerSide.Left)
                return 100;
            if (whoDoubles == PlayerSide.Right)
                return 0;
            return (int)(100 * leftAttack / rightAttack);
        }
        private static PlayerSide DetermineWhoDoublesAttack(int leftAttack, int rightAttack)
        {
            if (leftAttack >= (rightAttack * 2))
                return PlayerSide.Left;
            if (rightAttack >= (leftAttack * 2))
                return PlayerSide.Right;
            return PlayerSide.None;
        }
        private static PlayerSide DetermineWhoHasSolomonActive(SkillLeader leftLeader, SkillLeader rightLeader)
        {
            if (leftLeader == rightLeader)
                return PlayerSide.None;
            if (leftLeader == SkillLeader.Solomon)
                return PlayerSide.Left;
            if (rightLeader == SkillLeader.Solomon)
                return PlayerSide.Right;

            return PlayerSide.None;
        }
        private static PlayerSide DetermineRoundWinnerWithRandom(int leftAttack, int rightAttack, PlayerSide forceWinnerOnRandom, out bool finishMoveTriggers)
        {
            // If one has at least double the attack, wins automatically and performs a Finisher Move
            PlayerSide whoTriggersFinishMove = DetermineWhoDoublesAttack(leftAttack, rightAttack);
            if (whoTriggersFinishMove != PlayerSide.None)
            {
                finishMoveTriggers = true;
                return whoTriggersFinishMove;
            }

            finishMoveTriggers = false;

            // If each player has a chance to win, forceWinnerOnRandom can force the winner
            if (forceWinnerOnRandom != PlayerSide.None)
                return forceWinnerOnRandom;

            // If not, then the probability of winning is proportional to the Attack value
            int randomCombatResult = GlobalRandom.Next(0, leftAttack + rightAttack);
            return (randomCombatResult < leftAttack) ? PlayerSide.Left : PlayerSide.Right;
        }
        private static PlayerSide DetermineRoundWinnerWithoutRandom(int leftAttack, int rightAttack, int leftLevel, int rightLevel, bool leftHasCourage, PlayerSide whoHasSolomon)
        {
            /* 1. Compare Attacks, greater wins
               2. If equal Attacks, the player that has Solomon active wins
               3. If none (or both) have Solomon active, compare Levels, smaller wins
               4. If equal Levels, the one who has Courage wins */

            if (leftAttack > rightAttack)
                return PlayerSide.Left;
            if (rightAttack > leftAttack)
                return PlayerSide.Right;

            if (whoHasSolomon == PlayerSide.Left)
                return PlayerSide.Left;
            if (whoHasSolomon == PlayerSide.Right)
                return PlayerSide.Right;

            if (leftLevel < rightLevel)
                return PlayerSide.Left;
            if (rightLevel < leftLevel)
                return PlayerSide.Right;

            if (leftHasCourage)
                return PlayerSide.Left;
            return PlayerSide.Right;
        }
        private static CombatWinner DetermineCombatWinner(int leftLife, int rightLife, bool isLastRound = false, PlayerSide whoHasSolomon = PlayerSide.None)
        {
            CombatWinner result = DetermineCombatWinnerWithoutSolomon(leftLife, rightLife, isLastRound);
            if (result == CombatWinner.Draw)
            {
                if (whoHasSolomon == PlayerSide.Left)
                    return CombatWinner.Left;
                if (whoHasSolomon == PlayerSide.Right)
                    return CombatWinner.Right;
            }
            return result;
        }
        private static CombatWinner DetermineCombatWinnerWithoutSolomon(int leftLife, int rightLife, bool isLastRound = false)
        {
            bool leftKO = leftLife == 0;
            bool rightKO = leftLife == 0;

            if (leftKO && rightKO)
                return CombatWinner.Draw;
            if (leftKO)
                return CombatWinner.Right;
            if (rightKO)
                return CombatWinner.Left;

            if (!isLastRound)
                return CombatWinner.None;

            if (leftLife > rightLife)
                return CombatWinner.Left;
            if (rightLife > leftLife)
                return CombatWinner.Right;

            return CombatWinner.Draw;
        }

        /* Auxiliary Item Functions */

        private static void ApplyTurnBasedStatus(PlayerStatus playerStatus, bool hasWonTheRound)
        {
            playerStatus.Courage = !playerStatus.Courage;
            //playerStatus.Reprisal = !playerStatus.Reprisal; // implicit
            playerStatus.Confidence = hasWonTheRound;
            playerStatus.Revenge = !hasWonTheRound;
        }
        private static ProtectedStats CalculatedProtectedStats(Skill ability, Skill bonus)
        {
            return CalculatedProtectedStats(ability) | CalculatedProtectedStats(bonus);
        }
        private static ProtectedStats CalculatedProtectedStats(Skill skill)
        {
            switch (skill.Suffix)
            {
                case SkillSuffix.ProtectAttack:
                    return ProtectedStats.Attack;
                case SkillSuffix.ProtectDamage:
                    return ProtectedStats.Damage;
                case SkillSuffix.ProtectPower:
                    return ProtectedStats.Power;
                case SkillSuffix.ProtectPowerAndDamage:
                    return ProtectedStats.Power | ProtectedStats.Damage;
                default:
                    return ProtectedStats.None;
            }
        }
        private static CanceledModifiers CalculateCanceledModifiers(Skill enemyAbility, Skill enemyBonus)
        {
            return CalculateCancelModifiers(enemyAbility) | CalculateCancelModifiers(enemyBonus);
        }
        private static CanceledModifiers CalculateCancelModifiers(Skill enemySkill)
        {
            switch (enemySkill.Suffix)
            {
                case SkillSuffix.CancelAttackModifier:
                    return CanceledModifiers.Attack;
                case SkillSuffix.CancelDamageModifier:
                    return CanceledModifiers.Damage;
                case SkillSuffix.CancelLifeModifier:
                    return CanceledModifiers.Life;
                case SkillSuffix.CancelPillzModifier:
                    return CanceledModifiers.Pillz;
                case SkillSuffix.CancelPowerModifier:
                    return CanceledModifiers.Power;
                default:
                    return CanceledModifiers.None;
            }
        }

        /* Leaders */

        private static void ApplyLeaderAmbre(SkillLeader leader, PlayerStatus status, BatchAdder powerBatchAdder)
        {
            if (leader == SkillLeader.Ambre && status.Courage)
                powerBatchAdder.InsertAddition(3, 10);
        }
        private static void ApplyLeaderEyrik(SkillLeader leader, BatchSubtracter powerBatchSubstracter)
        {
            if (leader == SkillLeader.Eyrik)
                powerBatchSubstracter.InsertSubstraction(1, 5);
        }
        private static void ApplyLeaderHugo(SkillLeader leader, BatchAdder attackBatchAdder)
        {
            if (leader == SkillLeader.Hugo)
                attackBatchAdder.InsertAddition(6);
        }
        private static void ApplyLeaderMelody(SkillLeader leader, PlayerSide roundWinner, int usedPillz, bool usedFury, BatchAdder pillzBatchAdder)
        {
            if (leader == SkillLeader.Melody && roundWinner == PlayerSide.Right)
                pillzBatchAdder.InsertAddition(CalculateRecoveredPillz(usedPillz, usedFury, 1, 3));
        }
        private static void ApplyLeaderTimber(SkillLeader leader, BatchAdder damageBatchAdder)
        {
            if (leader == SkillLeader.Timber)
                damageBatchAdder.InsertAddition(1);
        }
        private static void ApplyLeaderVholt(SkillLeader leader, BatchSubtracter damageBatchSubstracter)
        {
            if (leader == SkillLeader.Vholt)
                damageBatchSubstracter.InsertSubstraction(2, 4);
        }

        /* Calculate Increments and Decrements */

        private static int CalculateAttackIncrements(int attack, bool areThisAttackModifiersCancelled, BatchAdder attackAdder)
        {
            return CalculateAttributeIncrements(attack, areThisAttackModifiersCancelled, attackAdder);
        }
        private static int CalculateDamageIncrements(int damage, bool areThisDamageModifiersCancelled, BatchAdder damageAdder, bool usedFury)
        {
            // Cancel Damage Modifier does affect everything but Fury, so if it applies, Fury is checked again to ensure it is applied
            if (!areThisDamageModifiersCancelled)
                return damageAdder.CalculateAdditionsAndReset(damage);
            return damage + (usedFury ? 2 : 0);
        }
        private static int CalculatePowerIncrements(int power, bool areThisPowerModifiersCancelled, BatchAdder powerAdder)
        {
            return CalculateAttributeIncrements(power, areThisPowerModifiersCancelled, powerAdder);
        }
        private static int CalculatePillzIncrements(int pillz, bool areThisPillzModifiersCancelled, BatchAdder pillzAdder)
        {
            return CalculateAttributeIncrements(pillz, areThisPillzModifiersCancelled, pillzAdder);
        }
        private static int CalculateLifeIncrements(int life, bool areThisLifeModifiersCancelled, BatchAdder lifeAdder)
        {
            return CalculateAttributeIncrements(life, areThisLifeModifiersCancelled, lifeAdder);
        }

        private static int CalculateAttackDecrements(int attack, bool areRivalAttackModifiersCancelled, bool isThisAttackStatProtected, BatchSubtracter attackSubstracter)
        {
            return CalculateAttributeDecrement(attack, areRivalAttackModifiersCancelled, isThisAttackStatProtected, attackSubstracter);
        }
        private static int CalculateDamageDecrements(int damage, bool areRivalDamageModifiesCancelled, bool isThisDamageStatProtected, BatchSubtracter damageSubstracter)
        {
            return CalculateAttributeDecrement(damage, areRivalDamageModifiesCancelled, isThisDamageStatProtected, damageSubstracter);
        }
        private static int CalculatePowerDecrements(int power, bool areRivalPowerModifiersCancelled, bool isThisPowerStatProtected, BatchSubtracter powerSubstracter)
        {
            return CalculateAttributeDecrement(power, areRivalPowerModifiersCancelled, isThisPowerStatProtected, powerSubstracter);
        }
        private static int CalculatePillzDecrements(int pillz, bool areRivalPillzModifiersCancelled, bool isThisPillzStatProtected, BatchSubtracter pillzSubstracter)
        {
            return CalculateAttributeDecrement(pillz, areRivalPillzModifiersCancelled, isThisPillzStatProtected, pillzSubstracter);
        }
        private static int CalculateLifeDecrements(int life, bool areRivalLifeModifiersCancelled, bool isThisLifeStatProtected, BatchSubtracter lifeSubstracter)
        {
            return CalculateAttributeDecrement(life, areRivalLifeModifiersCancelled, isThisLifeStatProtected, lifeSubstracter);
        }

        private static int CalculateAttributeIncrements(int attribute, bool areThisAttributeModifiersCancelled, BatchAdder attributeAdder)
        {
            if (!areThisAttributeModifiersCancelled)
                return attributeAdder.CalculateAdditionsAndReset(attribute);
            return attribute;
        }
        private static int CalculateAttributeDecrement(int attribute, bool areRivalAttributeModifiersCancelled, bool isThisAttriubteStatProtected, BatchSubtracter attributeSubtracter)
        {
            if (!areRivalAttributeModifiersCancelled && !isThisAttriubteStatProtected)
                return attributeSubtracter.CalculateSubstractionsAndReset(attribute);
            return attribute;
        }

        /* Other Attribute Functions */

        private static int CalculateBaseAttack(int power, int usedPillz)
        {
            return power * (usedPillz + 1);
        }
        private static int CalculateBasePower(PlayerStatus playerStatus, int usedCard)
        {
            return playerStatus.Hand[usedCard].Power;
        }
        private static int CalculateBaseDamage(PlayerStatus playerStatus, int usedCard)
        {
            return playerStatus.Hand[usedCard].Damage;
        }
        private static int CalculateUsedPillz(int usedPillz, bool usedFury)
        {
            return usedPillz - (usedFury ? 3 : 0);
        }
        private static int CalculateRecoveredPillz(int usedPillz, bool usedFury, int x, int y)
        {
            usedPillz = +((usedFury) ? 3 : 0);

            if (usedPillz <= 1)
                return 1;

            int result = (usedPillz / y) * x;
            result += Math.Min(usedPillz % y, x);

            return result;
        }
        private static int CalculatePoison(int initialLives, Poison poison)
        {
            if (initialLives < poison.Min)
                return 0;

            return initialLives - Math.Max(poison.Min, initialLives - poison.Value);
        }
        private static int CalculateHeal(int initialLives, Heal heal)
        {
            if (initialLives == 0 || initialLives >= heal.Max)
                return 0;

            return Math.Min(heal.Max, initialLives + heal.Value) - initialLives;
        }
        private static void ApplyFury(bool usedFury, BatchAdder damageBatchAdder)
        {
            if (usedFury)
                damageBatchAdder.InsertAddition(2);
        }
        private static void ApplyDamage(PlayerStatus playerStatus, int damage)
        {
            playerStatus.Life -= damage;
        }
    }
}