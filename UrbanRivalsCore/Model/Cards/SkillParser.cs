using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanRivalsCore.Model.Cards.Prefixes;

namespace UrbanRivalsCore.Model.Cards
{
    public static class SkillParser
    {
        private static readonly IEnumerable<Prefix> PRV_ALL_PREFIXES;

        static SkillParser()
        {
            PRV_ALL_PREFIXES = new List<Prefix>
            {
                new BacklashPrefix(),
                new CouragePrefix(),
            };
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
