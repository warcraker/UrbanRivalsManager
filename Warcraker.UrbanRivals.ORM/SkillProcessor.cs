using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills.Prefixes;

namespace Warcraker.UrbanRivals.TextProcess
{
    public static class SkillProcessor
    {
        private const string NO_ABILITY_TEXT = "No ability";
        private static readonly Regex FILLER_CHARS_REGEX = new Regex("[ ,.]+");

        static SkillProcessor()
        {
            // TODO
        }

        public static Skill ParseSkill(string text)
        {
            throw new NotImplementedException();
            //Skill skill;

            //if (text == NO_ABILITY_TEXT)
            //{
            //    skill = Skill.NO_ABILITY;
            //}
            //else
            //{
            //    string cleanText = FILLER_CHARS_REGEX.Replace(text, "");
            //    cleanText = cleanText.Replace(';', ':');


            //    string suffixAsText;
            //    IEnumerable<Prefix> prefixes = prv_parsePrefixes(cleanText, out suffixAsText);

            //    SuffixParser parser = PRV_ALL_SUFFIX_PARSERS.FirstOrDefault(p => p.isMatch(suffixAsText));

            //    if (parser != null)
            //    {
            //        Suffix suffix = parser.getSuffix(suffixAsText);
            //        if (prefixes.Any())
            //        {
            //            skill = Skill.getSkillWithPrefixes(prefixes, suffix);
            //        }
            //        else
            //        {
            //            skill = Skill.getSkillWithoutPrefixes(suffix);
            //        }
            //    }
            //    else
            //    {
            //        Leader leader = PRV_ALL_LEADERS.FirstOrDefault(x => x.isMatch(skillAsText));

            //        if (leader != null)
            //        {
            //            skill = Skill.getLeaderSkill(leader);
            //        }
            //        else
            //        {
            //            return null; // TODO Write alternative when unknown string
            //        }
            //    }
            //}

            //return skill;
        }

        private static IEnumerable<Prefix> prv_parsePrefixes(string textToParse, out string textWithoutPrefixes)
        {
            throw new NotImplementedException();
            //List<Prefix> prefixes;
            //Prefix parsedPrefix;

            //prefixes = new List<Prefix>();
            //do
            //{
            //    parsedPrefix = PRV_ALL_PREFIXES.FirstOrDefault(prefix => prefix.isMatch(textToParse));
            //    if (parsedPrefix != null)
            //    {
            //        textToParse = parsedPrefix.removePrefixFromText(textToParse);
            //        prefixes.Add(parsedPrefix);
            //    }
            //} while (parsedPrefix != null);

            //textWithoutPrefixes = textToParse;

            //return prefixes;
        }

    }
}
