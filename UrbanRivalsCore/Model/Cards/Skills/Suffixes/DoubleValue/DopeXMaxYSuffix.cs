﻿using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.DoubleValue
{
    public class DopeXMaxYSuffix : DoubleValueSuffix
    {
        private readonly static DoubleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static DopeXMaxYSuffix()
        {
            Regex regex = new Regex(@"^Dope(?<x>[1-9])Max(?<y>[1-9][0-9]?)$", RegexOptions.Compiled);

            PRV_PARSER = new DoubleValueSuffixParser(regex, (x, y) => new DopeXMaxYSuffix(x, y), 7);
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_dope_x_max_y;
        }

        public DopeXMaxYSuffix(int x, int y) : base(x, y)
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