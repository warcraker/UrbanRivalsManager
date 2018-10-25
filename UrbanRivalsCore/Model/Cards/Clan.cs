using System;
using System.Collections.Generic;
using System.Linq;
using UrbanRivalsUtils;

namespace UrbanRivalsCore.Model
{
    public class Clan
    {
        private static readonly List<Clan> s_allClans;
        private static readonly int s_numberOfClans;

        static Clan()
        {
            s_allClans = new List<Clan>()
            {
                new Clan(ClanId.Leader,         "Leader",          new Skill(SkillPrefix.None, SkillSuffix.CancelLeader)),
                new Clan(ClanId.AllStars,       "All Stars",       new Skill(SkillPrefix.None, SkillSuffix.DecreasePowerXMinY, 2, 1)),
                new Clan(ClanId.Bangers,        "Bangers",         new Skill(SkillPrefix.None, SkillSuffix.IncreasePowerX, 2)),
                new Clan(ClanId.Berzerk,        "Berzerk",         new Skill(SkillPrefix.None, SkillSuffix.DecreaseLifeXMinY, 2, 2)),
                new Clan(ClanId.Dominion,       "Dominion",        new Skill(SkillPrefix.Growth, SkillSuffix.DecreasePowerXMinY, 1, 4)),
                new Clan(ClanId.FangPiClang,    "Fang Pi Clang",   new Skill(SkillPrefix.None, SkillSuffix.IncreaseDamageX, 2)),
                new Clan(ClanId.Freaks,         "Freaks",          new Skill(SkillPrefix.None, SkillSuffix.PoisonXMinY, 2, 3)),
                new Clan(ClanId.Frozn,          "Frozn",           new Skill(SkillPrefix.Revenge, SkillSuffix.IncreasePowerAndDamageX, 2)),
                new Clan(ClanId.GHEIST,         "GHEIST",          new Skill(SkillPrefix.None, SkillSuffix.StopAbility)),
                new Clan(ClanId.GhosTown,       "GhosTown",        new Skill(SkillPrefix.None, SkillSuffix.IncreasePowerAndDamageX, 1)), // TODO Prefix day / night
                new Clan(ClanId.Hive,           "Hive",            new Skill(SkillPrefix.Equalizer, SkillSuffix.DecreaseAttackXMinY, 3, 5)),
                new Clan(ClanId.Huracan,        "Huracan",         new Skill(SkillPrefix.None, SkillSuffix.IncreaseAttackXPerRemainingLife, 1)),
                new Clan(ClanId.Jungo,          "Jungo",           new Skill(SkillPrefix.None, SkillSuffix.IncreaseLifeX, 2)),
                new Clan(ClanId.Junkz,          "Junkz",           new Skill(SkillPrefix.None, SkillSuffix.IncreaseAttackX, 8)),
                new Clan(ClanId.LaJunta,        "La Junta",        new Skill(SkillPrefix.None, SkillSuffix.IncreaseDamageX, 2)),
                new Clan(ClanId.Montana,        "Montana",         new Skill(SkillPrefix.None, SkillSuffix.DecreaseAttackXMinY, 12, 8)),
                new Clan(ClanId.Nightmare,      "Nightmare",       new Skill(SkillPrefix.None, SkillSuffix.StopBonus)),
                new Clan(ClanId.Piranas,        "Piranas",         new Skill(SkillPrefix.None, SkillSuffix.StopBonus)),
                new Clan(ClanId.Pussycats,      "Pussycats",       new Skill(SkillPrefix.None, SkillSuffix.DecreaseDamageXMinY, 2, 1)),
                new Clan(ClanId.Raptors,        "Raptors",         new Skill(SkillPrefix.None, SkillSuffix.CancelAttackModifier)),
                new Clan(ClanId.Rescue,         "Rescue",          new Skill(SkillPrefix.Support, SkillSuffix.IncreaseAttackX, 3)),
                new Clan(ClanId.Riots,          "Riots",           new Skill(SkillPrefix.VictoryOrDefeat, SkillSuffix.IncreasePillzX, 1)),
                new Clan(ClanId.Roots,          "Roots",           new Skill(SkillPrefix.None, SkillSuffix.StopAbility)),
                new Clan(ClanId.Sakrohm,        "Sakrohm",         new Skill(SkillPrefix.None, SkillSuffix.DecreaseAttackXMinY, 8, 3)),
                new Clan(ClanId.Sentinel,       "Sentinel",        new Skill(SkillPrefix.None, SkillSuffix.IncreaseAttackX, 8)),
                new Clan(ClanId.Skeelz,         "Skeelz",          new Skill(SkillPrefix.None, SkillSuffix.ProtectAbility)),
                new Clan(ClanId.UluWatu,        "Ulu Watu",        new Skill(SkillPrefix.None, SkillSuffix.IncreasePowerX, 2)),
                new Clan(ClanId.Uppers,         "Uppers",          new Skill(SkillPrefix.None, SkillSuffix.DecreaseAttackXMinY, 10, 3)),
                new Clan(ClanId.Vortex,         "Vortex",          new Skill(SkillPrefix.Defeat, SkillSuffix.RecoverXPillzOutOfY, 2, 3)),
            };
            s_numberOfClans = s_allClans.Count;
        }

        public ClanId id { get; private set; }
        public string name { get; private set; }
        public Skill bonus { get; private set; }

        private Clan(ClanId id, String name, Skill bonus)
        {
            this.id = id;
            this.name = name;
            this.bonus = bonus;
        }

        public static int getNumberOfClans()
        {
            return s_numberOfClans;
        }
        public static Clan getClanByName(string name)
        {
            Clan clan;

            AssertArgument.stringIsFilled(name, nameof(name));
            clan = s_allClans.FirstOrDefault(c => c.name == name);
            AssertArgument.check(clan != null, "Clan name doesn't exist", nameof(name));

            return clan;
        }
        public static Clan getClanById(ClanId id)
        {
            Clan clan;

            clan = s_allClans.FirstOrDefault(c => c.id == id);
            AssertArgument.check(clan != null, $"Must be a valid {nameof(ClanId)}", nameof(id));

            return clan;
        }

        public override string ToString()
        {
            return this.name;
        }
    }
}