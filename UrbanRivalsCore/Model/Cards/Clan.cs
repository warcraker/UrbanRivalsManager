using System;
using System.Collections.Generic;
using System.Linq;

namespace UrbanRivalsCore.Model
{
    /// <summary>
    /// Represents a clan in the game.
    /// </summary>
    public class Clan
    {
        /// <summary>
        /// Gets the identifier that UR uses to identify this clan.
        /// </summary>
        public ClanId ClanId { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the bonus.
        /// </summary>
        public Skill Bonus { get; private set; }

        /// <summary>
        /// Gets the number of existing clans.
        /// </summary>
        public static int NumberOfClans { get { return ClanList.Count; } }

        private static List<Clan> ClanList = new List<Clan>{                                                                                  
            new Clan(ClanId.Leader,         "Leader",          new Skill(SkillPrefix.None, SkillSuffix.CancelLeader)),
            new Clan(ClanId.AllStars,       "All Stars",       new Skill(SkillPrefix.None, SkillSuffix.DecreasePowerXMinY, 2, 1)),
            new Clan(ClanId.Bangers,        "Bangers",         new Skill(SkillPrefix.None, SkillSuffix.IncreasePowerX, 2)),
            new Clan(ClanId.Berzerk,        "Berzerk",         new Skill(SkillPrefix.None, SkillSuffix.DecreaseLifeXMinY, 2, 2)),
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

        // Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Clan"/> class.
        /// </summary>
        /// <param name="id">Clan id.</param>
        /// <param name="name">Name.</param>
        /// <param name="bonus">Bonus.</param>
        private Clan(ClanId id, String name, Skill bonus)
        {
            ClanId = id;
            Name = name;
            Bonus = bonus;
        }

        // Functions

        /// <summary>
        /// Returns the <see cref="Clan"/> that matches the name.
        /// </summary>
        /// <param name="name">Name of the clan.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> must be a valid clan name.</exception>
        public static Clan GetClanByName(string name)
        {
            var result = ClanList.FirstOrDefault<Clan>(c => c.Name == name);

            if (result == null)
                throw new ArgumentException(nameof(name), "Must be a valid " + nameof(Clan) + " name");

            return result;
        }

        /// <summary>
        /// Returns the <see cref="Clan"/> that matches the id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="id"/> must be a valid <see cref="ClanId"/>.</exception>
        public static Clan GetClanById(ClanId id)
        {
            var result = ClanList.FirstOrDefault<Clan>(c => c.ClanId == id);

            if (result == null)
                throw new ArgumentException(nameof(id), "Must be a valid " + nameof(ClanId));

            return result;
        }

        /// <summary>
        /// Returns the string representation of the clan.
        /// </summary>
        /// <returns>Name of the clan</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}