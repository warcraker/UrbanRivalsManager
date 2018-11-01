using System;

namespace UrbanRivalsCore.Model
{
    public class Skill
    {
        [Flags]
        private enum EmptySkill
        {
            None = 0,
            UnlockedAt2 = 0x01 | NoAbility,
            UnlockedAt3 = 0x02 | NoAbility,
            UnlockedAt4 = 0x03 | NoAbility,
            UnlockedAt5 = 0x04 | NoAbility,
            NoAbility = 0x08,
            NoBonus = 0x10,
        }

        public static readonly Skill UNLOCKED_AT_LEVEL_2 = prv_createEmptySkill(EmptySkill.UnlockedAt2);
        public static readonly Skill UNLOCKED_AT_LEVEL_3 = prv_createEmptySkill(EmptySkill.UnlockedAt3);
        public static readonly Skill UNLOCKED_AT_LEVEL_4 = prv_createEmptySkill(EmptySkill.UnlockedAt4);
        public static readonly Skill UNLOCKED_AT_LEVEL_5 = prv_createEmptySkill(EmptySkill.UnlockedAt5);
        public static readonly Skill NO_ABILITY = prv_createEmptySkill(EmptySkill.NoAbility);
        public static readonly Skill NO_BONUS = prv_createEmptySkill(EmptySkill.NoBonus);

        private static readonly int PRV_MAX_VALUE_PREFIX = Enum.GetValues(typeof(SkillPrefix)).Length - 1;

        public SkillLeader Leader { get; }
        public SkillPrefix Prefix { get; }
        public SkillSuffix Suffix { get; }
        public int X { get; }
        public int Y { get; }

        private EmptySkill EmptySkillFlags;

        public Skill(SkillPrefix prefix, SkillSuffix suffix, int x = 0, int y = 0)
        {
            if ((int)prefix < 0 || (int)prefix > Constants.EnumMaxAllowedValues.SkillPrefix)
                throw new ArgumentOutOfRangeException(nameof(prefix), prefix, "Must be a valid " + nameof(SkillPrefix));
            if ((int)suffix < 0 || (int)suffix > Constants.EnumMaxAllowedValues.SkillSuffix)
                throw new ArgumentOutOfRangeException(nameof(suffix), suffix, "Must be a valid " + nameof(SkillSuffix));
            if (suffix == SkillSuffix.None)
                throw new ArgumentException("Can't be None (0)", nameof(suffix));
            if (x < 0)
                throw new ArgumentOutOfRangeException(nameof(x), x, "Must be greater than or equal to 0");
            if (y < 0)
                throw new ArgumentOutOfRangeException(nameof(y), y, "Must be greater than or equal to 0");

            Prefix = prefix;
            Suffix = suffix;
            X = x;
            Y = y;
        }
        public Skill(SkillLeader leader)
        {
            if ((int)leader < 0 || (int)leader > Constants.EnumMaxAllowedValues.SkillLeader)
                throw new ArgumentOutOfRangeException(nameof(leader), leader, "Must be defined in SkillLeader");
            if (leader == SkillLeader.None)
                throw new ArgumentException("Can't be None (0)", nameof(leader));

            Leader = leader;
        }
        private Skill() { }
        private static Skill prv_createEmptySkill(EmptySkill flag)
        {
            Skill skill;

            skill = new Skill();
            skill.EmptySkillFlags = flag;

            return skill;
        }

        public Skill Copy()
        {
            if (this.EmptySkillFlags != EmptySkill.None)
            {
                switch (this.EmptySkillFlags)
                {
                    case EmptySkill.UnlockedAt2:
                        return UNLOCKED_AT_LEVEL_2;
                    case EmptySkill.UnlockedAt3:
                        return UNLOCKED_AT_LEVEL_3;
                    case EmptySkill.UnlockedAt4:
                        return UNLOCKED_AT_LEVEL_4;
                    case EmptySkill.UnlockedAt5:
                        return UNLOCKED_AT_LEVEL_5;
                    case EmptySkill.NoAbility:
                        return NO_ABILITY;
                    case EmptySkill.NoBonus:
                        return NO_BONUS;
                    default:
                        throw new Exception("Unexpected EmptySkillFlags: " + this.EmptySkillFlags.ToString()); // Sanity check
                }
            }
            if (this.Leader != SkillLeader.None)
                return new Skill(this.Leader);
            return new Skill(this.Prefix, this.Suffix, this.X, this.Y);
        }
        internal Skill CopyWithDifferentX(int newX)
        {
            if (this.Leader != SkillLeader.None)
                return new Skill(this.Leader);
            return new Skill(this.Prefix, this.Suffix, newX, this.Y);
        }
        public override string ToString()
        {
            if (this.EmptySkillFlags != EmptySkill.None)
            {
                if (this.EmptySkillFlags.HasFlag(EmptySkill.UnlockedAt2))
                    return String.Format(Properties.GameStrings.skill_not_unlocked, 2);
                if (this.EmptySkillFlags.HasFlag(EmptySkill.UnlockedAt3))
                    return String.Format(Properties.GameStrings.skill_not_unlocked, 3);
                if (this.EmptySkillFlags.HasFlag(EmptySkill.UnlockedAt4))
                    return String.Format(Properties.GameStrings.skill_not_unlocked, 4);
                if (this.EmptySkillFlags.HasFlag(EmptySkill.UnlockedAt5))
                    return String.Format(Properties.GameStrings.skill_not_unlocked, 5);
                if (this.EmptySkillFlags.HasFlag(EmptySkill.NoAbility))
                    return Properties.GameStrings.skill_no_ability;
                if (this.EmptySkillFlags.HasFlag(EmptySkill.NoBonus))
                    return Properties.GameStrings.skill_no_bonus;
            }

            if (this.Leader != SkillLeader.None)
            {
                switch (this.Leader)
                {
                    case SkillLeader.Ambre:
                        return Properties.GameStrings.skill_leader_ambre;
                    case SkillLeader.Ashigaru:
                        return Properties.GameStrings.skill_leader_ashigaru;
                    case SkillLeader.Bridget:
                        return Properties.GameStrings.skill_leader_bridget;
                    case SkillLeader.Eklore:
                        return Properties.GameStrings.skill_leader_eklore;
                    case SkillLeader.Eyrik:
                        return Properties.GameStrings.skill_leader_eyrik;
                    case SkillLeader.Hugo:
                        return Properties.GameStrings.skill_leader_hugo;
                    case SkillLeader.Melody:
                        return Properties.GameStrings.skill_leader_melody;
                    case SkillLeader.Morphun:
                        return Properties.GameStrings.skill_leader_morphun;
                    case SkillLeader.Solomon:
                        return Properties.GameStrings.skill_leader_solomon;
                    case SkillLeader.Timber:
                        return Properties.GameStrings.skill_leader_timber;
                    case SkillLeader.Vansaar:
                        return Properties.GameStrings.skill_leader_vansaar;
                    case SkillLeader.Vholt:
                        return Properties.GameStrings.skill_leader_vholt;
                } 
            } 

            // Only a few abilities can have the Backlash prefix, and those have different text
            if (this.Prefix == SkillPrefix.Backlash) // TODO check as flag
            {
                switch (this.Suffix)
                {
                    case SkillSuffix.DecreaseLifeXMinY:
                        return String.Format(Properties.GameStrings.skill_suffix_decrease_life_x_min_y_backlash, X, Y);
                    case SkillSuffix.DecreasePillzXMinY:
                        return String.Format(Properties.GameStrings.skill_suffix_decrease_pillz_x_min_y_backlash, X, Y);
                    case SkillSuffix.PoisonXMinY:
                        return String.Format(Properties.GameStrings.skill_suffix_poison_x_min_y_backlash, X, Y);
                }
            }

            string result = "";

            if (this.Prefix != SkillPrefix.None)
            {
                switch (this.Prefix)  // TODO check as flag
                {
                    // There is one case (DJ Korps ID=1260) where it has double prefix. Here we take care of it
                    case SkillPrefix.GrowthAndDefeat:
                        result = Properties.GameStrings.skill_prefix_defeat + Properties.GameStrings.skill_prefix_growth;
                        break;
                    case SkillPrefix.Backlash:
                        result = Properties.GameStrings.skill_prefix_backlash;
                        break;
                    case SkillPrefix.Brawl:
                        result = Properties.GameStrings.skill_prefix_brawl;
                        break;
                    case SkillPrefix.Confidence:
                        result = Properties.GameStrings.skill_prefix_confidence;
                        break;
                    case SkillPrefix.Courage:
                        result = Properties.GameStrings.skill_prefix_courage;
                        break;
                    case SkillPrefix.Defeat:
                        result = Properties.GameStrings.skill_prefix_defeat;
                        break;
                    case SkillPrefix.Degrowth:
                        result = Properties.GameStrings.skill_prefix_degrowth;
                        break;
                    case SkillPrefix.Equalizer:
                        result = Properties.GameStrings.skill_prefix_equalizer;
                        break;
                    case SkillPrefix.Growth:
                        result = Properties.GameStrings.skill_prefix_growth;
                        break;
                    case SkillPrefix.Killshot:
                        result = Properties.GameStrings.skill_prefix_killshot;
                        break;
                    case SkillPrefix.Reprisal:
                        result = Properties.GameStrings.skill_prefix_reprisal;
                        break;
                    case SkillPrefix.Revenge:
                        result = Properties.GameStrings.skill_prefix_revenge;
                        break;
                    case SkillPrefix.Stop:
                        result = Properties.GameStrings.skill_prefix_stop;
                        break;
                    case SkillPrefix.Support:
                        result = Properties.GameStrings.skill_prefix_support;
                        break;
                    case SkillPrefix.VictoryOrDefeat:
                        result = Properties.GameStrings.skill_prefix_victory_or_defeat;
                        break;
                } // End switch
            } // End if (prefix != none)

            switch (this.Suffix)
            {
                case SkillSuffix.CancelAttackModifier:
                    result += Properties.GameStrings.skill_suffix_cancel_attack_modifier;
                    break;
                case SkillSuffix.CancelDamageModifier:
                    result += Properties.GameStrings.skill_suffix_cancel_damage_modifier;
                    break;
                case SkillSuffix.CancelLeader:
                    result += Properties.GameStrings.skill_suffix_cancel_leader;
                    break;
                case SkillSuffix.CancelLifeModifier:
                    result += Properties.GameStrings.skill_suffix_cancel_life_modifier;
                    break;
                case SkillSuffix.CancelPillzAndLifeModifier:
                    result += Properties.GameStrings.skill_suffix_cancel_pillz_and_life_modifier;
                    break;
                case SkillSuffix.CancelPillzModifier:
                    result += Properties.GameStrings.skill_suffix_cancel_pillz_modifier;
                    break;
                case SkillSuffix.CancelPowerAndDamageModifier:
                    result += Properties.GameStrings.skill_suffix_cancel_power_and_damage_modifier;
                    break;
                case SkillSuffix.CancelPowerModifier:
                    result += Properties.GameStrings.skill_suffix_cancel_power_modifier;
                    break;
                case SkillSuffix.ConsumeXMinY:
                    result += String.Format(Properties.GameStrings.skill_suffix_consume_x_min_y, this.X, this.Y);
                    break;
                case SkillSuffix.CopyBonus:
                    result += Properties.GameStrings.skill_suffix_copy_bonus;
                    break;
                case SkillSuffix.CopyDamage:
                    result += Properties.GameStrings.skill_suffix_copy_damage;
                    break;
                case SkillSuffix.CopyPower:
                    result += Properties.GameStrings.skill_suffix_copy_power;
                    break;
                case SkillSuffix.CopyPowerAndDamage:
                    result += Properties.GameStrings.skill_suffix_copy_power_and_damage;
                    break;
                case SkillSuffix.CorrosionXMinY:
                    result = String.Format(Properties.GameStrings.skill_suffix_corrosion_x_min_y, this.X, this.Y);
                    break;
                case SkillSuffix.DecreaseAttackXMinY:
                    result += String.Format(Properties.GameStrings.skill_suffix_decrease_attack_x_min_y, this.X, this.Y);
                    break;
                case SkillSuffix.DecreaseAttackXPerRemainingLifeMinY:
                    result += String.Format(Properties.GameStrings.skill_suffix_decrease_attack_x_per_remaining_life_min_y, this.X, this.Y);
                    break;
                case SkillSuffix.DecreaseAttackXPerRemainingPillzMinY:
                    result += String.Format(Properties.GameStrings.skill_suffix_decrease_attack_x_per_remaining_pillz_min_y, this.X, this.Y);
                    break;
                case SkillSuffix.DecreaseDamageXMinY:
                    result += String.Format(Properties.GameStrings.skill_suffix_decrease_damage_x_min_y, this.X, this.Y);
                    break;
                case SkillSuffix.DecreaseLifeXMinY:
                    result += String.Format(Properties.GameStrings.skill_suffix_decrease_life_x_min_y, this.X, this.Y);
                    break;
                case SkillSuffix.DecreasePillzXMinY:
                    result += String.Format(Properties.GameStrings.skill_suffix_decrease_pillz_x_min_y, this.X, this.Y);
                    break;
                case SkillSuffix.DecreasePowerAndDamageXMinY:
                    result += String.Format(Properties.GameStrings.skill_suffix_decrease_power_and_damage_x_min_y, this.X, this.Y);
                    break;
                case SkillSuffix.DecreasePowerXMinY:
                    result += String.Format(Properties.GameStrings.skill_suffix_decrease_power_x_min_y, this.X, this.Y);
                    break;
                case SkillSuffix.DopeXMaxY:
                    result += String.Format(Properties.GameStrings.skill_suffix_dope_x_max_y, this.X, this.Y);
                    break;
                case SkillSuffix.ExchangeDamage:
                    result += String.Format(Properties.GameStrings.skill_suffix_exchange_damage);
                    break;
                case SkillSuffix.ExchangePower:
                    result += String.Format(Properties.GameStrings.skill_suffix_exchange_power);
                    break;
                case SkillSuffix.HealXMaxY:
                    result += String.Format(Properties.GameStrings.skill_suffix_heal_x_max_y, this.X, this.Y);
                    break;
                case SkillSuffix.IncreaseAttackX:
                    result += String.Format(Properties.GameStrings.skill_suffix_increase_attack_x, this.X);
                    break;
                case SkillSuffix.IncreaseAttackXPerRemainingLife:
                    result += String.Format(Properties.GameStrings.skill_suffix_increase_attack_x_per_remaining_life, this.X);
                    break;
                case SkillSuffix.IncreaseAttackXPerRemainingPillz:
                    result += String.Format(Properties.GameStrings.skill_suffix_increase_attack_x_per_remaining_pillz, this.X);
                    break;
                case SkillSuffix.IncreaseDamageX:
                    result += String.Format(Properties.GameStrings.skill_suffix_increase_damage_x, this.X);
                    break;
                case SkillSuffix.IncreaseLifeX:
                    result += String.Format(Properties.GameStrings.skill_suffix_increase_life_x, this.X);
                    break;
                case SkillSuffix.IncreaseLifeXMaxY:
                    result += String.Format(Properties.GameStrings.skill_suffix_increase_life_x_max_y, this.X, this.Y);
                    break;
                case SkillSuffix.IncreaseLifeXPerDamage:
                    result += String.Format(Properties.GameStrings.skill_suffix_increase_life_x_per_damage, this.X);
                    break;
                case SkillSuffix.IncreaseLifeXPerDamageMaxY:
                    result += String.Format(Properties.GameStrings.skill_suffix_increase_life_x_per_damage_max_y, this.X, this.Y);
                    break;
                case SkillSuffix.IncreasePillzAndLifeX:
                    result += String.Format(Properties.GameStrings.skill_suffix_increase_pillz_and_life_x, this.X);
                    break;
                case SkillSuffix.IncreasePillzX:
                    result += String.Format(Properties.GameStrings.skill_suffix_increase_pillz_x, this.X);
                    break;
                case SkillSuffix.IncreasePillzXMaxY:
                    result += String.Format(Properties.GameStrings.skill_suffix_increase_pillz_x_max_y, this.X, this.Y);
                    break;
                case SkillSuffix.IncreasePillzXPerDamage:
                    result += String.Format(Properties.GameStrings.skill_suffix_increase_pillz_x_per_damage, this.X);
                    break;
                case SkillSuffix.IncreasePowerAndDamageX:
                    result += String.Format(Properties.GameStrings.skill_suffix_increase_power_and_damage_x, this.X);
                    break;
                case SkillSuffix.IncreasePowerX:
                    result += String.Format(Properties.GameStrings.skill_suffix_increase_power_x, this.X);
                    break;
                case SkillSuffix.InfectionXMinY:
                    result += String.Format(Properties.GameStrings.skill_suffix_infection_x_min_y, this.X, this.Y);
                    break;
                case SkillSuffix.PoisonXMinY:
                    result += String.Format(Properties.GameStrings.skill_suffix_poison_x_min_y, this.X, this.Y);
                    break;
                case SkillSuffix.ProtectAbility:
                    result += Properties.GameStrings.skill_suffix_protect_ability;
                    break;
                case SkillSuffix.ProtectAttack:
                    result += Properties.GameStrings.skill_suffix_protect_attack;
                    break;
                case SkillSuffix.ProtectBonus:
                    result += Properties.GameStrings.skill_suffix_protect_bonus;
                    break;
                case SkillSuffix.ProtectDamage:
                    result += Properties.GameStrings.skill_suffix_protect_damage;
                    break;
                case SkillSuffix.ProtectPower:
                    result += Properties.GameStrings.skill_suffix_protect_power;
                    break;
                case SkillSuffix.ProtectPowerAndDamage:
                    result += Properties.GameStrings.skill_suffix_protect_power_and_damage;
                    break;
                case SkillSuffix.ReanimateX:
                    result += String.Format(Properties.GameStrings.skill_suffix_reanimate_x, this.X);
                    break;
                case SkillSuffix.RebirthXMaxY:
                    result += String.Format(Properties.GameStrings.skill_suffix_rebirth_x_max_y, this.X, this.Y);
                    break;
                case SkillSuffix.RecoverXPillzOutOfY:
                    result += String.Format(Properties.GameStrings.skill_suffix_recover_x_pillz_out_of_y, this.X, this.Y);
                    break;
                case SkillSuffix.RegenXMaxY:
                    result += String.Format(Properties.GameStrings.skill_suffix_regen_x_max_y, this.X, this.Y);
                    break;
                case SkillSuffix.StopAbility:
                    result += Properties.GameStrings.skill_suffix_stop_ability;
                    break;
                case SkillSuffix.StopBonus:
                    result += Properties.GameStrings.skill_suffix_stop_bonus;
                    break;
                case SkillSuffix.ToxinXMinY:
                    result += String.Format(Properties.GameStrings.skill_suffix_toxin_x_min_y, this.X, this.Y);
                    break;
            } // End Switch Suffix

            return result;
        }
    }
}