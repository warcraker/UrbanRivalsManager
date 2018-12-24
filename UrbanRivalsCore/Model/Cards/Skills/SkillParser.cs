using System;
using System.Collections.Generic;
using System.Linq;
using UrbanRivalsCore.Model.Cards.Skills.Prefixes;

namespace UrbanRivalsCore.Model.Cards.Skills
{
    public static class SkillParser
    {
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
        }

        public static Skill parseSkill(string skillAsText)
        {
            Skill skill;
            IEnumerable<Prefix> prefixes;
            object suffix;
            string suffixAsText;

            prefixes = prv_parsePrefixes(skillAsText, out suffixAsText);
            suffix = prv_parseSuffix(suffixAsText);
            skill = prv_combinePrefixesAndSuffix(prefixes, suffix);

            return skill;
        }

        private static IEnumerable<Prefix> prv_parsePrefixes(string textToParse, out string textWithoutPrefixes)
        {
            List<Prefix> prefixes;
            Prefix parsedPrefix;

            prefixes = new List<Prefix>();
            do
            {
                string textWithoutParsedPrefix;

                parsedPrefix = prv_parseSinglePrefix(textToParse, out textWithoutParsedPrefix);
                if (parsedPrefix != null)
                {
                    prefixes.Add(parsedPrefix);
                    textToParse = textWithoutParsedPrefix;
                }
            } while (parsedPrefix != null);

            if (prefixes.Count == 0)
            {
                prefixes.Add(PRV_DEFAULT_PREFIX);
            }

            textWithoutPrefixes = textToParse;

            return prefixes;
        }

        private static Prefix prv_parseSinglePrefix(string textToParse, out string textWithoutFoundPrefix)
        {
            Prefix parsedPrefix;

            parsedPrefix = PRV_ALL_PREFIXES.SingleOrDefault(prefix => prefix.isMatch(textToParse));
            if (parsedPrefix == null)
            {
                textWithoutFoundPrefix = textToParse;
            }
            else
            {
                textWithoutFoundPrefix = parsedPrefix.removePrefixFromText(textToParse);
            }

            return parsedPrefix;
        }

        private static object prv_parseSuffix(string suffixAsText)
        {
            throw new NotImplementedException();
        }

        private static Skill prv_combinePrefixesAndSuffix(IEnumerable<object> prefixes, object suffix)
        {
            throw new NotImplementedException();
        }
    }
}
