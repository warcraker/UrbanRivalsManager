using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanRivalsCore.Model;

namespace UrbanRivalsCore.ViewModel
{
    public static class CombatFactory
    {
        /// <summary>
        /// Returns a new instance of <see cref="Combat"/> fit for tourney game mode.
        /// </summary>
        /// <param name="leftHand">Left player hand.</param>
        /// <param name="rightHand">Right player hand.</param>
        /// <param name="isLeftFirstPlayer">Is left player the first one to play on first round?</param>
        /// <param name="randomFactor">Random factor.</param>
        /// <returns></returns>
        public static Combat GetTourneyCombat(Hand leftHand, Hand rightHand, bool isLeftFirstPlayer, RandomFactor randomFactor)
        {
            return GetNormalCombat(leftHand, rightHand, isLeftFirstPlayer, randomFactor);
        }

        /// <summary>
        /// Returns a new instance of <see cref="Combat"/> fit for classic game mode.
        /// </summary>
        /// <param name="leftHand">Left player hand.</param>
        /// <param name="rightHand">Right player hand.</param>
        /// <param name="isLeftFirstPlayer">Is left player the first one to play on first round?</param>
        /// <param name="randomFactor">Random factor.</param>
        /// <returns></returns>
        public static Combat GetClassicCombat(Hand leftHand, Hand rightHand, bool isLeftFirstPlayer, RandomFactor randomFactor)
        {
            return GetNormalCombat(leftHand, rightHand, isLeftFirstPlayer, randomFactor);
        }

        /// <summary>
        /// Returns a new instance of <see cref="Combat"/> fit for solo game mode.
        /// </summary>
        /// <param name="leftHand">Left player hand.</param>
        /// <param name="rightHand">Right player hand.</param>
        /// <param name="isLeftFirstPlayer">Is left player the first one to play on first round?</param>
        /// <param name="randomFactor">Random factor.</param>
        /// <returns></returns>
        public static Combat GetSoloCombat(Hand leftHand, Hand rightHand, bool isLeftFirstPlayer, RandomFactor randomFactor)
        {
            return GetNormalCombat(leftHand, rightHand, isLeftFirstPlayer, randomFactor);
        }

        /// <summary>
        /// Returns a new instance of <see cref="Combat"/> fit for leader wars game mode.
        /// </summary>
        /// <param name="leftHand">Left player hand.</param>
        /// <param name="rightHand">Right player hand.</param>
        /// <param name="isLeftFirstPlayer">Is left player the first one to play on first round?</param>
        /// <param name="randomFactor">Random factor.</param>
        /// <returns></returns>
        public static Combat GetLeaderWarsCombat(Hand leftHand, Hand rightHand, bool isLeftFirstPlayer, RandomFactor randomFactor)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a new instance of <see cref="Combat"/> fit for ELO game mode.
        /// </summary>
        /// <param name="leftHand">Left player hand.</param>
        /// <param name="rightHand">Right player hand.</param>
        /// <param name="isLeftFirstPlayer">Is left player the first one to play on first round?</param>
        /// <returns></returns>
        public static Combat GetEloCombat(Hand leftHand, Hand rightHand, bool isLeftFirstPlayer)
        {
            return new Combat(leftHand, rightHand, 14, 12, isLeftFirstPlayer, RandomFactor.NonRandom);
        }

        /// <summary>
        /// Returns a new instance of <see cref="Combat"/> fit for survivor game mode.
        /// </summary>
        /// <param name="leftHand">Left player hand.</param>
        /// <param name="rightHand">Right player hand.</param>
        /// <param name="isLeftFirstPlayer">Is left player the first one to play on first round?</param>
        /// <param name="leftStage">Left player starting conditions.</param>
        /// <param name="rightStage">Right player starting conditions.</param>
        /// <returns></returns>
        public static Combat GetSurvivorCombat(SurvivorStage leftStage, SurvivorStage rightStage, Hand leftHand, Hand rightHand, bool isLeftFirstPlayer)
        {
            if ((int)leftStage > OldConstants.EnumMaxAllowedValues.SurvivorStage)
                throw new ArgumentOutOfRangeException(nameof(leftStage), leftStage, "Must be a valid " + nameof(SurvivorStage));
            if ((int)rightStage > OldConstants.EnumMaxAllowedValues.SurvivorStage)
                throw new ArgumentOutOfRangeException(nameof(rightStage), rightStage, "Must be a valid " + nameof(SurvivorStage));

            int leftPillz, rightPillz, leftLifes, rightLifes;
            GetSurvivorValues(leftStage, out leftLifes, out leftPillz);
            GetSurvivorValues(rightStage, out rightLifes, out rightPillz);
            return new Combat(leftHand, rightHand, leftLifes, rightLifes, leftPillz, rightPillz, isLeftFirstPlayer, RandomFactor.NonRandom);            
        }

        /// <summary>
        /// Returns a new instance of <see cref="Combat"/> fit for duel game mode.
        /// </summary>
        /// <param name="leftHand">Left player hand.</param>
        /// <param name="rightHand">Right player hand.</param>
        /// <param name="isLeftFirstPlayer">Is left player the first one to play on first round?</param>
        /// <param name="randomFactor">Random factor.</param>
        /// <returns></returns>
        public static Combat GetDuelCombat(Hand leftHand, Hand rightHand, bool isLeftFirstPlayer, RandomFactor randomFactor)
        {
            return GetNormalCombat(leftHand, rightHand, isLeftFirstPlayer, randomFactor);
        }

        /// <summary>
        /// Returns a new instance of <see cref="Combat"/> for a customized non leader wars game.
        /// </summary>
        /// <param name="leftHand">Left player hand.</param>
        /// <param name="rightHand">Right player hand.</param>
        /// <param name="initialLeftLife">Starting left player lives.</param>
        /// <param name="initialRightLife">Starting right player lives.</param>
        /// <param name="initialLeftPillz">Starting left player pillz.</param>
        /// <param name="initialRightPillz">Starting right player pillz.</param>
        /// <param name="isLeftFirstPlayer">Is left player the first one to play on first round?</param>
        /// <param name="randomFactor">Random factor.</param>
        /// <returns></returns>
        public static Combat GetCustomCombat(Hand leftHand, Hand rightHand, int initialLeftLife, int initialRightLife, int initialLeftPillz, int initialRightPillz, bool isLeftFirstPlayer, RandomFactor randomFactor)
        {
            return new Combat(leftHand, rightHand, initialLeftLife, initialRightLife, initialLeftPillz, initialRightPillz, isLeftFirstPlayer, randomFactor);
        }

        private static Combat GetNormalCombat(Hand leftHand, Hand rightHand, bool isLeftFirstPlayer, RandomFactor randomFactor)
        {
            return new Combat(leftHand, rightHand, 12, 12, isLeftFirstPlayer, randomFactor);
        }
        private static void GetSurvivorValues(SurvivorStage stage, out int lifes, out int pillz)
        {
            switch (stage)
            {
                case SurvivorStage.Stage1Pillz12Lives12:
                    pillz = 12;
                    lifes = 12;
                    break;
                case SurvivorStage.Stage2Pillz11Lives13:
                    pillz = 11;
                    lifes = 13;
                    break;
                case SurvivorStage.Stage3Pillz10Lives14:
                    pillz = 10;
                    lifes = 14;
                    break;
                case SurvivorStage.Stage4Pillz9Lives15:
                    pillz = 9;
                    lifes = 15;
                    break;
                case SurvivorStage.Stage5to9Pillz8Lives15:
                    pillz = 8;
                    lifes = 15;
                    break;
                default:
                    throw new Exception("Should never arrive here.");
            }
        }
    }
}
