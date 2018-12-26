﻿using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes
{
    public class CorrosionXMinY : DoubleValueSuffix
    {
        private readonly static DoubleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static CorrosionXMinY()
        {
            Regex regex = new Regex(@"^Corrosion (?<x>[0-9]+), Min (?<y>[0-9]+)$");

            PRV_PARSER = new DoubleValueSuffixParser(regex, (x, y) => new CorrosionXMinY(x, y));
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_corrosion_x_min_y;
        }

        public CorrosionXMinY(int x, int y) : base(x, y)
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