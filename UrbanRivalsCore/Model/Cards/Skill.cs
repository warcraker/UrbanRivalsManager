using System;
using UrbanRivalsUtils;

namespace UrbanRivalsCore.Model
{
    public class Skill
    {
        private enum EmptySkill
        {
            None = 0,
            UnlockedAt2,
            UnlockedAt3,
            UnlockedAt4,
            UnlockedAt5,
            NoAbility,
            NoBonus,
        }

        public static readonly Skill UNLOCKED_AT_LEVEL_2 = prv_createEmptySkill(EmptySkill.UnlockedAt2);
        public static readonly Skill UNLOCKED_AT_LEVEL_3 = prv_createEmptySkill(EmptySkill.UnlockedAt3);
        public static readonly Skill UNLOCKED_AT_LEVEL_4 = prv_createEmptySkill(EmptySkill.UnlockedAt4);
        public static readonly Skill UNLOCKED_AT_LEVEL_5 = prv_createEmptySkill(EmptySkill.UnlockedAt5);
        public static readonly Skill NO_ABILITY = prv_createEmptySkill(EmptySkill.NoAbility);
        public static readonly Skill NO_BONUS = prv_createEmptySkill(EmptySkill.NoBonus);

        private static readonly int PRV_MAX_VALUE_PREFIX = Enum.GetValues(typeof(SkillPrefix)).Length - 1;

        public SkillLeader leader;
        public SkillPrefix prefix;
        public SkillSuffix suffix;
        public int x;
        public int y;

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

            this.prefix = prefix;
            this.suffix = suffix;
            this.x = x;
            this.y = y;
        }
        public Skill(SkillLeader leader)
        {
            if ((int)leader < 0 || (int)leader > Constants.EnumMaxAllowedValues.SkillLeader)
                throw new ArgumentOutOfRangeException(nameof(leader), leader, "Must be defined in SkillLeader");
            if (leader == SkillLeader.None)
                throw new ArgumentException("Can't be None (0)", nameof(leader));

            this.leader = leader;
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
            if (this.leader != SkillLeader.None)
                return new Skill(this.leader);
            return new Skill(this.prefix, this.suffix, this.x, this.y);
        }
        internal Skill CopyWithDifferentX(int newX)
        {
            if (this.leader != SkillLeader.None)
                return new Skill(this.leader);
            return new Skill(this.prefix, this.suffix, newX, this.y);
        }
        public override string ToString() // TODO
        {
            string result;

            switch (this.EmptySkillFlags)
            {
                case EmptySkill.UnlockedAt2:
                    result = String.Format(Properties.GameStrings.skill_not_unlocked, 2);
                    break;
                case EmptySkill.UnlockedAt3:
                    result = String.Format(Properties.GameStrings.skill_not_unlocked, 3);
                    break;
                case EmptySkill.UnlockedAt4:
                    result = String.Format(Properties.GameStrings.skill_not_unlocked, 4);
                    break;
                case EmptySkill.UnlockedAt5:
                    result = String.Format(Properties.GameStrings.skill_not_unlocked, 5);
                    break;
                case EmptySkill.NoAbility:
                    result = Properties.GameStrings.skill_no_ability;
                    break;
                case EmptySkill.NoBonus:
                    result = Properties.GameStrings.skill_no_bonus;
                    break;
                case EmptySkill.None:
                    //switch (this.leader)
                    //{
                    //    case SkillLeader.Ambre:
                    //        result = Properties.GameStrings.skill_leader_ambre;
                    //        break;
                    //    case SkillLeader.Ashigaru:
                    //        result = Properties.GameStrings.skill_leader_ashigaru;
                    //        break;
                    //    case SkillLeader.Bridget:
                    //        result = Properties.GameStrings.skill_leader_bridget;
                    //        break;
                    //    case SkillLeader.Eklore:
                    //        result = Properties.GameStrings.skill_leader_eklore;
                    //        break;
                    //    case SkillLeader.Eyrik:
                    //        result = Properties.GameStrings.skill_leader_eyrik;
                    //        break;
                    //    case SkillLeader.Hugo:
                    //        result = Properties.GameStrings.skill_leader_hugo;
                    //        break;
                    //    case SkillLeader.Melody:
                    //        result = Properties.GameStrings.skill_leader_melody;
                    //        break;
                    //    case SkillLeader.Morphun:
                    //        result = Properties.GameStrings.skill_leader_morphun;
                    //        break;
                    //    case SkillLeader.Solomon:
                    //        result = Properties.GameStrings.skill_leader_solomon;
                    //        break;
                    //    case SkillLeader.Timber:
                    //        result = Properties.GameStrings.skill_leader_timber;
                    //        break;
                    //    case SkillLeader.Vansaar:
                    //        result = Properties.GameStrings.skill_leader_vansaar;
                    //        break;
                    //    case SkillLeader.Vholt:
                    //        result = Properties.GameStrings.skill_leader_vholt;
                    //        break;
                    //    case SkillLeader.None:
                    //    {
                    //        string prefix;
                    //        string suffix;

                    //        switch (this.prefix)
                    //        {
                    //            case SkillPrefix.Backlash:
                    //                prefix = Properties.GameStrings.skill_prefix_backlash;
                    //                switch (this.suffix)
                    //                {
                    //                    case SkillSuffix.DecreaseLifeXMinY:
                    //                        suffix = String.Format(Properties.GameStrings.skill_suffix_decrease_life_x_min_y_backlash, this.x, this.y);
                    //                        break;
                    //                    case SkillSuffix.DecreasePillzXMinY:
                    //                        suffix = String.Format(Properties.GameStrings.skill_suffix_decrease_pillz_x_min_y_backlash, this.x, this.y);
                    //                        break;
                    //                    case SkillSuffix.PoisonXMinY:
                    //                        suffix = String.Format(Properties.GameStrings.skill_suffix_poison_x_min_y_backlash, this.x, this.y);
                    //                        break;
                    //                    default:
                    //                        suffix = "";
                    //                        Asserts.fail($"Invalid {nameof(SkillSuffix)} ({this.suffix.ToString()}) when {nameof(SkillPrefix)} is {nameof(SkillPrefix.Backlash)}");
                    //                        break;
                    //                }
                    //                break;
                    //            case SkillPrefix.GrowthAndDefeat: // TODO Remove
                    //                prefix = Properties.GameStrings.skill_prefix_defeat + Properties.GameStrings.skill_prefix_growth;
                    //                break;
                    //            case SkillPrefix.Brawl:
                    //                prefix = Properties.GameStrings.skill_prefix_brawl;
                    //                break;
                    //            case SkillPrefix.Confidence:
                    //                prefix = Properties.GameStrings.skill_prefix_confidence;
                    //                break;
                    //            case SkillPrefix.Courage:
                    //                prefix = Properties.GameStrings.skill_prefix_courage;
                    //                break;
                    //            case SkillPrefix.Defeat:
                    //                prefix = Properties.GameStrings.skill_prefix_defeat;
                    //                break;
                    //            case SkillPrefix.Degrowth:
                    //                prefix = Properties.GameStrings.skill_prefix_degrowth;
                    //                break;
                    //            case SkillPrefix.Equalizer:
                    //                prefix = Properties.GameStrings.skill_prefix_equalizer;
                    //                break;
                    //            case SkillPrefix.Growth:
                    //                prefix = Properties.GameStrings.skill_prefix_growth;
                    //                break;
                    //            case SkillPrefix.Killshot:
                    //                prefix = Properties.GameStrings.skill_prefix_killshot;
                    //                break;
                    //            case SkillPrefix.Reprisal:
                    //                prefix = Properties.GameStrings.skill_prefix_reprisal;
                    //                break;
                    //            case SkillPrefix.Revenge:
                    //                prefix = Properties.GameStrings.skill_prefix_revenge;
                    //                break;
                    //            case SkillPrefix.Stop:
                    //                prefix = Properties.GameStrings.skill_prefix_stop;
                    //                break;
                    //            case SkillPrefix.Support:
                    //                prefix = Properties.GameStrings.skill_prefix_support;
                    //                break;
                    //            case SkillPrefix.VictoryOrDefeat:
                    //                prefix = Properties.GameStrings.skill_prefix_victory_or_defeat;
                    //                break;
                    //            case SkillPrefix.None:
                    //                prefix = "";
                    //                break;
                    //            default:
                    //                prefix = "";
                    //                Asserts.fail($"Invalid {nameof(SkillPrefix)}");
                    //                break;
                    //        }
                    //    }
                    //    break;
                    //    default:
                    //        result = "";
                    //        Asserts.fail($"Invalid {nameof(SkillLeader)}");
                    //        break;
                    //}
                    result = "";
                    break;
                default:
                    result = "";
                    Asserts.fail($"Invalid {nameof(EmptySkill)}");
                    break;
            }

            ///////
            //switch (this.suffix)
            //{
            //    case SkillSuffix.ConsumeXMinY:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_consume_x_min_y, this.x, this.y);
            //        break;
            //    case SkillSuffix.CorrosionXMinY:
            //        result = String.Format(Properties.GameStrings.skill_suffix_corrosion_x_min_y, this.x, this.y);
            //        break;
            //    case SkillSuffix.DecreaseAttackXMinY:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_decrease_attack_x_min_y, this.x, this.y);
            //        break;
            //    case SkillSuffix.DecreaseAttackXPerRemainingLifeMinY:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_decrease_attack_x_per_remaining_life_min_y, this.x, this.y);
            //        break;
            //    case SkillSuffix.DecreaseAttackXPerRemainingPillzMinY:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_decrease_attack_x_per_remaining_pillz_min_y, this.x, this.y);
            //        break;
            //    case SkillSuffix.DecreaseDamageXMinY:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_decrease_damage_x_min_y, this.x, this.y);
            //        break;
            //    case SkillSuffix.DecreaseLifeXMinY:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_decrease_life_x_min_y, this.x, this.y);
            //        break;
            //    case SkillSuffix.DecreasePillzXMinY:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_decrease_pillz_x_min_y, this.x, this.y);
            //        break;
            //    case SkillSuffix.DecreasePowerAndDamageXMinY:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_decrease_power_and_damage_x_min_y, this.x, this.y);
            //        break;
            //    case SkillSuffix.DecreasePowerXMinY:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_decrease_power_x_min_y, this.x, this.y);
            //        break;
            //    case SkillSuffix.DopeXMaxY:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_dope_x_max_y, this.x, this.y);
            //        break;
            //    case SkillSuffix.HealXMaxY:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_heal_x_max_y, this.x, this.y);
            //        break;
            //    case SkillSuffix.IncreaseAttackX:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_increase_attack_x, this.x);
            //        break;
            //    case SkillSuffix.IncreaseAttackXPerRemainingLife:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_increase_attack_x_per_remaining_life, this.x);
            //        break;
            //    case SkillSuffix.IncreaseAttackXPerRemainingPillz:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_increase_attack_x_per_remaining_pillz, this.x);
            //        break;
            //    case SkillSuffix.IncreaseDamageX:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_increase_damage_x, this.x);
            //        break;
            //    case SkillSuffix.IncreaseLifeX:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_increase_life_x, this.x);
            //        break;
            //    case SkillSuffix.IncreaseLifeXMaxY:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_increase_life_x_max_y, this.x, this.y);
            //        break;
            //    case SkillSuffix.IncreaseLifeXPerDamage:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_increase_life_x_per_damage, this.x);
            //        break;
            //    case SkillSuffix.IncreaseLifeXPerDamageMaxY:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_increase_life_x_per_damage_max_y, this.x, this.y);
            //        break;
            //    case SkillSuffix.IncreasePillzAndLifeX:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_increase_pillz_and_life_x, this.x);
            //        break;
            //    case SkillSuffix.IncreasePillzX:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_increase_pillz_x, this.x);
            //        break;
            //    case SkillSuffix.IncreasePillzXMaxY:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_increase_pillz_x_max_y, this.x, this.y);
            //        break;
            //    case SkillSuffix.IncreasePillzXPerDamage:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_increase_pillz_x_per_damage, this.x);
            //        break;
            //    case SkillSuffix.IncreasePowerAndDamageX:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_increase_power_and_damage_x, this.x);
            //        break;
            //    case SkillSuffix.IncreasePowerX:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_increase_power_x, this.x);
            //        break;
            //    case SkillSuffix.InfectionXMinY:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_infection_x_min_y, this.x, this.y);
            //        break;
            //    case SkillSuffix.PoisonXMinY:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_poison_x_min_y, this.x, this.y);
            //        break;
            //    case SkillSuffix.ReanimateX:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_reanimate_x, this.x);
            //        break;
            //    case SkillSuffix.RebirthXMaxY:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_rebirth_x_max_y, this.x, this.y);
            //        break;
            //    case SkillSuffix.RecoverXPillzOutOfY:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_recover_x_pillz_out_of_y, this.x, this.y);
            //        break;
            //    case SkillSuffix.RegenXMaxY:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_regen_x_max_y, this.x, this.y);
            //        break;
            //    case SkillSuffix.ToxinXMinY:
            //        text =  String.Format(Properties.GameStrings.skill_suffix_toxin_x_min_y, this.x, this.y);
            //        break;
            //} 

            /////////////

            return result;
        }

        private static string prv_getSuffixWithoutNumberTextRepresentation(SkillSuffix suffix)
        {
            string text;

            switch (suffix)
            {
                case SkillSuffix.CancelAttackModifier:
                    text = Properties.GameStrings.skill_suffix_cancel_attack_modifier;
                    break;
                case SkillSuffix.CancelDamageModifier:
                    text = Properties.GameStrings.skill_suffix_cancel_damage_modifier;
                    break;
                case SkillSuffix.CancelLeader:
                    text = Properties.GameStrings.skill_suffix_cancel_leader;
                    break;
                case SkillSuffix.CancelLifeModifier:
                    text = Properties.GameStrings.skill_suffix_cancel_life_modifier;
                    break;
                case SkillSuffix.CancelPillzAndLifeModifier:
                    text = Properties.GameStrings.skill_suffix_cancel_pillz_and_life_modifier;
                    break;
                case SkillSuffix.CancelPillzModifier:
                    text = Properties.GameStrings.skill_suffix_cancel_pillz_modifier;
                    break;
                case SkillSuffix.CancelPowerAndDamageModifier:
                    text = Properties.GameStrings.skill_suffix_cancel_power_and_damage_modifier;
                    break;
                case SkillSuffix.CancelPowerModifier:
                    text = Properties.GameStrings.skill_suffix_cancel_power_modifier;
                    break;
                case SkillSuffix.CopyBonus:
                    text = Properties.GameStrings.skill_suffix_copy_bonus;
                    break;
                case SkillSuffix.CopyDamage:
                    text = Properties.GameStrings.skill_suffix_copy_damage;
                    break;
                case SkillSuffix.CopyPower:
                    text = Properties.GameStrings.skill_suffix_copy_power;
                    break;
                case SkillSuffix.CopyPowerAndDamage:
                    text = Properties.GameStrings.skill_suffix_copy_power_and_damage;
                    break;
                case SkillSuffix.ExchangeDamage:
                    text = String.Format(Properties.GameStrings.skill_suffix_exchange_damage);
                    break;
                case SkillSuffix.ExchangePower:
                    text = String.Format(Properties.GameStrings.skill_suffix_exchange_power);
                    break;
                case SkillSuffix.ProtectAbility:
                    text = Properties.GameStrings.skill_suffix_protect_ability;
                    break;
                case SkillSuffix.ProtectAttack:
                    text = Properties.GameStrings.skill_suffix_protect_attack;
                    break;
                case SkillSuffix.ProtectBonus:
                    text = Properties.GameStrings.skill_suffix_protect_bonus;
                    break;
                case SkillSuffix.ProtectDamage:
                    text = Properties.GameStrings.skill_suffix_protect_damage;
                    break;
                case SkillSuffix.ProtectPower:
                    text = Properties.GameStrings.skill_suffix_protect_power;
                    break;
                case SkillSuffix.ProtectPowerAndDamage:
                    text = Properties.GameStrings.skill_suffix_protect_power_and_damage;
                    break;
                case SkillSuffix.StopAbility:
                    text = Properties.GameStrings.skill_suffix_stop_ability;
                    break;
                case SkillSuffix.StopBonus:
                    text = Properties.GameStrings.skill_suffix_stop_bonus;
                    break;
                default:
                    text = "";
                    Asserts.fail($"Invalid {nameof(SkillSuffix)} ({suffix.ToString()}) for {nameof(prv_getSuffixWithoutNumberTextRepresentation)}");
                    break;
            }

            return text;
        }
        //private static string prv_getSuffixWithXTextRepresentation(SkillSuffix suffix, int x)
        //{
        //    string text;
        //    string format;

        //    text = String.Format(

        //    return text;
        //}
    }
}
