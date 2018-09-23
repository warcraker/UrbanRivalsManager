using System;

namespace UrbanRivalsCore.Model
{
    /* IMPORTANT!! If any of the next items is modified, the whole database must be rebuilt:
     * CardRarity
     * ClanId
     * SkillPrefix
     * SkillSuffix
     *
     * If SkillLeader is modified, all leader cards info on the database must be rebuilt.
     */

    /// <summary>
    /// Represents the rarity of a card.
    /// </summary>
    public enum CardRarity
    {
        Common = 0,
        Uncommon,
        Rare,
        Collector,
        Legendary,
        Rebirth,
    }

    /// <summary>
    /// Represents if a combat uses random factor to calculate winner of the round.
    /// </summary>
    public enum RandomFactor
    {
        /// <summary>
        /// Doesn't use random factor.
        /// </summary>
        NonRandom,
        /// <summary>
        /// Uses random factor.
        /// </summary>
        Random,
    }

    /// <summary>
    /// Represents the winner of the combat.
    /// </summary>
    public enum CombatWinner
    {
        None = 0,
        Draw,
        Left,
        Right,
    }

    /// <summary>
    /// Represents one player or the other.
    /// </summary>
    public enum PlayerSide
    {
        /// <summary>
        /// No player is represented.
        /// </summary>
        None = 0,
        /// <summary>
        /// Left player = Bottom player = You
        /// </summary>
        Left,
        /// <summary>
        /// Right player = Top player = Enemy
        /// </summary>
        Right,
    }

    /// <summary>
    /// Represents a leader skill.
    /// </summary>
    public enum SkillLeader
    {
        None = 0,
        Ambre,
        Ashigaru,
        Bridget,
        Eklore,
        Eyrik,
        Hugo,
        Melody,
        Morphun,
        Solomon,
        Timber,
        Vansaar,
        Vholt,
    }

    /// <summary>
    /// Represents a skill prefix.
    /// </summary>
    public enum SkillPrefix
    {
        None= 0,
        /// <summary>
        /// Special case. Growth + Defeat prefixes. There is one card (DJ Korps Id=1260) that has 2 prefixes, so this one must exist for that card.
        /// </summary>
        GrowthAndDefeat,

        Backlash,
        Confidence,
        Courage,
        Defeat,
        Equalizer,
        Growth,
        Killshot,
        Reprisal,
        Revenge,
        Stop,
        Support,
        VictoryOrDefeat,
    }

    /// <summary>
    /// Represents a skill suffix.
    /// </summary>
    public enum SkillSuffix
    {
        None = 0,
        CancelAttackModifier,
        CancelDamageModifier,
        CancelLeader,
        CancelLifeModifier,
        CancelPillzModifier,
        CancelPowerModifier,
        CopyBonus,
        CopyDamage,
        CopyPower,
        CopyPowerAndDamage,
        DecreaseAttackXMinY,
        DecreaseAttackXPerRemainingLifeMinY,
        DecreaseDamageXMinY,
        DecreaseLifeXMinY,
        DecreasePillzXMinY,
        DecreasePowerAndDamageXMinY,
        DecreasePowerXMinY,
        HealXMaxY,
        IncreaseAttackX,
        IncreaseAttackXPerRemainingLife,
        IncreaseAttackXPerRemainingPillz,
        IncreaseDamageX,
        IncreaseLifeX,
        IncreaseLifeXMaxY,
        IncreaseLifeXPerDamage,
        IncreaseLifeXPerDamageMaxY,
        IncreasePillzX,
        IncreasePillzXMaxY,
        IncreasePillzXPerDamage,
        IncreasePowerAndDamageX,
        IncreasePowerX,
        PoisonXMinY,
        ProtectAbility,
        ProtectAttack,
        ProtectBonus,
        ProtectDamage,
        ProtectPower,
        ProtectPowerAndDamage,
        RecoverXPillzOutOfY,
        RegenXMaxY,
        StopAbility,
        StopBonus,
        ToxinXMinY,
    }

    /// <summary>
    /// Represents starting conditions in a survivor game.
    /// </summary>
    public enum SurvivorStage
    {
        Stage1Pillz12Lives12,
        Stage2Pillz11Lives13,
        Stage3Pillz10Lives14,
        Stage4Pillz9Lives15,
        Stage5to9Pillz8Lives15,
    }

    /// <summary>
    /// Represents the identifier used by UR to identify a clan.
    /// </summary>
    public enum ClanId
    {
        None = 0,
        AllStars = 38,
        Bangers = 31,
        Berzerk = 46,
        FangPiClang = 25,
        Freaks = 40,
        Frozn = 47,
        GHEIST = 32,
        GhosTown = 52,
        Hive = 51,
        Huracan = 48,
        Jungo = 43,
        Junkz = 26,
        LaJunta = 27,
        Leader = 36,
        Montana = 03,
        Nightmare = 37,
        Piranas = 42,
        Pussycats = 04,
        Raptors = 50,
        Rescue = 41,
        Riots = 49,
        Roots = 29,
        Sakrohm = 30,
        Sentinel = 33,
        Skeelz = 44,
        UluWatu = 10,
        Uppers = 28,
        Vortex = 45,
    }

    internal enum SupportIndex
    {
        None = -1,
        Leader = 0, // Leaders supporting each other? XD... just kidding, someone had to be zero =)
        AllStars,
        Bangers,
        Berzerk,
        FangPiClang,
        Freaks,
        Frozn,
        GHEIST,
        GhosTown,
        Hive,
        Huracan,
        Jungo,
        Junkz,
        LaJunta,
        Montana,
        Nightmare,
        Piranas,
        Pussycats,
        Raptors,
        Rescue,
        Riots,
        Roots,
        Sakrohm,
        Sentinel,
        Skeelz,
        UluWatu,
        Uppers,
        Vortex,
    }

    internal enum SkillIndex
    {
        LA = 0, // Left Ability
        RA = 1, // Right Ability
        LB = 2, // Left Bonus
        RB = 3, // Right Bonus
    }

    [Flags]
    internal enum EmptySkillCases
    {
        // Unlocked skill
        None = 0,

        // Mutually exclusive. 
        // Ability not yet unlocked. This must not be assigned to a Bonus Skill. 
        UnlockedAt2 = 0x01 | NoAbility,
        UnlockedAt3 = 0x02 | NoAbility,
        UnlockedAt4 = 0x03 | NoAbility,
        UnlockedAt5 = 0x04 | NoAbility,

        // The card has no Ability, or it is canceled
        NoAbility = 0x08,
        // The card is the only of his Clan being drawn
        NoBonus = 0x10, 
    }
    [Flags]
    internal enum ActivationCases
    {
        NoChain = 0,

        // Mutually exclusive
        StopAbility = 0x01,
        StopBonus = 0x02,
        ProtectAbility = 0x03,
        ProtectBonus = 0x04,

        // Mutually exclusive
        Stop = 0x08,
        Protect = 0x10,
    }
    [Flags]
    internal enum ActivationStatus
    {
        Normal = 0,
        Stopped,
    }
    [Flags]
    internal enum ProtectedStats
    {
        None = 0,
        Attack = 1 << 0,
        Damage = 1 << 1,
        Power = 1 << 2,
    }
    internal static class ProtectedStatsExtensions
    {
        public static bool Attack(this ProtectedStats flags)
        {
            return flags.HasThisFlag(ProtectedStats.Attack);
        }
        public static bool Damage(this ProtectedStats flags)
        {
            return flags.HasThisFlag(ProtectedStats.Damage);
        }
        public static bool Power(this ProtectedStats flags)
        {
            return flags.HasThisFlag(ProtectedStats.Power);
        }
        private static bool HasThisFlag(this ProtectedStats setOfFlags, ProtectedStats flag)
        {
            return (setOfFlags & flag) == flag;
        }
    }
    [Flags]
    internal enum CanceledModifiers
    {
        None = 0,
        Attack = 1 << 0,
        Damage = 1 << 1,
        Life = 1 << 2,
        Power = 1 << 3,
        Pillz = 1 << 4,
    }
    internal static class CanceledModifiersExtensions
    {
        public static bool Attack (this CanceledModifiers flags)
        {
            return flags.HasThisFlag(CanceledModifiers.Attack);
        }
        public static bool Damage (this CanceledModifiers flags)
        {
            return flags.HasThisFlag(CanceledModifiers.Damage);
        }
        public static bool Life (this CanceledModifiers flags)
        {
            return flags.HasThisFlag(CanceledModifiers.Life);
        }
        public static bool Pillz (this CanceledModifiers flags)
        {
            return flags.HasThisFlag(CanceledModifiers.Pillz);
        }
        public static bool Power (this CanceledModifiers flags)
        {
            return flags.HasThisFlag(CanceledModifiers.Power);
        }
        private static bool HasThisFlag(this CanceledModifiers setOfFlags, CanceledModifiers flag)
        {
            return (setOfFlags & flag) == flag;
        }
    }


}