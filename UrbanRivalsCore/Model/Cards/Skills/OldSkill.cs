﻿using System;
using System.Collections.Generic;
using System.Linq;
using UrbanRivalsCore.Model.Cards.Skills.Leaders;
using UrbanRivalsCore.Model.Cards.Skills.Prefixes;
using UrbanRivalsCore.Model.Cards.Skills.Suffixes;
using Warcraker.Utils;

namespace UrbanRivalsCore.Model.Cards.Skills
{
    public class OldSkill
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

        private class PrvDefaultPrefix : Prefix
        {
            public override bool isMatch(string text)
            {
                throw new InvalidOperationException();
            }
            public override string removePrefixFromText(string text)
            {
                throw new InvalidOperationException();
            }
            public override string ToString()
            {
                return "";
            }
        }

        public static readonly OldSkill ABILITY_UNLOCKED_AT_LEVEL_2;
        public static readonly OldSkill ABILITY_UNLOCKED_AT_LEVEL_3;
        public static readonly OldSkill ABILITY_UNLOCKED_AT_LEVEL_4;
        public static readonly OldSkill ABILITY_UNLOCKED_AT_LEVEL_5;
        public static readonly OldSkill NO_ABILITY;
        public static readonly OldSkill NO_BONUS;

        private static readonly Prefix PRV_DEFAULT_PREFIX = new PrvDefaultPrefix();

        private readonly PrvEEmptySkill emptySkillValue;
        private readonly Leader leader;
        private readonly Prefix[] prefixes;
        private readonly Suffix suffix;

        static OldSkill()
        {
            ABILITY_UNLOCKED_AT_LEVEL_2 = new OldSkill(PrvEEmptySkill.AbilityUnlockedAtLevel2);
            ABILITY_UNLOCKED_AT_LEVEL_3 = new OldSkill(PrvEEmptySkill.AbilityUnlockedAtLevel3);
            ABILITY_UNLOCKED_AT_LEVEL_4 = new OldSkill(PrvEEmptySkill.AbilityUnlockedAtLevel4);
            ABILITY_UNLOCKED_AT_LEVEL_5 = new OldSkill(PrvEEmptySkill.AbilityUnlockedAtLevel5);
            NO_ABILITY = new OldSkill(PrvEEmptySkill.NoAbility);
            NO_BONUS = new OldSkill(PrvEEmptySkill.NotActiveBonus);
        }

        public static OldSkill getLeaderSkill(Leader leader)
        {
            AssertArgument.CheckIsNotNull(leader, nameof(leader));

            return new OldSkill(leader);
        }
        public static OldSkill getSkillWithPrefixes(IEnumerable<Prefix> prefixes, Suffix suffix)
        {
            AssertArgument.CheckIsNotNull(prefixes, nameof(prefixes));
            int prefixesCount = prefixes.Count();
            AssertArgument.CheckIntegerRange(prefixesCount > 0, $"must contain at least one item", prefixesCount, nameof(prefixes));
            AssertArgument.CheckIsNotNull(suffix, nameof(suffix));

            return new OldSkill(prefixes, suffix);
        }
        public static OldSkill getSkillWithoutPrefixes(Suffix suffix)
        {
            AssertArgument.CheckIsNotNull(suffix, nameof(suffix));

            Prefix[] prefixes = new Prefix[]
            {
                PRV_DEFAULT_PREFIX,
            };
            return new OldSkill(prefixes, suffix);
        }

        private OldSkill()
        {
            this.emptySkillValue = PrvEEmptySkill.NormalSkill;
        }
        private OldSkill(PrvEEmptySkill emptySkillValue)
        {
            this.emptySkillValue = emptySkillValue;
            this.leader = null;
            this.prefixes = new Prefix[0];
            this.suffix = null;
        }
        private OldSkill(Leader leader)
            : this()
        {
            this.leader = leader;
            this.prefixes = new Prefix[0];
            this.suffix = null;
        }
        private OldSkill(IEnumerable<Prefix> prefixes, Suffix suffix)
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
                    Asserts.Fail("Invalid skill value state");
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