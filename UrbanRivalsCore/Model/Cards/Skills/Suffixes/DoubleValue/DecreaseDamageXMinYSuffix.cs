﻿using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.DoubleValue
{
    public class DecreaseDamageXMinYSuffix : DoubleValueSuffix
    {
        private readonly static DoubleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static DecreaseDamageXMinYSuffix()
        {
            Regex regex = new Regex(@"^-(?<x>[1-9])OppD(?:amage|mg)Min(?<y>[1-9]?)$", RegexOptions.Compiled); 

            PRV_PARSER = new DoubleValueSuffixParser(regex, (x, y) => new DecreaseDamageXMinYSuffix(x, y), 141);
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_decrease_damage_x_min_y;
        }

        public DecreaseDamageXMinYSuffix(int x, int y) : base(x, y)
        {
            ;
        }

        public static DoubleValueSuffixParser getParser()
        {
            return PRV_PARSER;
        }

        public override string ToString()
        {
            return getTextRepresentation(PRV_TEXT_REPRESENTATION);
        }
    }
}