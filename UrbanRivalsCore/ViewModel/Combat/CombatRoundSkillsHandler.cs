using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanRivalsCore.Model;

namespace UrbanRivalsCore.ViewModel
{
    internal class CombatRoundSkillsHandler
    {
        Skill[] skills = new Skill[4];
        ActivationStatus[] status = new ActivationStatus[4];

        public CombatRoundSkillsHandler(Skill leftAbility, Skill rightAbility, Skill leftBonus, Skill rightBonus)
        {
            skills[(int)SkillIndex.LA] = leftAbility.Copy();
            skills[(int)SkillIndex.RA] = rightAbility.Copy();
            skills[(int)SkillIndex.LB] = leftBonus.Copy();
            skills[(int)SkillIndex.RB] = rightBonus.Copy();
        }

        public Skill this[SkillIndex index]
        {
            get
            {
                return this[(int)index];
            }
            private set
            {
                this[(int)index] = value;
            }
        }
        public Skill this[int index]
        {
            get
            {
                if (status[index] == ActivationStatus.Normal)
                    return skills[index];

                if (index == (int)SkillIndex.LA || index == (int)SkillIndex.RA)
                    return Skill.NoAbility;

                return Skill.NoBonus;
            }
            private set
            {
                skills[index] = value;
            }
        }

        public ActivationStatus GetStatus(int index)
        {
            return status[index];
        }
        public ActivationStatus GetStatus(SkillIndex index)
        {
            return GetStatus((int)index);
        }
        public ActivationStatus[] GetStatusArray()
        {
            return (ActivationStatus[])status.Clone();
        }
        public void FreezeSkill(int index)
        {
            status[index] = ActivationStatus.Stopped;
        }
        public void FreezeSkill(SkillIndex index)
        {
            FreezeSkill((int)index);
        }
        public void UnfreezeSkill(int index)
        {
            status[index] = ActivationStatus.Normal;
        }
        public void UnfreezeSkill(SkillIndex index)
        {
            UnfreezeSkill((int)index);
        }

        /* Apply Prefixes */

        public void ApplyCourageReprisalConfidenceRevengePrefixes(PlayerStatus leftPlayerStatus, PlayerStatus rightPlayerStatus)
        {
            if (leftPlayerStatus == null)
                throw new ArgumentNullException(nameof(leftPlayerStatus));
            if (rightPlayerStatus == null)
                throw new ArgumentNullException(nameof(rightPlayerStatus));

            for (int i = 0; i < 4; i++)
            {
                bool isLeft = IsLeft(i);
                switch (this[i].Prefix)
                {
                    case SkillPrefix.Courage:
                        if (!((isLeft) ? leftPlayerStatus.Courage : rightPlayerStatus.Courage))
                            FreezeSkill(i);
                        break;
                    case SkillPrefix.Reprisal:
                        if (!((isLeft) ? leftPlayerStatus.Reprisal : rightPlayerStatus.Reprisal))
                            FreezeSkill(i);
                        break;
                    case SkillPrefix.Confidence:
                        if (!((isLeft) ? leftPlayerStatus.Confidence : rightPlayerStatus.Confidence))
                            FreezeSkill(i);
                        break;
                    case SkillPrefix.Revenge:
                        if (!((isLeft) ? leftPlayerStatus.Revenge : rightPlayerStatus.Revenge))
                            FreezeSkill(i);
                        break;
                }
            }
        }
        public void ApplyGrowthPrefix(int roundCounter)
        {
            for (int i = 0; i < 4; i++)
            {
                if (this[i].Prefix == SkillPrefix.Growth)
                    skills[i] = skills[i].CopyWithDifferentX(CalculateGrowth(skills[i], roundCounter));
            }
        }
        public void ApplySupportPrefix(PlayerStatus leftPlayerStatus, PlayerStatus rightPlayerStatus, CardDrawed leftCard, CardDrawed rightCard)
        {
            for (int i = 0; i < 4; i++)
            {
                bool isLeft = IsLeft(i);
                switch (this[i].Prefix)
                {
                    case SkillPrefix.Support:
                        if (isLeft)
                            skills[i] = skills[i].CopyWithDifferentX(CalculateSupportValue(leftPlayerStatus, leftCard));
                        else
                            skills[i] = skills[i].CopyWithDifferentX(CalculateSupportValue(rightPlayerStatus, rightCard));
                        break;
                }
            }
        }
        public void ApplyStopPrefix()
        {
            for (int i = 0; i < 4; i++)
            {
                if (skills[i].Prefix == SkillPrefix.Stop)
                {
                    if (GetStatus(i) == ActivationStatus.Stopped)
                        UnfreezeSkill(i);
                    else
                        FreezeSkill(i);
                }
            }
        }
        public void ApplyKillshotPrefix(int leftAttack, int rightAttack)
        {
            for (int i = 0; i < 4; i++)
            {
                bool isLeft = IsLeft(i);
                switch (this[i].Prefix)
                {
                    case SkillPrefix.Killshot:
                        if (isLeft)
                        {
                            if (!IsKillshotTriggered(leftAttack, rightAttack))
                                FreezeSkill(i);
                        }
                        else
                        {
                            if (!IsKillshotTriggered(rightAttack, leftAttack))
                                FreezeSkill(i);
                        }
                        break;
                }
            }
        }
        public void ApplyBacklashDefeatVictoryOrDefeatPrefixes(PlayerSide roundWinner)
        {
            for (int i = 0; i < 4; i++)
            {
                bool isLeft = IsLeft(i);
                switch (this[i].Prefix)
                {
                    case SkillPrefix.None:
                    case SkillPrefix.Backlash:
                        if (isLeft)
                        {
                            if (roundWinner != PlayerSide.Left)
                                FreezeSkill(i);
                        }
                        else
                        {
                            if (roundWinner != PlayerSide.Right)
                                FreezeSkill(i);
                        }
                        break;
                    case SkillPrefix.Defeat:
                        if (isLeft)
                        {
                            if (roundWinner == PlayerSide.Left)
                                FreezeSkill(i);
                        }
                        else
                        {
                            if (roundWinner == PlayerSide.Right)
                                FreezeSkill(i);
                        }
                        break;
                    // This case is redundant, but for the sake of legibility
                    case SkillPrefix.VictoryOrDefeat:
                        // It is always active (unless previously stopped) so no action needed to be taken
                        break;
                }
            }
        }

        /* Apply Suffixes */

        public void ApplyCopyBonusSuffix()
        {
            if (this[SkillIndex.LA].Suffix == SkillSuffix.CopyBonus &&
                this[SkillIndex.RB].Suffix != SkillSuffix.CancelLeader)
                skills[(int)SkillIndex.LA] = skills[(int)SkillIndex.RB].Copy();

            if (this[SkillIndex.RA].Suffix == SkillSuffix.CopyBonus &&
                this[SkillIndex.LB].Suffix != SkillSuffix.CancelLeader)
                skills[(int)SkillIndex.RA] = skills[(int)SkillIndex.LB].Copy();
        }
        public void ApplyStopProtectSuffixes()
        {
            StopProtectCalculator.CalculateAndApplyStopProtectChains(this);
        }
        public void ApplyCopyPowerSuffix(ref int leftPower, ref int rightPower, bool areLeftPowerModifiersCancelled, bool areRightPowerModifiersCancelled)
        {
            bool leftDoesCopy = false;
            if (!areLeftPowerModifiersCancelled)
            {
                var suffix = this[SkillIndex.LA].Suffix;
                leftDoesCopy = (suffix == SkillSuffix.CopyPower || suffix == SkillSuffix.CopyPowerAndDamage);
            }

            bool rightDoesCopy = false;
            if (!areRightPowerModifiersCancelled)
            {
                var suffix = this[SkillIndex.RA].Suffix;
                rightDoesCopy = (suffix == SkillSuffix.CopyPower || suffix == SkillSuffix.CopyPowerAndDamage);
            }

            int auxLeftPower = (leftDoesCopy) ? leftPower : rightPower;
            int auxRightPower = (rightDoesCopy) ? rightPower : leftPower;

            leftPower = auxLeftPower;
            rightPower = auxRightPower;
        }
        public void ApplyCopyDamageSuffix(ref int leftDamage, ref int rightDamage, bool areLeftDamageModifiersCancelled, bool areRightDamageModifiersCancelled)
        {
            bool leftDoesCopy = false;
            if (!areLeftDamageModifiersCancelled)
            {
                var suffix = this[SkillIndex.LA].Suffix;
                leftDoesCopy = (suffix == SkillSuffix.CopyDamage || suffix == SkillSuffix.CopyPowerAndDamage);
            }

            bool rightDoesCopy = false;
            if (!areRightDamageModifiersCancelled)
            {
                var suffix = this[SkillIndex.RA].Suffix;
                rightDoesCopy = (suffix == SkillSuffix.CopyDamage || suffix == SkillSuffix.CopyPowerAndDamage);
            }

            int auxLeftDamage = (leftDoesCopy) ? leftDamage : rightDamage;
            int auxRightDamage = (rightDoesCopy) ? rightDamage : leftDamage;

            leftDamage = auxLeftDamage;
            rightDamage = auxRightDamage;
        }
        public void ApplyDecreaseAttackSuffixes(BatchSubtracter leftAttackSubstracter, BatchSubtracter rightAttackSubstracter, 
            PlayerStatus initialLeftPlayerStatus, PlayerStatus initialRightPlayerStatus)
        {
            for (int i = 0; i < 4; i++)
            {
                bool isLeft = IsLeft(i);
                switch (this[i].Suffix)
                {
                    case SkillSuffix.DecreaseAttackXMinY:
                        if (isLeft)
                            rightAttackSubstracter.InsertSubstraction(skills[i].X, skills[i].Y);
                        else
                            leftAttackSubstracter.InsertSubstraction(skills[i].X, skills[i].Y);
                        break;
                    case SkillSuffix.DecreaseAttackXPerRemainingLifeMinY:
                        if (isLeft)
                            rightAttackSubstracter.InsertSubstraction(skills[i].X * initialLeftPlayerStatus.Life, skills[i].Y);
                        else
                            leftAttackSubstracter.InsertSubstraction(skills[i].X * initialRightPlayerStatus.Life, skills[i].Y);
                        break;
                }
            }
        }
        public void ApplyDecreaseDamageSuffixees(BatchSubtracter leftDamageSubstracter, BatchSubtracter rightDamageSubstracter)
        {
            for (int i = 0; i < 4; i++)
            {
                bool isLeft = IsLeft(i);
                switch (this[i].Suffix)
                {
                    case SkillSuffix.DecreaseDamageXMinY:
                        if (isLeft)
                            rightDamageSubstracter.InsertSubstraction(skills[i].X, skills[i].Y);
                        else
                            leftDamageSubstracter.InsertSubstraction(skills[i].X, skills[i].Y);
                        break;
                    case SkillSuffix.DecreasePowerAndDamageXMinY:
                        if (isLeft)
                            rightDamageSubstracter.InsertSubstraction(skills[i].X, skills[i].Y);
                        else
                            leftDamageSubstracter.InsertSubstraction(skills[i].X, skills[i].Y);
                        break;
                }
            }
        }
        public void ApplyDecreasePowerSuffixes(BatchSubtracter leftPowerSubstracter, BatchSubtracter rightPowerSubstracter)
        {
            for (int i = 0; i < 4; i++)
            {
                bool isLeft = IsLeft(i);
                switch (this[i].Suffix)
                {
                    case SkillSuffix.DecreasePowerAndDamageXMinY:
                    case SkillSuffix.DecreasePowerXMinY:
                        if (isLeft)
                            rightPowerSubstracter.InsertSubstraction(skills[i].X, skills[i].Y);
                        else
                            leftPowerSubstracter.InsertSubstraction(skills[i].X, skills[i].Y);
                        break;
                }
            }
        }
        public void ApplyIncreaseAttackSuffixes(PlayerStatus initialLeftPlayerStatus, PlayerStatus initialRightPlayerStatus, 
            BatchAdder leftAttackAdder, BatchAdder rightAttackAdder)
        {
            for (int i = 0; i < 4; i++)
            {
                bool isLeft = IsLeft(i);
                switch (this[i].Suffix)
                {
                    case SkillSuffix.IncreaseAttackX:
                        if (isLeft)
                            leftAttackAdder.InsertAddition(skills[i].X);
                        else
                            rightAttackAdder.InsertAddition(skills[i].X);
                        break;
                    case SkillSuffix.IncreaseAttackXPerRemainingLife:
                        if (isLeft)
                            leftAttackAdder.InsertAddition(skills[i].X * initialLeftPlayerStatus.Life);
                        else
                            rightAttackAdder.InsertAddition(skills[i].X * initialRightPlayerStatus.Life);
                        break;
                    case SkillSuffix.IncreaseAttackXPerRemainingPillz:
                        if (isLeft)
                            leftAttackAdder.InsertAddition(skills[i].X * initialLeftPlayerStatus.Pillz);
                        else
                            rightAttackAdder.InsertAddition(skills[i].X * initialRightPlayerStatus.Pillz);
                        break;
                }
            }
        }
        public void ApplyIncreaseDamageSuffixes(BatchAdder leftDamageAdder, BatchAdder rightDamageAdder)
        {
            for (int i = 0; i < 4; i++)
            {
                bool isLeft = IsLeft(i);
                switch (this[i].Suffix)
                {
                    case SkillSuffix.IncreaseDamageX:
                    case SkillSuffix.IncreasePowerAndDamageX:
                        if (isLeft)
                            leftDamageAdder.InsertAddition(skills[i].X);
                        else
                            rightDamageAdder.InsertAddition(skills[i].X);
                        break;
                }
            }
        }
        public void ApplyIncreasePowerSuffixes(BatchAdder leftPowerAdder, BatchAdder rightPowerAdder)
        {
            for (int i = 0; i < 4; i++)
            {
                bool isLeft = IsLeft(i);
                switch (this[i].Suffix)
                {
                    case SkillSuffix.IncreasePowerAndDamageX:
                    case SkillSuffix.IncreasePowerX:
                        if (isLeft)
                            leftPowerAdder.InsertAddition(skills[i].X);
                        else
                            rightPowerAdder.InsertAddition(skills[i].X);
                        break;
                }
            }
        }
        public void ApplyBacklashVariantSuffixes(BatchSubtracter leftLifeSubstracter, BatchSubtracter rightLifeSubstracter, 
            BatchSubtracter leftPillzSubstracter, BatchSubtracter rightPillzSubstracter, 
            Poison poisonToLeft, Poison poisonToRight, 
            CanceledModifiers leftCanceledModifiers, CanceledModifiers rightCanceledModifiers)
        {
            for (int i = 0; i < 4; i++)
            {
                bool isLeft = IsLeft(i);
                // Backlash has some distinctions so we treat it in an individual loop
                if (skills[i].Prefix == SkillPrefix.Backlash)
                {
                    switch (skills[i].Suffix)
                    {
                        case SkillSuffix.DecreaseLifeXMinY:
                            if (isLeft)
                            {
                                if (!leftCanceledModifiers.Life())
                                    leftLifeSubstracter.InsertSubstraction(skills[i].X, skills[i].Y);
                            }
                            else
                            {
                                if (!rightCanceledModifiers.Life())
                                    rightLifeSubstracter.InsertSubstraction(skills[i].X, skills[i].Y);
                            }
                            break;
                        case SkillSuffix.DecreasePillzXMinY:
                            if (isLeft)
                            {
                                if (!leftCanceledModifiers.Pillz())
                                    leftPillzSubstracter.InsertSubstraction(skills[i].X, skills[i].Y);
                            }
                            else
                            {
                                if (!rightCanceledModifiers.Pillz())
                                    rightPillzSubstracter.InsertSubstraction(skills[i].X, skills[i].Y);
                            }
                            break;
                        case SkillSuffix.PoisonXMinY:
                            if (isLeft)
                            {
                                if (!leftCanceledModifiers.Life())
                                    poisonToLeft = new Poison(skills[i]);
                            }
                            else
                            {
                                if (!rightCanceledModifiers.Life())
                                    poisonToRight = new Poison(skills[i]);
                            }
                            break;
                    }
                }
            }
        }
        public void ApplyAfterDuelNormalSuffixes(BatchSubtracter leftLifeSubstracter, BatchSubtracter rightLifeSubstracter, 
            BatchSubtracter leftPillzSubstracter, BatchSubtracter rightPillzSubstracter, 
            BatchAdder leftLifeAdder, BatchAdder rightLifeAdder,
            BatchAdder leftPillzAdder, BatchAdder rightPillzAdder,
            Heal healToLeft, Heal healToRight,
            Poison poisonToLeft, Poison poisonToRight,
            int leftDamage, int rightDamage,
            int leftUsedPillz, int rightUsedPillz,
            bool leftUsedFury, bool rightUsedFury,
            CanceledModifiers leftCanceledModifiers, CanceledModifiers rightCanceledModifiers)
        {
            for (int i = 0; i < 4; i++)
            {
                bool isLeft = IsLeft(i);
                switch (skills[i].Suffix)
                {
                    case SkillSuffix.DecreaseLifeXMinY:
                        if (isLeft)
                            rightLifeSubstracter.InsertSubstraction(skills[i].X, skills[i].Y);
                        else
                            leftLifeSubstracter.InsertSubstraction(skills[i].X, skills[i].Y);
                        break;
                    case SkillSuffix.DecreasePillzXMinY:
                        if (isLeft)
                            rightPillzSubstracter.InsertSubstraction(skills[i].X, skills[i].Y);
                        else
                            leftPillzSubstracter.InsertSubstraction(skills[i].X, skills[i].Y);
                        break;
                    case SkillSuffix.HealXMaxY:
                        if (isLeft)
                        {
                            if (!leftCanceledModifiers.Life())
                                healToLeft = new Heal(skills[i]);
                        }
                        else
                        {
                            if (!rightCanceledModifiers.Life())
                                healToRight = new Heal(skills[i]);
                        }
                        break;
                    case SkillSuffix.IncreaseLifeX:
                        if (isLeft)
                            leftLifeAdder.InsertAddition(skills[i].X);
                        else
                            rightLifeAdder.InsertAddition(skills[i].X);
                        break;
                    case SkillSuffix.IncreaseLifeXMaxY:
                        if (isLeft)
                            leftLifeAdder.InsertAddition(skills[i].Y, skills[i].X);
                        else
                            rightLifeAdder.InsertAddition(skills[i].Y, skills[i].X);
                        break;
                    case SkillSuffix.IncreaseLifeXPerDamage:
                        if (isLeft)
                            leftLifeAdder.InsertAddition(skills[i].X * leftDamage);
                        else
                            rightLifeAdder.InsertAddition(skills[i].X * rightDamage);
                        break;
                    case SkillSuffix.IncreaseLifeXPerDamageMaxY:
                        if (isLeft)
                            leftLifeAdder.InsertAddition(skills[i].Y, skills[i].X * leftDamage);
                        else
                            rightLifeAdder.InsertAddition(skills[i].Y, skills[i].X * rightDamage);
                        break;
                    case SkillSuffix.IncreasePillzX:
                        if (isLeft)
                            leftPillzAdder.InsertAddition(skills[i].X);
                        else
                            rightPillzAdder.InsertAddition(skills[i].X);
                        break;
                    case SkillSuffix.IncreasePillzXMaxY:
                        if (isLeft)
                            leftPillzAdder.InsertAddition(skills[i].Y, skills[i].X);
                        else
                            rightPillzAdder.InsertAddition(skills[i].Y, skills[i].X);
                        break;
                    case SkillSuffix.IncreasePillzXPerDamage:
                        if (isLeft)
                            leftPillzAdder.InsertAddition(skills[i].X * leftDamage);
                        else
                            rightPillzAdder.InsertAddition(skills[i].X * rightDamage);
                        break;
                    case SkillSuffix.PoisonXMinY:
                        if (isLeft)
                        {
                            if (!leftCanceledModifiers.Life())
                                poisonToRight = new Poison(skills[i]);
                        }
                        else
                        {
                            if (!rightCanceledModifiers.Life())
                                poisonToLeft = new Poison(skills[i]);
                        }
                        break;
                    case SkillSuffix.RecoverXPillzOutOfY:
                        if (isLeft)
                            leftPillzAdder.InsertAddition(CalculateRecoveredPillz(leftUsedPillz, leftUsedFury, skills[i].X, skills[i].Y));
                        else
                            rightPillzAdder.InsertAddition(CalculateRecoveredPillz(rightUsedPillz, rightUsedFury, skills[i].X, skills[i].Y));
                        break;
                    case SkillSuffix.RegenXMaxY:
                        if (isLeft)
                        {
                            if (!leftCanceledModifiers.Life())
                            {
                                leftLifeAdder.InsertAddition(skills[i].Y, skills[i].X);
                                healToLeft = new Heal(skills[i]);
                            }
                        }
                        else
                        {
                            if (!rightCanceledModifiers.Life())
                            {
                                rightLifeAdder.InsertAddition(skills[i].Y, skills[i].X);
                                healToRight = new Heal(skills[i]);
                            }
                        }
                        break;
                    case SkillSuffix.ToxinXMinY:
                        if (isLeft)
                        {
                            if (!leftCanceledModifiers.Life())
                            {
                                rightLifeSubstracter.InsertSubstraction(skills[i].Y, skills[i].X);
                                poisonToRight = new Poison(skills[i]);
                            }
                        }
                        else
                        {
                            if (!rightCanceledModifiers.Life())
                            {
                                leftLifeSubstracter.InsertSubstraction(skills[i].Y, skills[i].X);
                                poisonToLeft = new Poison(skills[i]);
                            }
                        }
                        break;
                }
            }
        }

        /* Fixes */

        public void FixDJCorpsDoublePrefix(int roundCounter)
        {
            for (int i = 0; i < 4; i++)
                if (skills[i].Prefix == (SkillPrefix.GrowthAndDefeat))
                    skills[i] = new Skill(SkillPrefix.Defeat, SkillSuffix.DecreaseLifeXMinY, CalculateGrowth(skills[i], roundCounter), 1);
        }

        /* Utils */

        private static bool IsLeft(int skillIndex)
        {
            return (skillIndex == (int)SkillIndex.LA || skillIndex == (int)SkillIndex.LB);
        }
        private static int CalculateGrowth(Skill skill, int roundCounter)
        {
            return skill.X * (roundCounter + 1);
        }
        private static int CalculateSupportValue(PlayerStatus player, CardDrawed card)
        {
            return card.ability.X * player.Hand.GetSupportMultiplier(card.clan.id);
        }
        private static int CalculateRecoveredPillz(int usedPillz, bool usedFury, int x, int y)
        {
            throw new NotImplementedException(); // TODO
        }
        private static bool IsKillshotTriggered(int attackerAttackValue, int defenderAttackValue)
        {
            return attackerAttackValue >= 2 * defenderAttackValue;
        }
    }
}
