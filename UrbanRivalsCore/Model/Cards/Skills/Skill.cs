using System;
using System.Collections.Generic;
using System.Linq;
using UrbanRivalsCore.Model.Cards.Skills.Leaders;
using UrbanRivalsCore.Model.Cards.Skills.Prefixes;
using UrbanRivalsCore.Model.Cards.Skills.Suffixes;
using UrbanRivalsUtils;

namespace UrbanRivalsCore.Model.Cards.Skills
{
    public class Skill
    {
        private enum PrvEEmptySkill
        {
            NormalSkill = 0,
            NotActiveBonus,
            NoAbility,
            AbilityUnlockedAtLevel2,
            AbilityUnlockedAtLevel3,
            AbilityUnlockedAtLevel4,
            AbilityUnlockedAtLevel5,
        }

        public static readonly Skill ABILITY_UNLOCKED_AT_LEVEL_2;
        public static readonly Skill ABILITY_UNLOCKED_AT_LEVEL_3;
        public static readonly Skill ABILITY_UNLOCKED_AT_LEVEL_4;
        public static readonly Skill ABILITY_UNLOCKED_AT_LEVEL_5;
        public static readonly Skill NO_ABILITY;
        public static readonly Skill NO_BONUS;

        private readonly PrvEEmptySkill emptySkillValue;
        private readonly Leader leader;
        private readonly Prefix[] prefixes;
        private readonly Suffix suffix;

        static Skill()
        {
            ABILITY_UNLOCKED_AT_LEVEL_2 = new Skill(PrvEEmptySkill.AbilityUnlockedAtLevel2);
            ABILITY_UNLOCKED_AT_LEVEL_3 = new Skill(PrvEEmptySkill.AbilityUnlockedAtLevel3);
            ABILITY_UNLOCKED_AT_LEVEL_4 = new Skill(PrvEEmptySkill.AbilityUnlockedAtLevel4);
            ABILITY_UNLOCKED_AT_LEVEL_5 = new Skill(PrvEEmptySkill.AbilityUnlockedAtLevel5);
            NO_ABILITY = new Skill(PrvEEmptySkill.NoAbility);
            NO_BONUS = new Skill(PrvEEmptySkill.NotActiveBonus);
        }

        public static Skill getLeaderSkill(Leader leader)
        {
            AssertArgument.isNotNull(leader, nameof(leader));

            return new Skill(leader);
        }
        public static Skill getSkill(IEnumerable<Prefix> prefixes, Suffix suffix)
        {
            AssertArgument.isNotNull(prefixes, nameof(prefixes));
            int prefixesCount = prefixes.Count();
            AssertArgument.checkIntegerRange(prefixesCount > 0, $"must contain at least one item", prefixesCount, nameof(prefixes));
            AssertArgument.isNotNull(suffix, nameof(suffix));

            return new Skill(prefixes, suffix);
        }

        private Skill()
        {
            this.emptySkillValue = PrvEEmptySkill.NormalSkill;
        }
        private Skill(PrvEEmptySkill emptySkillValue)
        {
            this.emptySkillValue = emptySkillValue;
            this.leader = null;
            this.prefixes = new Prefix[0];
            this.suffix = null;
        }
        private Skill(Leader leader)
            : this()
        {
            this.leader = leader;
            this.prefixes = new Prefix[0];
            this.suffix = null;
        }
        private Skill(IEnumerable<Prefix> prefixes, Suffix suffix)
            : this()
        {
            this.leader = null;
            this.prefixes = prefixes.ToArray();
            this.suffix = suffix;
        }

        public override string ToString()
        {
            string text;

            switch (this.emptySkillValue)
            {
                case PrvEEmptySkill.NotActiveBonus:
                    text = Properties.GameStrings.skill_no_bonus;
                    break;
                case PrvEEmptySkill.NoAbility:
                    text = Properties.GameStrings.skill_no_ability;
                    break;
                case PrvEEmptySkill.AbilityUnlockedAtLevel2:
                    text = prv_getUnlockedAtLevelStringRepresentation(2);
                    break;
                case PrvEEmptySkill.AbilityUnlockedAtLevel3:
                    text = prv_getUnlockedAtLevelStringRepresentation(3);
                    break;
                case PrvEEmptySkill.AbilityUnlockedAtLevel4:
                    text = prv_getUnlockedAtLevelStringRepresentation(4);
                    break;
                case PrvEEmptySkill.AbilityUnlockedAtLevel5:
                    text = prv_getUnlockedAtLevelStringRepresentation(5);
                    break;
                case PrvEEmptySkill.NormalSkill:
                    if (this.leader != null)
                    {
                        text = this.leader.ToString();
                    }
                    else
                    {
                        text = "";
                        for (int i = 0; i < this.prefixes.Length; i++)
                        {
                            text += this.prefixes[i].ToString();
                        }
                        text += this.suffix.ToString();
                    }
                    break;
                default:
                    text = "";
                    Asserts.fail("Invalid skill value state");
                    break;
            }

            return text;
        }

        private static string prv_getUnlockedAtLevelStringRepresentation(int level)
        {
            return String.Format(Properties.GameStrings.skill_not_unlocked, level);
        }
    }
}
