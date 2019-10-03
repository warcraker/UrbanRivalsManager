using System;
using System.Collections.Generic;
using System.Linq;
using UrbanRivalsCore.Model.Cards.Skills.Leaders;
using UrbanRivalsCore.Model.Cards.Skills.Prefixes;
using UrbanRivalsCore.Model.Cards.Skills.Suffixes;
using UrbanRivalsCore.Model.Cards.Skills.Suffixes.DoubleValue;
using UrbanRivalsCore.Model.Cards.Skills.Suffixes.SingleValue;
using UrbanRivalsCore.Model.Cards.Skills.Suffixes.Plain;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;
using UrbanRivalsUtils;
using System.Text.RegularExpressions;

namespace UrbanRivalsCore.Model.Cards.Skills
{
    public static class SkillParser
    {
        private const string PRV_NO_ABILITY_TEXT = "No ability";
        private static readonly IEnumerable<Leader> PRV_ALL_LEADERS;
        public static readonly List<Prefix> PRV_ALL_PREFIXES;
        public static readonly List<SuffixParser> PRV_ALL_SUFFIX_PARSERS;
        private static readonly Regex PRV_REMOVE_FILLER_CHARS = new Regex("[ ,.]");

        static SkillParser()
        {
            // TODO get all parsers by reflection
            PRV_ALL_PREFIXES = new List<Prefix>
            {
                new BacklashPrefix(),
                new BrawlPrefix(),
                new ConfidencePrefix(),
                new CouragePrefix(),
                new DayPrefix(),
                new DefeatPrefix(),
                new DegrowthPrefix(),
                new EqualizerPrefix(),
                new GrowthPrefix(),
                new KillshotPrefix(),
                new NightPrefix(),
                new ReprisalPrefix(),
                new RevengePrefix(),
                new StopPrefix(),
                new SupportPrefix(),
                new VictoryOrDefeatPrefix(),
            };
            PRV_ALL_LEADERS = new Leader[]
            {
                new AmbreLeader(),
                new AshigaruLeader(),
                new BridgetLeader(),
                new EkloreLeader(),
                new EyrikLeader(),
                new HugoLeader(),
                new JohnDoomLeader(),
                new MelodyLeader(),
                new MementoLeader(),
                new MorphunLeader(),
                new MrBigDukeLeader(),
                new RobertCobbLeader(),
                new SolomonLeader(),
                new TimberLeader(),
                new VansaarLeader(),
                new VholtLeader(),
            };

            PlainSuffixParser[] allPlainSuffixParsers = new PlainSuffixParser[]
            {
                CancelAttackModifierSuffix.getParser(),
                CancelDamageModifierSuffix.getParser(),
                CancelLeaderSuffix.getParser(),
                CancelLifeModifierSuffix.getParser(),
                CancelPillzAndLifeModifierSuffix.getParser(),
                CancelPillzModifierSuffix.getParser(),
                CancelPowerAndDamageModifierSuffix.getParser(),
                CancelPowerModifierSuffix.getParser(),
                CopyBonusSuffix.getParser(),
                CopyDamageSuffix.getParser(),
                CopyPowerAndDamageSuffix.getParser(),
                CopyPowerSuffix.getParser(),
                ExchangeDamageSuffix.getParser(),
                ExchangePowerAndDamageSuffix.getParser(),
                ExchangePowerSuffix.getParser(),
                ProtectAbilitySuffix.getParser(),
                ProtectAttackSuffix.getParser(),
                ProtectBonusSuffix.getParser(),
                ProtectDamageSuffix.getParser(),
                ProtectPowerAndDamageSuffix.getParser(),
                ProtectPowerSuffix.getParser(),
                StopAbilitySuffix.getParser(),
                StopBonusSuffix.getParser(),
            };
            SingleValueSuffixParser[] allSingleValueSuffixParsers = new SingleValueSuffixParser[]
            {
                IncreaseAttackXPerRemainingLifeSuffix.getParser(),
                IncreaseAttackXPerRemainingPillzSuffix.getParser(),
                IncreaseAttackXPerOppDamageSuffix.getParser(),
                IncreaseAttackXPerOppPowerSuffix.getParser(),
                IncreaseAttackXSuffix.getParser(),
                IncreaseDamageXSuffix.getParser(),
                IncreaseLifeXPerDamage.getParser(),
                IncreaseLifeXSuffix.getParser(),
                IncreasePillzAndLifeXSuffix.getParser(),
                IncreasePillzXPerDamageSuffix.getParser(),
                IncreasePillzXSuffix.getParser(),
                IncreasePowerAndDamageXSuffix.getParser(),
                IncreasePowerXSuffix.getParser(),
                ReanimateXSuffix.getParser(),
            };
            DoubleValueSuffixParser[] allDoubleValueSuffixParsers = new DoubleValueSuffixParser[]
            {
                CombustXMinYSuffix.getParser(),
                ConsumeXMinYSuffix.getParser(),
                CorrosionXMinYSuffix.getParser(),
                DecreaseAttackXMinYSuffix.getParser(),
                DecreaseAttackXPerRemainingLifeMinYSuffix.getParser(),
                DecreaseAttackXPerRemainingPillzMinYSuffix.getParser(),
                DecreaseDamageXMinYSuffix.getParser(),
                DecreaseLifeAndPillzXMinYSuffix.getParser(),
                DecreaseLifeXMinYSuffix.getParser(),
                DecreasePillzXMinYSuffix.getParser(),
                DecreasePowerAndDamageXMinYSuffix.getParser(),
                DecreasePowerXMinYSuffix.getParser(),
                DopeAndRegenXMaxYSuffix.getParser(),
                DopeXMaxYSuffix.getParser(),
                HealXMaxYSuffix.getParser(),
                IncreaseLifePerDamageXMaxYSuffix.getParser(),
                IncreaseLifeXMaxYSuffix.getParser(),
                IncreaseLifePerDamageXMaxYSuffix.getParser(),
                IncreasePillzPerDamageXMaxYSuffix.getParser(),
                IncreasePillzXMaxYSuffix.getParser(),
                InfectionXMinYSuffix.getParser(),
                PoisonXMinYSuffix.getParser(),
                RebirthXMaxYSuffix.getParser(),
                RecoverXPillzOutOfYSuffix.getParser(),
                RegenXMaxYSuffix.getParser(),
                RepairXMaxYSuffix.getParser(),
                ToxinXMinYSuffix.getParser(),
                XantiaxXMinYSuffix.getParser(),
            };

            PRV_ALL_SUFFIX_PARSERS = new List<SuffixParser>();
            PRV_ALL_SUFFIX_PARSERS.AddRange(allPlainSuffixParsers);
            PRV_ALL_SUFFIX_PARSERS.AddRange(allSingleValueSuffixParsers);
            PRV_ALL_SUFFIX_PARSERS.AddRange(allDoubleValueSuffixParsers);

            PRV_ALL_SUFFIX_PARSERS = PRV_ALL_SUFFIX_PARSERS.OrderBy(s => s.Weight).ToList();
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
                string cleanText = prv_parseFillerChars(skillAsText);
                string suffixAsText;
                IEnumerable<Prefix> prefixes = prv_parsePrefixes(cleanText, out suffixAsText);

                SuffixParser parser = PRV_ALL_SUFFIX_PARSERS.FirstOrDefault(p => p.isMatch(suffixAsText));

                if (parser != null)
                {
                    Suffix suffix = parser.getSuffix(suffixAsText);
                    if (prefixes.Any())
                    {
                        skill = Skill.getSkillWithPrefixes(prefixes, suffix);
                    }
                    else
                    {
                        skill = Skill.getSkillWithoutPrefixes(suffix);
                    }
                }
                else
                {
                    Leader leader = PRV_ALL_LEADERS.FirstOrDefault(x => x.isMatch(skillAsText));

                    if (leader != null)
                    {
                        skill = Skill.getLeaderSkill(leader);
                    }
                    else
                    {
                        return null; // TODO Write alternative when unknown string
                    }
                }
            }

            return skill;
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
            Asserts.Check(parser != null, $"No {nameof(SuffixParser)} was found for text [{suffixAsText}]");
            return parser.getSuffix(suffixAsText);
        }
        private static string prv_parseFillerChars(string text)
        {
            string cleantText = PRV_REMOVE_FILLER_CHARS.Replace(text, "");
            cleantText = cleantText.Replace(';', ':');

            return cleantText;
        }
    }
}
