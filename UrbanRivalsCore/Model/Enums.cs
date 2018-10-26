﻿using System;

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

    public enum CardRarity
    {
        Common = 0,
        Uncommon,
        Rare,
        Collector,
        Legendary,
        Rebirth,
        Mythic,
    }

    public enum RandomFactor
    {
        NonRandom,
        Random,
    }

    public enum CombatWinner
    {
        None = 0,
        Draw,
        Left,
        Right,
    }

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

    public enum SkillLeader
    {
        None = 0,
        Ambre,
        Ashigaru,
        Bridget,
        Eklore,
        Eyrik,
        Hugo,
        JonhDoom,
        Melody,
        Morphun,
        MrBigDuke,
        RobbertCobb,
        Solomon,
        Timber,
        Vansaar,
        Vholt,
    }

    public enum SkillPrefix
    {
        None= 0,
        GrowthAndDefeat,

        Backlash,
        Brawl,
        Confidence,
        Courage,
        Defeat,
        Degrowth,
        Equalizer,
        Growth,
        Killshot,
        Reprisal,
        Revenge,
        Stop,
        Support,
        VictoryOrDefeat,
    }

    public enum SkillSuffix
    {
        None = 0,
        CancelAttackModifier,
        CancelDamageModifier,
        CancelLeader,
        CancelLifeModifier,
        CancelPillzModifier,
        CancelPillzAndLifeModifier,
        CancelPowerAndDamageModifier,
        CancelPowerModifier,
        ConsumeXMinY,
        CopyBonus,
        CopyDamage,
        CopyPower,
        CopyPowerAndDamage,
        CorrosionXMinY,
        DecreaseAttackXMinY,
        DecreaseAttackXPerRemainingLifeMinY,
        DecreaseAttackXPerRemainingPillzMinY,
        DecreaseDamageXMinY,
        DecreaseLifeAndPillzXMinY,
        DecreaseLifeXMinY,
        DecreasePillzXMinY,
        DecreasePowerAndDamageXMinY,
        DecreasePowerXMinY,
        DopeXMaxY,
        ExchangeDamage,
        ExchangePower,
        HealXMaxY,
        IncreaseAttackX,
        IncreaseAttackXPerRemainingLife,
        IncreaseAttackXPerRemainingPillz,
        IncreaseDamageX,
        IncreaseLifeX,
        IncreaseLifeXMaxY,
        IncreaseLifeXPerDamage,
        IncreaseLifeXPerDamageMaxY,
        IncreasePillzAndLifeX,
        IncreasePillzX,
        IncreasePillzXMaxY,
        IncreasePillzXPerDamage,
        IncreasePowerAndDamageX,
        IncreasePowerX,
        InfectionXMinY,
        PoisonXMinY,
        ProtectAbility,
        ProtectAttack,
        ProtectBonus,
        ProtectDamage,
        ProtectPower,
        ProtectPowerAndDamage,
        ReanimateX,
        RebirthXMaxY,
        RecoverXPillzOutOfY,
        RegenXMaxY,
        StopAbility,
        StopBonus,
        ToxinXMinY,
    }

    // TODO Remove if not used
    public enum SurvivorStage
    {
        Stage1Pillz12Lives12,
        Stage2Pillz11Lives13,
        Stage3Pillz10Lives14,
        Stage4Pillz9Lives15,
        Stage5to9Pillz8Lives15,
    }

    public enum ClanId
    {
        None = 0,
        AllStars = 38,
        Bangers = 31,
        Berzerk = 46,
        Dominion = 53,
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

    internal enum SkillIndex
    {
        LA = 0, // Left Ability
        RA = 1, // Right Ability
        LB = 2, // Left Bonus
        RB = 3, // Right Bonus
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