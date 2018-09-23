using System;

namespace UrbanRivalsCore.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class Skill
    {
        private EmptySkillCases EmptySkillFlags;

        /// <summary>
        /// The skill will be unlocked when the card gets to level 2.
        /// </summary>
        public static readonly Skill UnlockedAtLevel2 = new Skill(EmptySkillCases.UnlockedAt2);
        /// <summary>
        /// The skill will be unlocked when the card gets to level 3.
        /// </summary>
        public static readonly Skill UnlockedAtLevel3 = new Skill(EmptySkillCases.UnlockedAt3);
        /// <summary>
        /// The skill will be unlocked when the card gets to level 4.
        /// </summary>
        public static readonly Skill UnlockedAtLevel4 = new Skill(EmptySkillCases.UnlockedAt4);
        /// <summary>
        /// The skill will be unlocked when the card gets to level 5.
        /// </summary>
        public static readonly Skill UnlockedAtLevel5 = new Skill(EmptySkillCases.UnlockedAt5);
        /// <summary>
        /// The card doesn't have ability.
        /// </summary>
        public static readonly Skill NoAbility = new Skill(EmptySkillCases.NoAbility);
        /// <summary>
        /// The card doesn't have bonus.
        /// </summary>
        public static readonly Skill NoBonus = new Skill(EmptySkillCases.NoBonus);
        
        /// <summary>
        /// Gets the leader skill identifier. Gets None if it isn't a leader skill.
        /// </summary>
        public SkillLeader Leader { get; }
        /// <summary>
        /// Gets the prefix.
        /// </summary>
        public SkillPrefix Prefix { get; }
        /// <summary>
        /// Gets the suffix.
        /// </summary>
        public SkillSuffix Suffix { get; }
        /// <summary>
        /// Gets the primary modifier of the skill if applicable.
        /// <example>"Power + 3" and "Poison 3, Min 4" have both X = 3</example>
        /// </summary>
        public int X { get; }
        /// <summary>
        /// Gets the secondary modifier of the skill if applicable.
        /// <example>"Poison 3, Min 4" have Y = 4</example>
        /// </summary>
        public int Y { get; }

        // Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Skill"/> class.
        /// </summary>
        /// <param name="prefix">Prefix.</param>
        /// <param name="suffix">Suffix.</param>
        /// <param name="x">Primary modifier.</param>
        /// <param name="y">Secondary modifier.</param>
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

        private Skill(EmptySkillCases flags)
        {
            EmptySkillFlags = flags;
        }

        // Functions
        /// <summary>
        /// Returns a new instance as a copy of this instance.
        /// </summary>
        /// <returns></returns>
        public Skill Copy()
        {
            if (this.EmptySkillFlags != EmptySkillCases.None)
            {
                switch (this.EmptySkillFlags)
                {
                    case EmptySkillCases.UnlockedAt2:
                        return UnlockedAtLevel2;
                    case EmptySkillCases.UnlockedAt3:
                        return UnlockedAtLevel3;
                    case EmptySkillCases.UnlockedAt4:
                        return UnlockedAtLevel4;
                    case EmptySkillCases.UnlockedAt5:
                        return UnlockedAtLevel5;
                    case EmptySkillCases.NoAbility:
                        return NoAbility;
                    case EmptySkillCases.NoBonus:
                        return NoBonus;
                    default:
                        throw new Exception("Unexpected EmptySkillFlags: " + this.EmptySkillFlags.ToString()); // Sanity check
                }
            }
            if (this.Leader != SkillLeader.None)
                return new Skill(this.Leader);
            return new Skill(this.Prefix, this.Suffix, this.X, this.Y);
        }

        /// <summary>
        /// Returns a copy of the <see cref="Skill"/> with a different <see cref="X"/> value.
        /// </summary>
        /// <param name="newX">New value for X.</param>
        /// <returns></returns>
        internal Skill CopyWithDifferentX(int newX)
        {
            if (this.Leader != SkillLeader.None)
                return new Skill(this.Leader);
            return new Skill(this.Prefix, this.Suffix, newX, this.Y);
        }

        /// <summary>
        /// Returns the string representation of the skill.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (EmptySkillFlags != EmptySkillCases.None)
            {
                if (EmptySkillFlags.HasFlag(EmptySkillCases.UnlockedAt2))
                    return String.Format(Properties.GameStrings.skill_not_unlocked, 2);
                if (EmptySkillFlags.HasFlag(EmptySkillCases.UnlockedAt3))
                    return String.Format(Properties.GameStrings.skill_not_unlocked, 3);
                if (EmptySkillFlags.HasFlag(EmptySkillCases.UnlockedAt4))
                    return String.Format(Properties.GameStrings.skill_not_unlocked, 4);
                if (EmptySkillFlags.HasFlag(EmptySkillCases.UnlockedAt5))
                    return String.Format(Properties.GameStrings.skill_not_unlocked, 5);
                if (EmptySkillFlags.HasFlag(EmptySkillCases.NoAbility))
                    return Properties.GameStrings.skill_no_ability;
                if (EmptySkillFlags.HasFlag(EmptySkillCases.NoBonus))
                    return Properties.GameStrings.skill_no_bonus;
            }

            if (Leader != SkillLeader.None)
            {
                switch (Leader)
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
            if (Prefix == SkillPrefix.Backlash)
            {
                switch (Suffix)
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

            if (Prefix != SkillPrefix.None)
            {
                switch (Prefix)
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

            switch (Suffix)
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
                case SkillSuffix.CancelPillzModifier:
                    result += Properties.GameStrings.skill_suffix_cancel_pillz_modifier;
                    break;
                case SkillSuffix.CancelPowerModifier:
                    result += Properties.GameStrings.skill_suffix_cancel_power_modifier;
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
                case SkillSuffix.DecreaseAttackXMinY:
                    result += String.Format(Properties.GameStrings.skill_suffix_decrease_attack_x_min_y, X, Y);
                    break;
                case SkillSuffix.DecreaseAttackXPerRemainingLifeMinY:
                    result += String.Format(Properties.GameStrings.skill_suffix_decrease_attack_x_per_remaining_life_min_y, X, Y);
                    break;
                case SkillSuffix.DecreaseDamageXMinY:
                    result += String.Format(Properties.GameStrings.skill_suffix_decrease_damage_x_min_y, X, Y);
                    break;
                case SkillSuffix.DecreaseLifeXMinY:
                    result += String.Format(Properties.GameStrings.skill_suffix_decrease_life_x_min_y, X, Y);
                    break;
                case SkillSuffix.DecreasePillzXMinY:
                    result += String.Format(Properties.GameStrings.skill_suffix_decrease_pillz_x_min_y, X, Y);
                    break;
                case SkillSuffix.DecreasePowerAndDamageXMinY:
                    result += String.Format(Properties.GameStrings.skill_suffix_decrease_power_and_damage_x_min_y, X, Y);
                    break;
                case SkillSuffix.DecreasePowerXMinY:
                    result += String.Format(Properties.GameStrings.skill_suffix_decrease_power_x_min_y, X, Y);
                    break;
                case SkillSuffix.HealXMaxY:
                    result += String.Format(Properties.GameStrings.skill_suffix_heal_x_max_y, X, Y);
                    break;
                case SkillSuffix.IncreaseAttackX:
                    result += String.Format(Properties.GameStrings.skill_suffix_increase_attack_x, X);
                    break;
                case SkillSuffix.IncreaseAttackXPerRemainingLife:
                    result += String.Format(Properties.GameStrings.skill_suffix_increase_attack_x_per_remaining_life, X);
                    break;
                case SkillSuffix.IncreaseAttackXPerRemainingPillz:
                    result += String.Format(Properties.GameStrings.skill_suffix_increase_attack_x_per_remaining_pillz, X);
                    break;
                case SkillSuffix.IncreaseDamageX:
                    result += String.Format(Properties.GameStrings.skill_suffix_increase_damage_x, X);
                    break;
                case SkillSuffix.IncreaseLifeX:
                    result += String.Format(Properties.GameStrings.skill_suffix_increase_life_x, X);
                    break;
                case SkillSuffix.IncreaseLifeXMaxY:
                    result += String.Format(Properties.GameStrings.skill_suffix_increase_life_x_max_y, X, Y);
                    break;
                case SkillSuffix.IncreaseLifeXPerDamage:
                    result += String.Format(Properties.GameStrings.skill_suffix_increase_life_x_per_damage, X);
                    break;
                case SkillSuffix.IncreaseLifeXPerDamageMaxY:
                    result += String.Format(Properties.GameStrings.skill_suffix_increase_life_x_per_damage_max_y, X, Y);
                    break;
                case SkillSuffix.IncreasePillzX:
                    result += String.Format(Properties.GameStrings.skill_suffix_increase_pillz_x, X);
                    break;
                case SkillSuffix.IncreasePillzXMaxY:
                    result += String.Format(Properties.GameStrings.skill_suffix_increase_pillz_x_max_y, X, Y);
                    break;
                case SkillSuffix.IncreasePillzXPerDamage:
                    result += String.Format(Properties.GameStrings.skill_suffix_increase_pillz_x_per_damage, X);
                    break;
                case SkillSuffix.IncreasePowerAndDamageX:
                    result += String.Format(Properties.GameStrings.skill_suffix_increase_power_and_damage_x, X);
                    break;
                case SkillSuffix.IncreasePowerX:
                    result += String.Format(Properties.GameStrings.skill_suffix_increase_power_x, X);
                    break;
                case SkillSuffix.PoisonXMinY:
                    result += String.Format(Properties.GameStrings.skill_suffix_poison_x_min_y, X, Y);
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
                case SkillSuffix.RecoverXPillzOutOfY:
                    result += String.Format(Properties.GameStrings.skill_suffix_recover_x_pillz_out_of_y, X, Y);
                    break;
                case SkillSuffix.RegenXMaxY:
                    result += String.Format(Properties.GameStrings.skill_suffix_regen_x_max_y, X, Y);
                    break;
                case SkillSuffix.StopAbility:
                    result += Properties.GameStrings.skill_suffix_stop_ability;
                    break;
                case SkillSuffix.StopBonus:
                    result += Properties.GameStrings.skill_suffix_stop_bonus;
                    break;
                case SkillSuffix.ToxinXMinY:
                    result += String.Format(Properties.GameStrings.skill_suffix_toxin_x_min_y, X, Y);
                    break;
            } // End Switch Suffix

            return result;
        }
    }
}