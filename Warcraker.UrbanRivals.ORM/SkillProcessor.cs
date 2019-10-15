using HashUtils;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills.Prefixes;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Double;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Plain;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Single;

namespace Warcraker.UrbanRivals.ORM
{
    public class SkillProcessor
    {
        private const string NO_ABILITY_TEXT = "No ability";
        private static readonly Regex FILLER_CHARS_REGEX = new Regex("[ ,.]+");
        private static readonly PrefixParser[] PREFIX_PARSERS;
        private static readonly SuffixParser[] SUFFIX_PARSERS;
        private static readonly RegexOptions OPTIONS = RegexOptions.None;

        static SkillProcessor()
        {
            // TODO Leader parsers
            // TODO add suffix parsers
            // TODO assert number of prefix/suffix's created by reflection

            PREFIX_PARSERS = new PrefixParser[]
            {
                new PrefixParser(new BacklashPrefix(), new Regex("^Backlash:", OPTIONS)),
                new PrefixParser(new BrawlPrefix(), new Regex("^Brawl:", OPTIONS)),
                new PrefixParser(new ConfidencePrefix(), new Regex("^Confid(?:ence)?:", OPTIONS)),
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
                new PrefixParser(new VictoryOrDefeatPrefix(), new Regex(@"^Vict(?:ory)?OrDef(?:eat)?:", OPTIONS)),
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
    }
}
