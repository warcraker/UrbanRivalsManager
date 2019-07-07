﻿using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.Plain
{
    public class CancelPillzAndLifeModifierSuffix : Suffix
    {
        private readonly static PlainSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static CancelPillzAndLifeModifierSuffix()
        {
            Regex regex = new Regex(@"^CancelOppPillz&LifeModif$", RegexOptions.Compiled);

            PRV_PARSER = new PlainSuffixParser(regex, new CancelPillzAndLifeModifierSuffix(), 7);
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_cancel_pillz_and_life_modifier;
        }

        public CancelPillzAndLifeModifierSuffix() : base()
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