﻿using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.DoubleValue
{
    public class ConsumeXMinYSuffix : DoubleValueSuffix
    {
        private readonly static DoubleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static ConsumeXMinYSuffix()
        {
            Regex regex = new Regex(@"^Consume(?<x>[0-9])Min(?<y>[0-9])$", RegexOptions.Compiled); 

            PRV_PARSER = new DoubleValueSuffixParser(regex, (x, y) => new ConsumeXMinYSuffix(x, y), 5);
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_consume_x_min_y;
        }

        public ConsumeXMinYSuffix(int x, int y) : base(x, y)
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