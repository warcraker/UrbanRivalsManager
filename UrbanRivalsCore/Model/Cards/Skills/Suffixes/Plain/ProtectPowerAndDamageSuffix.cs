﻿using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.Plain
{
    public class ProtectPowerAndDamageSuffix : Suffix
    {
        private readonly static PlainSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static ProtectPowerAndDamageSuffix()
        {
            Regex regex = new Regex(@"^Prot(?:ection:PowerAndDamage|ecPowerAndDmg|ectPowerAndDamage|:Power&Damage)$", RegexOptions.Compiled); 

            PRV_PARSER = new PlainSuffixParser(regex, new ProtectPowerAndDamageSuffix(), 18);
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_protect_power_and_damage;
        }

        public ProtectPowerAndDamageSuffix() : base()
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
