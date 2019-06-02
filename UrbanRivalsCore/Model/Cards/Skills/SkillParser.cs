﻿using System;
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
        private static readonly IEnumerable<Prefix> PRV_ALL_PREFIXES;
        private static readonly IEnumerable<SuffixParser> PRV_ALL_SUFFIX_PARSERS;
        private static readonly Regex PRV_REMOVE_FILLER_CHARS = new Regex("[ ,.]");

        static SkillParser()
        {
            PRV_ALL_PREFIXES = new Prefix[]
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
                IncreasePillzXMaxYSuffix.getParser(),
                InfectionXMinYSuffix.getParser(),
                PoisonXMinYSuffix.getParser(),
                RebirthXMaxYSuffix.getParser(),
                RecoverXPillzOutOfYSuffix.getParser(),
                RegenXMaxYSuffix.getParser(),
                ToxinXMinYSuffix.getParser(),
                XantiaxXMinYSuffix.getParser(),
            };

            List<SuffixParser> allSuffixParsers = new List<SuffixParser>();
            allSuffixParsers.AddRange(allPlainSuffixParsers);
            allSuffixParsers.AddRange(allSingleValueSuffixParsers);
            allSuffixParsers.AddRange(allDoubleValueSuffixParsers);

            PRV_ALL_SUFFIX_PARSERS = allSuffixParsers.ToArray();
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
                    string cleanText = prv_parseFillerChars(skillAsText);
                    string suffixAsText;
                    IEnumerable<Prefix> prefixes = prv_parsePrefixes(cleanText, out suffixAsText);

                    // TODO use first or default?
                    SuffixParser parser = PRV_ALL_SUFFIX_PARSERS.SingleOrDefault(p => p.isMatch(suffixAsText));

                    // TODO write alterantive when no parser gets ok
                    if (parser == null)
                    {
                        return null; // TODO remove and leave assert
                        Asserts.check(parser != null, $"No {nameof(SuffixParser)} was found for text [{suffixAsText}]");
                    }

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
        private static string prv_parseFillerChars(string text)
        {
            string cleantText = PRV_REMOVE_FILLER_CHARS.Replace(text, "");
            cleantText = cleantText.Replace(';', ':');

            return cleantText;
        }
    }
}
