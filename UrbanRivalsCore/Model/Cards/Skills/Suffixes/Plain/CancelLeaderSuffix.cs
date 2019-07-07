﻿using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.Plain
{
    public class CancelLeaderSuffix : Suffix
    {
        private readonly static PlainSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static CancelLeaderSuffix()
        {
            Regex regex = new Regex(@"^CancelLeader$", RegexOptions.Compiled);

            PRV_PARSER = new PlainSuffixParser(regex, new CancelLeaderSuffix(), 0);
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_cancel_leader;
        }

        public CancelLeaderSuffix() : base()
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
