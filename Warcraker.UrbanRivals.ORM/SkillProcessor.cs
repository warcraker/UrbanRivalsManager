using HashUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static Warcraker.UrbanRivals.Common.Constants;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills.Prefixes;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes;
using Warcraker.UrbanRivals.DataRepository.DataModels;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Double;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Plain;
using Warcraker.UrbanRivals.SkillParser;

namespace Warcraker.UrbanRivals.ORM
{
    public class SkillProcessor
    {
        private const string NO_ABILITY_TEXT = "No ability";
        private static readonly Regex FILLER_CHARS_REGEX = new Regex("[ ,.]+");
        private static readonly PrefixParser[] PREFIX_PARSERS;
        private static readonly SuffixParser[] SUFFIX_PARSERS;

        static SkillProcessor()
        {
            // TODO Leader parsers
            // TODO add suffix parsers
            // TODO assert number of prefix/suffix's created by reflection

            PREFIX_PARSERS = new PrefixParser[]
            {
                new PrefixParser(new BacklashPrefix(), GetRegex("^Backlash:")),
                new PrefixParser(new BrawlPrefix(), GetRegex("^Brawl:")),
                new PrefixParser(new ConfidencePrefix(), GetRegex("^Confid(?:ence)?:")),
                new PrefixParser(new CouragePrefix(), GetRegex("^Courage:")),
                new PrefixParser(new DayPrefix(), GetRegex("^Day:")),
                new PrefixParser(new DefeatPrefix(), GetRegex("^Defeat:")),
                new PrefixParser(new DegrowthPrefix(), GetRegex("^Degrowth:")),
                new PrefixParser(new EqualizerPrefix(), GetRegex("^Equalizer:")),
                new PrefixParser(new GrowthPrefix(), GetRegex("^Growth:")),
                new PrefixParser(new KillshotPrefix(), GetRegex("^Killshot:")),
                new PrefixParser(new NightPrefix(), GetRegex("^Night:")),
                new PrefixParser(new ReprisalPrefix(), GetRegex("^Reprisal:")),
                new PrefixParser(new RevengePrefix(), GetRegex("^Revenge:")),
                new PrefixParser(new StopPrefix(), GetRegex("^Stop:")),
                new PrefixParser(new SupportPrefix(), GetRegex("^Support:")),
                new PrefixParser(new VictoryOrDefeatPrefix(), GetRegex(@"^Vict(?:ory)?OrDef(?:eat)?:")),
            };
            SuffixParser[] doubleValueSuffixParsers = new SuffixParser[]
            {
                new SuffixParser((x, y) => new CombustXMinYSuffix(x, y), GetRegex(@"^Combust(?<x>[0-9])Min(?<y>[0-9])$")),
            };
            SuffixParser[] singleValueSuffixParsers = new SuffixParser[]
            {
                new SuffixParser((x, y) => new IncreaseAttackXPerOppDamageSuffix(x), GetRegex(@"^\+(?<x>[1-9])AttackPerOppDamage$")),
            };
            SuffixParser[] plainSuffixParsers = new SuffixParser[]
            {
                new SuffixParser((x, y) => new CancelAttackModifierSuffix(), GetRegex(@"^CancelOppAttackModif$")),
            };

            SUFFIX_PARSERS = doubleValueSuffixParsers
                .Concat(singleValueSuffixParsers)
                .Concat(plainSuffixParsers)
                .ToArray();
        }

        public static Skill ParseSkill(string text)
        {
            Skill skill;

            if (text == NO_ABILITY_TEXT)
            {
                skill = PlaceholderSkill.NO_ABILITY;
            }
            else
            {
                string cleanText = FILLER_CHARS_REGEX.Replace(text, "");
                cleanText = cleanText.Replace(';', ':');

                string suffixAsText;
                IEnumerable<Prefix> prefixes = ParsePrefixes(cleanText, out suffixAsText);

                SuffixParser suffixParser = SUFFIX_PARSERS.FirstOrDefault(p => p.IsMatch(suffixAsText));

                if (suffixParser != null)
                {
                    Suffix suffix = suffixParser.ParseSuffix(suffixAsText);
                    skill = new Skill(prefixes, suffix);
                }
                else
                {
                    // TODO
                    //Leader leader = LEADERS.FirstOrDefault(x => x.isMatch(skillAsText));
                    //if (leader != null)
                    //{
                    //    skill = Skill.getLeaderSkill(leader);
                    //}
                    //else
                    //{
                    // TODO Write alternative when unknown string
                    //}

                    return null;
                }
            }

            return skill;
        }

        private static IEnumerable<Prefix> ParsePrefixes(string textToParse, out string textWithoutPrefixes)
        {
            List<Prefix> prefixes = new List<Prefix>();
            PrefixParser parser;

            textWithoutPrefixes = textToParse;
            do
            {
                parser = PREFIX_PARSERS.FirstOrDefault(prefix => prefix.IsMatch(textToParse));
                if (parser != null)
                {
                    textWithoutPrefixes = parser.RemovePrefixFromText(textWithoutPrefixes);
                    prefixes.Add(parser.Prefix);
                }
            } while (parser != null);

            return prefixes;
        }

        private static SkillData SkillToSkillData(Skill skill)
        {
            string[] prefixNames = skill.Prefixes
                .Select(prefix => GetTypeName(prefix))
                .ToArray();

            int skillHash = HashCode
                .OfEach(prefixNames)
                .And(GetTypeName(skill.Suffix))
                .And(skill.Suffix.X)
                .And(skill.Suffix.Y);

            string prefixCsv = prefixNames
                .Aggregate(new StringBuilder(), (acc, item) => acc.Append(COMMA_SEPARATOR).Append(item))
                .ToString();

            return new SkillData
            {
                Hash = skillHash,
                PrefixesClassNames = prefixCsv,
                SuffixClassName = GetTypeName(skill.Suffix),
                X = skill.Suffix.X,
                Y = skill.Suffix.Y,
            };
        }
        private static Regex GetRegex(string pattern)
        {
            return new Regex(pattern, RegexOptions.None);
        }
        private static string GetTypeName(object o)
        {
            return o.GetType().Name;
        }
    }
}
