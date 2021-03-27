﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills.Leaders;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills.Prefixes;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Double;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Plain;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Single;
using Warcraker.Utils;

namespace Warcraker.UrbanRivals.ORM
{
    public class SkillProcessor
    {
        private const string NO_ABILITY_TEXT = "No ability";
        private static readonly Regex FILLER_CHARS_REGEX = new Regex("[ ,.]+");
        private static readonly PrefixParser[] PREFIX_PARSERS;
        private static readonly SuffixParser[] SUFFIX_PARSERS;
        private static readonly LeaderParser[] LEADER_PARSERS;
        private static readonly RegexOptions OPTIONS = RegexOptions.None;

        static SkillProcessor()
        {
            // TODO Leader parsers
            // TODO add suffix parsers
            // TODO assert number of prefix/suffix's created by reflection

            PREFIX_PARSERS = new PrefixParser[]
            {
                new PrefixParser(new AsymmetryPrefix(), new Regex("^Asymmetry:", OPTIONS)),
                new PrefixParser(new BacklashPrefix(), new Regex("^Backlash:", OPTIONS)),
                new PrefixParser(new BrawlPrefix(), new Regex("^Brawl:", OPTIONS)),
                new PrefixParser(new ConfidencePrefix(), new Regex("^Conf(?:id(?:ence)?)?:", OPTIONS)),
                new PrefixParser(new CouragePrefix(), new Regex("^Courage:", OPTIONS)),
                new PrefixParser(new DayPrefix(), new Regex("^Day:", OPTIONS)),
                new PrefixParser(new DefeatPrefix(), new Regex("^Defeat:", OPTIONS)),
                new PrefixParser(new DegrowthPrefix(), new Regex("^Degrowth:", OPTIONS)),
                new PrefixParser(new EqualizerPrefix(), new Regex("^Equalizer:", OPTIONS)),
                new PrefixParser(new GrowthPrefix(), new Regex("^Growth:", OPTIONS)),
                new PrefixParser(new KillshotPrefix(), new Regex("^Killshot:", OPTIONS)),
                new PrefixParser(new NightPrefix(), new Regex("^Night:", OPTIONS)),
                new PrefixParser(new ReprisalPrefix(), new Regex("^Reprisal:", OPTIONS)),
                new PrefixParser(new RevengePrefix(), new Regex("^Revenge:", OPTIONS)),
                new PrefixParser(new StopPrefix(), new Regex("^Stop:", OPTIONS)),
                new PrefixParser(new SupportPrefix(), new Regex("^Support:", OPTIONS)),
                new PrefixParser(new SymmetryPrefix(), new Regex("^Symmetry:", OPTIONS)),
                new PrefixParser(new VictoryOrDefeatPrefix(), new Regex(@"^Vict(?:ory)?OrDef(?:eat)?:", OPTIONS)),
            };

            SuffixParser[] doubleValueSuffixParsers = new SuffixParser[]
            {
                new SuffixParser((x, y) => new CombustXMinYSuffix(x, y), new Regex(@"^Combust(?<x>[0-9])Min(?<y>[0-9])$", OPTIONS)),
                new SuffixParser((x, y) => new ConsumeXMinYSuffix(x, y), new Regex(@"^Consume(?<x>[0-9])Min(?<y>[0-9])$", OPTIONS)),
                new SuffixParser((x, y) => new CorrosionXMinYSuffix(x, y), new Regex(@"^Corrosion(?<x>[1-9])Min(?<y>[0-9])$", OPTIONS)),
                new SuffixParser((x, y) => new CorruptXMinYSuffix(x, y), new Regex(@"^Corrupt(?<x>[0-9])Min(?<y>[0-9])", OPTIONS)),
                new SuffixParser((x, y) => new DecreaseAttackXMinYSuffix(x, y), new Regex(@"^-(?<x>[1-9][0-9]?)OppAttackMin(?<y>[1-9][0-9]?)$", OPTIONS)),
                new SuffixParser((x, y) => new DecreaseAttackXPerRemainingLifeMinYSuffix(x, y), new Regex(@"^-(?<x>[1-9])OppAttPerLifeLeftMin(?<y>[1-9]?[0-9])$", OPTIONS)),
                new SuffixParser((x, y) => new DecreaseAttackXPerRemainingPillzMinYSuffix(x, y), new Regex(@"^-(?<x>[1-9])OppAttPerPillzLeftMin(?<y>[1-9]?[0-9])$", OPTIONS)),
                new SuffixParser((x, y) => new DecreaseDamageXMinYSuffix(x, y), new Regex(@"^-(?<x>[1-9])OppD(?:amage|mg)Min(?<y>[0-9]?)$", OPTIONS)),
                new SuffixParser((x, y) => new DecreaseLifeAndPillzXMinYSuffix(x, y), new Regex(@"^-(?<x>[1-9])(?:Opp)?(?:Life&Pillz|Pillz(?:&|And)Life)Min(?<y>[0-9])$", OPTIONS)),
                new SuffixParser((x, y) => new DecreaseLifeXMinYSuffix(x, y), new Regex(@"^-(?<x>[1-9])(?:Opp)?LifeMin(?<y>[0-9])$", OPTIONS)),
                new SuffixParser((x, y) => new DecreasePillzXMinYSuffix(x, y), new Regex(@"^-(?<x>[1-9])(?:OppPillz|Pillz(?:Opp)?)Min(?<y>[0-9])$", OPTIONS)),
                new SuffixParser((x, y) => new DecreasePowerAndDamageXMinYSuffix(x, y), new Regex(@"^-(?<x>[1-9])(?:Opp)?Pow(?:er)?(?:&|And)D(?:amageM|amM|mgm)in(?<y>[0-9])$", OPTIONS)),
                new SuffixParser((x, y) => new DecreasePowerXMinYSuffix(x, y), new Regex(@"^-(?<x>[1-9])OppPowerMin(?<y>[0-9])$", OPTIONS)),
                new SuffixParser((x, y) => new DopeAndRegenXMaxYSuffix(x, y), new Regex(@"^Dope\+Regen(?<x>[1-9])Max(?<y>[1-9]?[0-9])$", OPTIONS)),
                new SuffixParser((x, y) => new DopeXMaxYSuffix(x, y), new Regex(@"^Dope(?<x>[1-9])Max(?<y>[1-9]?[0-9])$", OPTIONS)),
                new SuffixParser((x, y) => new HealXMaxYSuffix(x, y), new Regex(@"^Heal(?<x>[1-9])Max(?<y>[1-9]?[0-9])$", OPTIONS)),
                new SuffixParser((x, y) => new IncreaseLifePerDamageXMaxYSuffix(x, y), new Regex(@"^\+(?<x>[1-9])LifePerDamageMax(?<y>[1-9]?[0-9])$", OPTIONS)),
                new SuffixParser((x, y) => new IncreaseLifeXMaxYSuffix(x, y), new Regex(@"^\+(?<x>[1-9])LifeMax(?<y>[1-9]?[0-9])$", OPTIONS)),
                new SuffixParser((x, y) => new IncreasePillzPerDamageXMaxYSuffix(x, y), new Regex(@"^\+(?<x>[1-9])PillzPerDamageMax(?<y>[1-9]?[0-9])$", OPTIONS)),
                new SuffixParser((x, y) => new IncreasePillzXMaxYSuffix(x, y), new Regex(@"^\+(?<x>[1-9])PillzMax(?<y>[1-9]?[0-9])$", OPTIONS)),
                new SuffixParser((x, y) => new IncreasePowerPerLifeLeftXMaxYSuffix(x, y), new Regex(@"^\+(?<x>[0-9])PowerPerLifeLeftMax(?<y>[1-9]?[0-9])$", OPTIONS)),
                new SuffixParser((x, y) => new InfectionXMinYSuffix(x, y), new Regex(@"^Infection(?<x>[1-9])Min(?<y>[0-9])$", OPTIONS)),
                new SuffixParser((x, y) => new MindwipeXMinYSuffix(x, y), new Regex(@"^Mindwipe(?<x>[0-9])Min(?<y>[0-9])$", OPTIONS)),
                new SuffixParser((x, y) => new PoisonXMinYSuffix(x, y), new Regex(@"^Poison(?<x>[1-9])Min(?<y>[0-9])$", OPTIONS)),
                new SuffixParser((x, y) => new RebirthXMaxYSuffix(x, y), new Regex(@"^Rebirth(?<x>[1-9])Max(?<y>[0-9])$", OPTIONS)),
                new SuffixParser((x, y) => new RecoverXPillzOutOfYSuffix(x, y), new Regex(@"^Recover(?<x>[1-9])PillzOutOf(?<y>[0-9])$", OPTIONS)),
                new SuffixParser((x, y) => new RegenXMaxYSuffix(x, y), new Regex(@"^Regen(?<x>[1-9])Max(?<y>[1-9]?[0-9])$", OPTIONS)),
                new SuffixParser((x, y) => new RepairXMaxYSuffix(x, y), new Regex(@"^Repair(?<x>[0-9])Max(?<y>[1-9]?[0-9])$", OPTIONS)),
                new SuffixParser((x, y) => new ToxinXMinYSuffix(x, y), new Regex(@"^Toxin(?<x>[1-9])Min(?<y>[0-9])$", OPTIONS)),
                new SuffixParser((x, y) => new XantiaxXMinYSuffix(x, y), new Regex(@"^Xantiax:-(?<x>[1-9])LifeMin(?<y>[0-9])$", OPTIONS)),
            };
            SuffixParser[] singleValueSuffixParsers = new SuffixParser[]
            {
                new SuffixParser((x, y) => new IncreaseAttackXPerOppDamageSuffix(x), new Regex(@"^\+(?<x>[1-9])AttackPerOppDamage$", OPTIONS)),
                new SuffixParser((x, y) => new IncreaseAttackXPerOppPowerSuffix(x), new Regex(@"^\+(?<x>[1-9])AttackPerOppPower$", OPTIONS)),
                new SuffixParser((x, y) => new IncreaseAttackXPerRemainingLifeSuffix(x), new Regex(@"^\+(?<x>[1-9])At(?:tac)?kPerLifeLeft$", OPTIONS)),
                new SuffixParser((x, y) => new IncreaseAttackXPerRemainingPillzSuffix(x), new Regex(@"^\+(?<x>[1-9])At(?:tac)?kPerPillzLeft$", OPTIONS)),
                new SuffixParser((x, y) => new IncreaseAttackXSuffix(x), new Regex(@"^At(?:tac)?k\+(?<x>[1-9][0-9]?)$", OPTIONS)),
                new SuffixParser((x, y) => new IncreaseDamageXSuffix(x), new Regex(@"^Damage\+(?<x>[1-9])$", OPTIONS)),
                new SuffixParser((x, y) => new IncreaseLifeXPerDamageSuffix(x), new Regex(@"^\+(?<x>[1-9])LifePerD(?:amage|mg)$", OPTIONS)),
                new SuffixParser((x, y) => new IncreaseLifeXSuffix(x), new Regex(@"^\+(?<x>[1-9])(?:Opp)?Life$", OPTIONS)),
                new SuffixParser((x, y) => new IncreasePillzAndLifeXSuffix(x), new Regex(@"^\+(?<x>[1-9])PillzAndLife$", OPTIONS)),
                new SuffixParser((x, y) => new IncreasePillzXPerDamageSuffix(x), new Regex(@"^\+(?<x>[1-9])PillzPerDamage$", OPTIONS)),
                new SuffixParser((x, y) => new IncreasePillzXSuffix(x), new Regex(@"^\+(?<x>[1-9])Pillz$", OPTIONS)),
                new SuffixParser((x, y) => new IncreasePowerAndDamageXSuffix(x), new Regex(@"^Power(?:And|&)Damage\+(?<x>[1-9])$", OPTIONS)),
                new SuffixParser((x, y) => new IncreasePowerXSuffix(x), new Regex(@"^Power\+(?<x>[1-9])$", OPTIONS)),
                new SuffixParser((x, y) => new ReanimateXSuffix(x), new Regex(@"^Reanimate:\+(?<x>[1-9])Life$", OPTIONS)),
            };
            SuffixParser[] plainSuffixParsers = new SuffixParser[]
            {
                new SuffixParser((x, y) => new CancelAttackModifierSuffix(), new Regex(@"^CancelOppAttackModif$", OPTIONS)),
                new SuffixParser((x, y) => new CancelDamageModifierSuffix(), new Regex(@"^CancelOppDamageModif$", OPTIONS)),
                new SuffixParser((x, y) => new CancelLeaderSuffix(), new Regex(@"^CancelLeader$", OPTIONS)),
                new SuffixParser((x, y) => new CancelLifeModifierSuffix(), new Regex(@"^CancelOppLifeModif$", OPTIONS)),
                new SuffixParser((x, y) => new CancelPillzAndLifeModifierSuffix(), new Regex(@"^CancelOppPillz&LifeModif$", OPTIONS)),
                new SuffixParser((x, y) => new CancelPillzModifierSuffix(), new Regex(@"^CancelOppPillzModif$", OPTIONS)),
                new SuffixParser((x, y) => new CancelPowerAndDamageModifierSuffix(), new Regex(@"^Canc(?:Power&?DamMod|elOppPower(?:&DamageMod|AndDamageModif))$", OPTIONS)),
                new SuffixParser((x, y) => new CancelPowerModifierSuffix(), new Regex(@"^CancelOppPowerModif$", OPTIONS)),
                new SuffixParser((x, y) => new CopyBonusSuffix(), new Regex(@"^Copy:?OppBonus$", OPTIONS)),
                new SuffixParser((x, y) => new CopyDamageSuffix(), new Regex(@"^Copy:OppDamage|Damage=DamageOpp$", OPTIONS)),
                new SuffixParser((x, y) => new CopyPowerAndDamageSuffix(), new Regex(@"^Copy:PowerAndDamageOpp$", OPTIONS)),
                new SuffixParser((x, y) => new CopyPowerSuffix(), new Regex(@"^Copy:OppPower|Power=PowerOpp$", OPTIONS)),
                new SuffixParser((x, y) => new ExchangeDamageSuffix(), new Regex(@"^DamageExchange$", OPTIONS)),
                new SuffixParser((x, y) => new ExchangePowerAndDamageSuffix(), new Regex(@"^PowerAndDamageExchange$", OPTIONS)),
                new SuffixParser((x, y) => new ExchangePowerSuffix(), new Regex(@"^PowerExchange$", OPTIONS)),
                new SuffixParser((x, y) => new ProtectAbilitySuffix(), new Regex(@"^Protection:Ability$", OPTIONS)),
                new SuffixParser((x, y) => new ProtectAttackSuffix(), new Regex(@"^Protection:Attack$", OPTIONS)),
                new SuffixParser((x, y) => new ProtectBonusSuffix(), new Regex(@"^BonusProtection|Protection:Bonus$", OPTIONS)),
                new SuffixParser((x, y) => new ProtectDamageSuffix(), new Regex(@"^Protection:Damage$", OPTIONS)),
                new SuffixParser((x, y) => new ProtectPowerAndDamageSuffix(), new Regex(@"^Prot(?:ection:PowerAndDamage|ecPowerAndDmg|ectPowerAndDamage|:Power&Damage)$", OPTIONS)),
                new SuffixParser((x, y) => new ProtectPowerSuffix(), new Regex(@"^Protection:Power$", OPTIONS)),
                new SuffixParser((x, y) => new StopAbilitySuffix(), new Regex(@"^Stop(?:Opp)?Ability$", OPTIONS)),
                new SuffixParser((x, y) => new StopBonusSuffix(), new Regex(@"^Stop(?:Opp)?Bonus$", OPTIONS)),
            };

            SUFFIX_PARSERS = doubleValueSuffixParsers
                .Concat(singleValueSuffixParsers)
                .Concat(plainSuffixParsers)
                .ToArray();

            LEADER_PARSERS = new LeaderParser[]
            {
                new LeaderParser((x, y) => new AmbreLeader(x), new Regex(@"^Team:Courage:Power\+(?<x>[0-9])$", OPTIONS)),
                new LeaderParser((x, y) => new AshigaruLeader(), new Regex(@"^Counter-attack$", OPTIONS)),
                new LeaderParser((x, y) => new BridgetLeader(x), new Regex(@"^Team:VictoryOrDefeat:\+(?<x>[0-9])Life$", OPTIONS)),
                new LeaderParser((x, y) => new EkloreLeader(x, y), new Regex(@"^-(?<x>[0-9])OppPillzPerRoundMin(?<y>[0-9])$", OPTIONS)),
                new LeaderParser((x, y) => new EyrikLeader(x, y), new Regex(@"^Team:-(?<x>[0-9])OppPowerMin(?<y>[0-9])$", OPTIONS)),
                new LeaderParser((x, y) => new KateLeader(), new Regex(@"^Illusion$", OPTIONS)),
                new LeaderParser((x, y) => new HugoLeader(x), new Regex(@"^Team:\+(?<x>[0-9])Attack$", OPTIONS)),
                new LeaderParser((x, y) => new JonhDoomLeader(x, y), new Regex(@"^Team:Reprisal:-(?<x>[0-9])Pow&DmgMin(?<y>[0-9])$", OPTIONS)),
                new LeaderParser((x, y) => new MelodyLeader(x, y), new Regex(@"^Team:Defeat:Rec(?<x>[0-9])PillzOutOf(?<y>[0-9])$", OPTIONS)),
                new LeaderParser((x, y) => new MementoLeader(), new Regex(@"^RemoveAbilityConditions$", OPTIONS)),
                new LeaderParser((x, y) => new MorphunLeader(x, y), new Regex(@"^\+(?<x>[0-9])PillzPerRoundMax(?<y>[1-9]?[0-9])$", OPTIONS)),
                new LeaderParser((x, y) => new MrBigDukeLeader(), new Regex(@"^Team:Protection:Damage$", OPTIONS)),
                new LeaderParser((x, y) => new RobertCobbLeader(), new Regex(@"^Bypass$", OPTIONS)),
                new LeaderParser((x, y) => new SolomonLeader(), new Regex(@"^Tie-break$", OPTIONS)),
                new LeaderParser((x, y) => new TimberLeader(x), new Regex(@"^Team:\+(?<x>[0-9])Damage$", OPTIONS)),
                new LeaderParser((x, y) => new VansarLeader(x), new Regex(@"^Team:Xp\+(?<x>[0-9]{2,3})%$", OPTIONS)),
                new LeaderParser((x, y) => new VholtLeader(x, y), new Regex(@"^Team:-(?<x>[0-9])OppDamageMin(?<y>[0-9])$", OPTIONS)),
            };
        }

        public static Skill ParseSkill(string text)
        {
            Skill skill;

            if (text == NO_ABILITY_TEXT)
            {
                skill = NoAbility.INSTANCE;
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
                    LeaderParser leaderParser = LEADER_PARSERS.FirstOrDefault(p => p.IsMatch(suffixAsText));

                    if (leaderParser != null)
                    {
                        Leader leader = leaderParser.ParseLeader(suffixAsText);
                        skill = new Skill(new Prefix[0], leader);
                    }
                    else
                    {
                        skill = UnknownSkill.INSTANCE;
                        Asserts.Fail($"Unable to parse skill: {text}");
                    }
                }
            }

            return skill;
        }

        private static IEnumerable<Prefix> ParsePrefixes(string textToParse, out string textWithoutPrefixes)
        {
            string text = textToParse;
            var prefixes = new List<Prefix>();
            PrefixParser parser;

            do
            {
                parser = PREFIX_PARSERS.FirstOrDefault(prefix => prefix.IsMatch(text));
                if (parser != null)
                {
                    text = parser.RemovePrefixFromText(text);
                    prefixes.Add(parser.Prefix);
                }
            } while (parser != null);

            textWithoutPrefixes = text;
            return prefixes;
        }
    }
}
