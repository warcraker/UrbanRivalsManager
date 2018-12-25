using System;
using System.Collections.Generic;
using System.Linq;
using UrbanRivalsCore.Model.Cards.Skills.Leaders;
using UrbanRivalsCore.Model.Cards.Skills.Prefixes;
using UrbanRivalsCore.Model.Cards.Skills.Suffixes;

namespace UrbanRivalsCore.Model.Cards.Skills
{
    public static class SkillParser
    {
        private const string PRV_NO_ABILITY_TEXT = "No ability";
        private static readonly IEnumerable<Leader> PRV_ALL_LEADERS;
        private static readonly IEnumerable<Prefix> PRV_ALL_PREFIXES;
        private static readonly Prefix PRV_DEFAULT_PREFIX;

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
            PRV_DEFAULT_PREFIX = new DefaultPrefix();
            PRV_ALL_LEADERS = new List<Leader>
            {
                new Ambre(),
                new Ashigaru(),
                new Bridget(),
                new Eklore(),
                new Eyrik(),
                new Hugo(),
                new JohnDoom(),
                new Melody(),
                new Morphun(),
                new MrBigDuke(),
                new RobertCobb(),
                new Solomon(),
                new Timber(),
                new Vansaar(),
                new Vholt(),
            };
        }

        public static CoreSkill parseSkill(string skillAsText)
        {
            CoreSkill skill;

            if (skillAsText == PRV_NO_ABILITY_TEXT)
            {
                skill = null; // TODO
            }
            else
            {
                Leader leader = prv_parseLeader(skillAsText);
                if (leader != null)
                {
                    skill = null; // TODO
                }
                else
                {
                    IEnumerable<Prefix> prefixes;
                    Suffix suffix;
                    string suffixAsText;

                    prefixes = prv_parsePrefixes(skillAsText, out suffixAsText);
                    suffix = prv_parseSuffix(suffixAsText);
                    skill = prv_combinePrefixesAndSuffix(prefixes, suffix);
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
                parsedPrefix = PRV_ALL_PREFIXES.SingleOrDefault(prefix => prefix.isMatch(textToParse));
                if (parsedPrefix != null)
                {
                    textToParse = parsedPrefix.removePrefixFromText(textToParse);
                    prefixes.Add(parsedPrefix);
                }
            } while (parsedPrefix != null);

            textWithoutPrefixes = textToParse;

            if (prefixes.Count == 0)
            {
                prefixes.Add(PRV_DEFAULT_PREFIX);
            }

            return prefixes;
        }

        private static Suffix prv_parseSuffix(string suffixAsText)
        {
            throw new NotImplementedException();
        }

        private static CoreSkill prv_combinePrefixesAndSuffix(IEnumerable<object> prefixes, object suffix)
        {
            throw new NotImplementedException();
        }
    }
}
