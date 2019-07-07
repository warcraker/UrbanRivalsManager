﻿using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.DoubleValue
{
    public class CorrosionXMinYSuffix : DoubleValueSuffix
    {
        private readonly static DoubleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static CorrosionXMinYSuffix()
        {
            Regex regex = new Regex(@"^Corrosion(?<x>[1-9])Min(?<y>[0-9])$", RegexOptions.Compiled);

            PRV_PARSER = new DoubleValueSuffixParser(regex, (x, y) => new CorrosionXMinYSuffix(x, y), 1);
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_corrosion_x_min_y;
        }

        public CorrosionXMinYSuffix(int x, int y) : base(x, y)
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