using System;
using System.Collections.Generic;
using System.Linq;
using UrbanRivalsCore.Model.Cards.Skills.Leaders;
using UrbanRivalsCore.Model.Cards.Skills.Prefixes;
using UrbanRivalsCore.Model.Cards.Skills.Suffixes;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;
using UrbanRivalsUtils;

namespace UrbanRivalsCore.Model.Cards.Skills
{
    public static class SkillParser
    {
        private const string PRV_NO_ABILITY_TEXT = "No ability";
        private static readonly IEnumerable<Leader> PRV_ALL_LEADERS;
        private static readonly IEnumerable<Prefix> PRV_ALL_PREFIXES;
        private static readonly IEnumerable<SuffixParser> PRV_ALL_SUFFIX_PARSERS;

        static SkillParser()
        {
            PRV_ALL_PREFIXES = new List<Prefix>
            {
                new BacklashPrefix(),
                new BrawlPrefix(),
                new ConfidencePrefix(),
                new CouragePrefix(),
                new DefeatPrefix(),
                new DegrowthPrefix(),
                new EqualizerPrefix(),
                new GrowthPrefix(),
                new KillshotPrefix(),
                new ReprisalPrefix(),
                new RevengePrefix(),
                new StopPrefix(),
                new SupportPrefix(),
                new VictoryOrDefeatPrefix(),
            };
            PRV_ALL_LEADERS = new List<Leader>
            {
                new AmbreLeader(),
                new AshigaruLeader(),
                new BridgetLeader(),
                new EkloreLeader(),
                new EyrikLeader(),
                new HugoLeader(),
                new JohnDoomLeader(),
                new MelodyLeader(),
                new MorphunLeader(),
                new MrBigDukeLeader(),
                new RobertCobbLeader(),
                new SolomonLeader(),
                new TimberLeader(),
                new VansaarLeader(),
                new VholtLeader(),
            };
            PRV_ALL_SUFFIX_PARSERS = new List<SuffixParser>
            {
                CancelAttackModifierSuffix.getParser(),
                CancelDamageModifierSuffix.getParser(),
                CancelLeaderSuffix.getParser(),
                ConsumeXMinYSuffix.getParser(),
                CopyBonusSuffix.getParser(),
                CorrosionXMinYSuffix.getParser(),
                DecreaseAttackXMinYSuffix.getParser(),
                DecreaseDamageXMinYSuffix.getParser(),
                IncreaseAttackXSuffix.getParser(),
            };
        }

        public static Skill parseSkill(string skillAsText)
        {
            Skill skill;

            if (skillAsText == PRV_NO_ABILITY_TEXT)
            {
                skill = Skill.NO_ABILITY;
            }
            else
            {
                Leader leader = prv_parseLeader(skillAsText);
                if (leader != null)
                {
                    skill = Skill.getLeaderSkill(leader);
                }
                else
                {
                    string suffixAsText;

                    IEnumerable<Prefix> prefixes = prv_parsePrefixes(skillAsText, out suffixAsText);
                    Suffix suffix = prv_parseSuffix(suffixAsText);

                    if (prefixes.Any())
                    {
                        skill = Skill.getSkillWithPrefixes(prefixes, suffix);
                    }
                    else
                    {
                        skill = Skill.getSkillWithoutPrefixes(suffix);
                    }
                }
            }

            return skill;
        }

        private static Leader prv_parseLeader(string abilityText)
        {
            return PRV_ALL_LEADERS.SingleOrDefault(leader => leader.isMatch(abilityText));
        }
        private static IEnumerable<Prefix> prv_parsePrefixes(string textToParse, out string textWithoutPrefixes)
        {
            List<Prefix> prefixes;
            Prefix parsedPrefix;

            prefixes = new List<Prefix>();
            do
            {
                parsedPrefix = PRV_ALL_PREFIXES.FirstOrDefault(prefix => prefix.isMatch(textToParse));
                if (parsedPrefix != null)
                {
                    textToParse = parsedPrefix.removePrefixFromText(textToParse);
                    prefixes.Add(parsedPrefix);
                }
            } while (parsedPrefix != null);

            textWithoutPrefixes = textToParse;

            return prefixes;
        }
        private static Suffix prv_parseSuffix(string suffixAsText)
        {
            SuffixParser parser = PRV_ALL_SUFFIX_PARSERS.SingleOrDefault(p => p.isMatch(suffixAsText));
            Asserts.check(parser != null, $"No {nameof(SuffixParser)} was found for text [{suffixAsText}]");
            return parser.getSuffix(suffixAsText);
        }
    }
}
