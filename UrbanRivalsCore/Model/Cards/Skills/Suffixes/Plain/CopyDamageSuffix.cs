﻿
using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.Plain
{
    public class CopyDamageSuffix : Suffix
    {
        private readonly static PlainSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static CopyDamageSuffix()
        {
            Regex regex = new Regex(@"^Copy:OppDamage|Damage=DamageOpp$", RegexOptions.Compiled); 

            PRV_PARSER = new PlainSuffixParser(regex, new CopyDamageSuffix(), 26);
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_copy_damage;
        }

        public CopyDamageSuffix() : base()
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