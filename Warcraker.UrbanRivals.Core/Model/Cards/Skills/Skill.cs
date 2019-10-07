using System;
using System.Collections.Generic;
using System.Linq;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills.Leaders;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills.Prefixes;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes;
using Warcraker.Utils;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills
{
    public class Skill
    {
        private enum EmptySkill
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

        public IEnumerable<Prefix> Prefixes { get; set; }
        public Suffix Suffix { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        private readonly IEnumerable<Prefix> prefixes;
        private readonly Suffix suffix;
        private readonly int x;
        private readonly int y;

        private readonly EmptySkill emptySkillValue;

        static Skill()
        {
            ABILITY_UNLOCKED_AT_LEVEL_2 = new Skill(EmptySkill.AbilityUnlockedAtLevel2);
            ABILITY_UNLOCKED_AT_LEVEL_3 = new Skill(EmptySkill.AbilityUnlockedAtLevel3);
            ABILITY_UNLOCKED_AT_LEVEL_4 = new Skill(EmptySkill.AbilityUnlockedAtLevel4);
            ABILITY_UNLOCKED_AT_LEVEL_5 = new Skill(EmptySkill.AbilityUnlockedAtLevel5);
            NO_ABILITY = new Skill(EmptySkill.NoAbility);
            NO_BONUS = new Skill(EmptySkill.NotActiveBonus);
        }

        public static Skill GetStandardSkill(IEnumerable<Prefix> prefixes, Suffix suffix, int x, int y)
        {
            AssertArgument.CheckIsNotNull(prefixes, nameof(prefixes));
            AssertArgument.CheckIsNotNull(suffix, nameof(suffix));
            AssertArgument.CheckIntegerRange(x >= 0, "Cannot be negative", x, nameof(x));
            AssertArgument.CheckIntegerRange(y >= 0, "Cannot be negative", y, nameof(y));

            return new Skill(prefixes, suffix, x, y);
        }
        
        public override string ToString()
        {
            string text;

            switch (this.emptySkillValue)
            {
                case EmptySkill.NotActiveBonus:
                    text = ""; // TODO // Properties.GameStrings.skill_no_bonus;
                    break;
                case EmptySkill.NoAbility:
                    text = ""; // TODO // Properties.GameStrings.skill_no_ability;
                    break;
                case EmptySkill.AbilityUnlockedAtLevel2:
                    text = GetUnlockedAtLevelStringRepresentation(2);
                    break;
                case EmptySkill.AbilityUnlockedAtLevel3:
                    text = GetUnlockedAtLevelStringRepresentation(3);
                    break;
                case EmptySkill.AbilityUnlockedAtLevel4:
                    text = GetUnlockedAtLevelStringRepresentation(4);
                    break;
                case EmptySkill.AbilityUnlockedAtLevel5:
                    text = GetUnlockedAtLevelStringRepresentation(5);
                    break;
                case EmptySkill.NormalSkill:
                    text = this.prefixes.Aggregate("", (acc, prefix) => acc + prefix.ToString());
                    text += this.suffix.ToString();
                    break;
                default:
                    text = "Invalid";
                    Asserts.Fail("Invalid skill value state");
                    break;
            }

            return text;
        }

        private Skill()
        {
            this.emptySkillValue = EmptySkill.NormalSkill;
            this.prefixes = new Prefix[0];
            this.suffix = null;
            this.x = 0;
            this.y = 0;
        }
        private Skill(EmptySkill emptySkillValue)
        {
            this.emptySkillValue = emptySkillValue;
        }
        private Skill(IEnumerable<Prefix> prefixes, Suffix suffix, int x, int y)
            : this()
        {
            this.prefixes = prefixes.ToArray();
            this.suffix = suffix;
            this.x = x;
            this.y = y;
        }

        private static string GetUnlockedAtLevelStringRepresentation(int level)
        {
            return "" + level; // TODO // String.Format(Properties.GameStrings.skill_not_unlocked, level);
        }
    }
}
