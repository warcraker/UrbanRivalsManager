using System.Text.RegularExpressions;
using UrbanRivalsCore.Model.Cards.Skills.SuffixParsers;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.DoubleValue
{
    public class DecreasePowerAndDamageXMinYSuffix : DoubleValueSuffix
    {
        private readonly static DoubleValueSuffixParser PRV_PARSER;
        private readonly static string PRV_TEXT_REPRESENTATION;

        static DecreasePowerAndDamageXMinYSuffix()
        {
            // TODO too complex ^-(?<x>[1-9]) Opp Pow(?:er (?:&|And) Damage, M|\. (?:And Damage, M|& Dmg,m|& Damage, M|& Dam\., M))in (?<y>[1-9])$
            Regex regex = new Regex(@"^-(?<x>[1-9]) (?:Opp )?Pow(\. |er )(?:& |And )(?:D(?:mg|am\.|amage)),(?:m| M)in (?<y>[1-9])$"); 

            PRV_PARSER = new DoubleValueSuffixParser(regex, (x, y) => new DecreasePowerAndDamageXMinYSuffix(x, y));
            PRV_TEXT_REPRESENTATION = Properties.GameStrings.skill_suffix_decrease_power_and_damage_x_min_y;
        }

        public DecreasePowerAndDamageXMinYSuffix(int x, int y) : base(x, y)
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