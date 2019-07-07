﻿using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.Plain
{
    public class StopAbilitySuffix : Suffix
    {
        private readonly static PlainSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static StopAbilitySuffix()
        {
            Regex regex = new Regex(@"^Stop(?:Opp)?Ability$", RegexOptions.Compiled); 

            PRV_PARSER = new PlainSuffixParser(regex, new StopAbilitySuffix(), 79);
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_stop_ability;
        }

        public StopAbilitySuffix() : base()
        {
            ;
        }

        public static PlainSuffixParser getParser()
        {
            return PRV_PARSER;
        }

        public override string ToString()
        {
            return PRV_TEXT_REPRESENTATION;
        }
    }
}