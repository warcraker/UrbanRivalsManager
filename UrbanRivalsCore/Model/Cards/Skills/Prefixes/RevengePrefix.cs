﻿using System.Text.RegularExpressions;

namespace UrbanRivalsCore.Model.Cards.Skills.Prefixes
{
    public class RevengePrefix : Prefix
    {
        private static readonly string TEXT_REPRESENTATION = Properties.GameStrings.skill_prefix_revenge;
        private static readonly Regex REGEX = new Regex("^Revenge:", RegexOptions.None);

        public override bool isMatch(string text)
        {
            return REGEX.IsMatch(text);
        }
        public override string removePrefixFromText(string text)
        {
            return REGEX.Replace(text, "");
        }
        public override string ToString()
        {
            return TEXT_REPRESENTATION;
        }
    }
}
