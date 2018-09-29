using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UrbanRivalsCore.Model;

namespace UrbanRivalsApiAdapter
{
    /// <summary>
    /// Creates a <see cref="CardBase"/> instance out of server data.
    /// </summary>
    public static class ApiToCardBaseAdapter
    {
        private static class UsedRegex
        {
            public static readonly Regex PrefixAndSuffix =
                new Regex(@"^(?<prefix>Backlash|Brawl|Confidence|Courage|Defeat|Degrowth|Equalizer|Growth|Killshot|Reprisal|Revenge|Stop|Support|Victory Or Defeat|Vict[.] Or Def[.]) ?: (?<suffix>[a-zA-Z0-9 .,:=&+-]+)$");
            public static class Suffix
            {
                public static readonly Regex CancelAttackModifier           = new Regex(@"^Cancel Opp[.] Attack Modif[.]$");
                public static readonly Regex CancelDamageModifier           = new Regex(@"^Cancel Opp[.] Damage Modif[.]$");
                public static readonly Regex CancelLifeModifier             = new Regex(@"^Cancel Opp[.] Life Modif[.]$");
                public static readonly Regex CancelPillzAndLifeModifier     = new Regex(@"^Cancel Opp[.] Pillz & Life Modif[.]$");
                public static readonly Regex CancelPillzModifier            = new Regex(@"^Cancel Opp[.] Pillz Modif[.]$");
                public static readonly Regex CancelPowerAndDamageModifier   = new Regex(@"^Cancel Opp[.]? Power And Damage( Modif[.]| Mod)?$");
                public static readonly Regex CancelPowerModifier            = new Regex(@"^Cancel Opp[.] Power Modif[.]$");

                public static readonly Regex ConsumeXMinY = new Regex(@"^Consume (?<x>[0-9]+), Min (?<y>[0-9]+)$");

                public static readonly Regex CopyBonus          = new Regex(@"^Copy:? (Bonus Opp[.]|Opp[.] Bonus)?$");
                public static readonly Regex CopyDamage         = new Regex(@"^Copy: Opp[.] Damage$");
                public static readonly Regex CopyPower          = new Regex(@"^Copy: Opp[.] Power|Power = Power Opp$");
                public static readonly Regex CopyPowerAndDamage = new Regex(@"^Copy: Power And Damage Opp[.]?$");

                public static readonly Regex DecreaseAttackXMinY                    = new Regex(@"^- ?(?<x>[0-9]+) (Opp[.]? )?Attack,? Min (?<y>[0-9]+)$");
                public static readonly Regex DecreaseAttackXPerRemainingLifeMinY    = new Regex(@"^- ?(?<x>[0-9]+) (Opp[.]? )?(Attack|Att[.]) Per Life Left,? Min (?<y>[0-9]+)$");
                public static readonly Regex DecreaseAttackXPerRemainingPillzMinY   = new Regex(@"^-(?<x>[0-9]+) Opp Att[.] Per Pillz Left, Min (?<y>[0-9]+)$");
                public static readonly Regex DecreaseDamageXMinY                    = new Regex(@"^- ?(?<x>[0-9]+) (Opp[.]? )?D(?:amage|mg),? Min (?<y>[0-9]+)$");
                public static readonly Regex DecreaseLifeAndPillzXMinY              = new Regex(@"^-(?<x>[0-9]+) Opp( Life & Pillz Min|[.] Pillz And Life, Min) (?<y>[0-9]+)$");
                public static readonly Regex DecreaseLifeXMinY                      = new Regex(@"^- ?(?<x>[0-9]+) (Opp[.]? )?Life[.,]? (Opp[.]? )?Min (?<y>[0-9]+)$");
                public static readonly Regex DecreasePillzXMinY                     = new Regex(@"^- ?(?<x>[0-9]+) (Opp[.]? )?Pillz[.,]? (Opp[.]? )?Min (?<y>[0-9]+)$");
                public static readonly Regex DecreasePowerAndDamageXMinY            = new Regex(@"^- ?(?<x>[0-9]+) (Opp[.]? )?Pow(er)?[.]? (And|&) D(amage|mg|am)[.]?,? ?[Mm]in (?<y>[0-9]+)$");
                public static readonly Regex DecreasePowerXMinY                     = new Regex(@"^- ?(?<x>[0-9]+) (Opp[.]? )?Pow(er)?[,.]? Min (?<y>[0-9]+)$");

                public static readonly Regex DopeXMaxY = new Regex(@"^Dope (?<x>[0-9]), Max[.] (?<y>[0-9]+)$");

                public static readonly Regex ExchangeDamage = new Regex(@"^Damage Exchange$");
                public static readonly Regex ExchangePower  = new Regex(@"^Power Exchange$");

                public static readonly Regex HealXMaxY = new Regex(@"^Heal (?<x>[0-9]+) Max[.]? (?<y>[0-9]+)$");

                public static readonly Regex IncreaseAttackX                    = new Regex(@"^At(tac)?k[.]? [+](?<x>[0-9]+)$");
                public static readonly Regex IncreaseAttackXPerRemainingLife    = new Regex(@"^[+] ?(?<x>[0-9]+) At(tac)?k Per Life Left$");
                public static readonly Regex IncreaseAttackXPerRemainingPillz   = new Regex(@"^[+] ?(?<x>[0-9]+) At(tac)?k Per Pillz Left$");
                public static readonly Regex IncreaseDamageX                    = new Regex(@"^D(amage|mg[.]?) [+] ?(?<x>[0-9]+)$");
                public static readonly Regex IncreaseLifeX                      = new Regex(@"^[+] ?(?<x>[0-9]+) Life$");
                public static readonly Regex IncreaseLifeXMaxY                  = new Regex(@"^[+] ?(?<x>[0-9]+) Life[,]? Max[.] (?<y>[0-9]+)$");
                public static readonly Regex IncreaseLifeXPerDamage             = new Regex(@"^[+] ?(?<x>[0-9]+) Life Per D(amage|mg[.]?)$");
                public static readonly Regex IncreaseLifeXPerDamageMaxY         = new Regex(@"^[+] ?(?<x>[0-9]+) Life Per D(amage|mg[.]?) Max[.] (?<y>[0-9]+)$");
                public static readonly Regex IncreasePillzAndLifeX              = new Regex(@"^[+](?<x>[0-9]+) Pillz And Life$");
                public static readonly Regex IncreasePillzX                     = new Regex(@"^[+] ?(?<x>[0-9]+) Pillz$");
                public static readonly Regex IncreasePillzXMaxY                 = new Regex(@"^[+] ?(?<x>[0-9]+) Pillz Max[.] (?<y>[0-9]+)$");
                public static readonly Regex IncreasePillzXPerDamage            = new Regex(@"^[+] ?(?<x>[0-9]+) Pillz Per D(amage|mg[.]?)$");
                public static readonly Regex IncreasePowerAndDamageX            = new Regex(@"^Pow(er[.]?) (And|&) D(amage|mg[.]?) ?[+] ?(?<x>[0-9]+)$");
                public static readonly Regex IncreasePowerX                     = new Regex(@"^Pow(er[.]?) [+] ?(?<x>[0-9]+)$");

                public static readonly Regex InfectionXMinY = new Regex(@"^Infection (?<x>[0-9]), Min (?<y>[0-9])$");

                public static readonly Regex PoisonXMinY = new Regex(@"^Poison (?<x>[0-9]),? Min (?<y>[0-9])$");

                public static readonly Regex ProtectAbility         = new Regex(@"^Protection ?: Ability$");
                public static readonly Regex ProtectAttack          = new Regex(@"^Protection ?: Attack$");
                public static readonly Regex ProtectBonus           = new Regex(@"^Protection ?: Bonus|Bonus Protection$");
                public static readonly Regex ProtectDamage          = new Regex(@"^Protection ?: Damage$");
                public static readonly Regex ProtectPower           = new Regex(@"^Protection ?: Power$");
                public static readonly Regex ProtectPowerAndDamage  = new Regex(@"^Prot(ection|tion|ec[.]|t[.]|[.])(:| :)? Pow(er|[.]) (And|&) D(amage|mg[.]?)$");

                public static readonly Regex RecoverXPillzOutOfY = new Regex(@"^Recover (?<x>[0-9]) Pillz Out Of (?<y>[0-9])$");

                public static readonly Regex RegenXMaxY = new Regex(@"^Regen (?<x>[0-9]),? Max[.]? (?<y>[0-9]+)$");

                public static readonly Regex StopAbility    = new Regex(@"^Stop( Opp[.]?)? Ability$");
                public static readonly Regex StopBonus      = new Regex(@"^Stop( Opp[.]?)? Bonus$");

                public static readonly Regex ToxinXMinY = new Regex(@"^Toxin (?<x>[0-9]),? Min (?<y>[0-9])$");
            }
        }

        private static CardRarity ParseRarity(string rarityString)
        {
            switch (rarityString)
            {
                case "c":
                    return CardRarity.Common;
                case "u":
                    return CardRarity.Uncommon;
                case "r":
                    return CardRarity.Rare;
                case "l":
                    return CardRarity.Legendary;
                case "cr":
                    return CardRarity.Collector;
                case "rb":
                    return CardRarity.Rebirth;
                case "m":
                    return CardRarity.Mythic;
                default:
                    throw new Exception("Rarity has a not defined value = " + rarityString);
            }
        }
        private static DateTime ParseReleaseDate(int date)
        {
            // The server gives the seconds since 01/01/1970
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds((double)date);
        }
        private static Skill ParseAbility(string abilityString)
        {
            if (abilityString == "No ability")
                return Skill.NoAbility;

            // -- Exceptional cases --

            // There are some cases that hold enough inconsistency that justify exceptional cases in the parser

            // Exceptional case: Balorg (ID = 1195). Every card with a prefix has a colon (:) after the prefix. This one has a semicolon (;)
            // Why: The alternative is add an extra case on the regex "PrefixAndSuffix"
            if (abilityString == "Courage; -2 Opp Pillz. Min 1")
                return new Skill(SkillPrefix.Courage, SkillSuffix.DecreasePillzXMinY, 2, 1);

            // Exceptional case: DJ Korps (ID = 1260). Every card has a single prefix, or none. This one has a double prefix
            // Why: The alternative is add a loop that calls (at least) two times the regex "PrefixAndSuffix" for each parse
            if (abilityString == "Defeat: Growth: -1 Opp. Life, Min 1")
                return new Skill(SkillPrefix.GrowthAndDefeat, SkillSuffix.DecreaseLifeXMinY, 1, 1);

            // Exceptional case: Excess LD (ID = 1678). Every card has a single prefix, or none. This one has a double prefix
            // Why: The alternative is add a loop that calls (at least) two times the regex "PrefixAndSuffix" for each parse
            if (abilityString == "Stop: Victory Or Defeat : +3 Life")
                return null;

            // Exceptional case: Ghoonbones (ID = 1755). Every card has a single prefix, or none. This one has a double prefix
            // Why: The alternative is add a loop that calls (at least) two times the regex "PrefixAndSuffix" for each parse
            if (abilityString == "Backlash: Growth: - 1 Life Min 2") 
                return null;

            // Exceptional case: Hydraereva (ID = 1619). Every card has a single prefix, or none. This one has a double prefix
            // Why: The alternative is add a loop that calls (at least) two times the regex "PrefixAndSuffix" for each parse
            if (abilityString == "Defeat: Equalizer: +1 Life")
                return null;
            

            // -- End Exceptional cases --

            // Leaders 
            switch (abilityString)
            {
                case "Team: Courage, Power +3, Max. 10":
                    return new Skill(SkillLeader.Ambre);
                case "Counter-attack":
                    return new Skill(SkillLeader.Ashigaru);
                case "+1 Life Per Round":
                    return new Skill(SkillLeader.Bridget);
                case "-1 Opp. Pillz, Per Round, Min 3":
                    return new Skill(SkillLeader.Eklore);
                case "Team: -1 Opp. Power, Min 5":
                    return new Skill(SkillLeader.Eyrik);
                case "Team: +6 Attack":
                    return new Skill(SkillLeader.Hugo);
                case "Team: Defeat: Rec. 1 Pillz Out Of 3":
                    return new Skill(SkillLeader.Melody);
                case "+1 Pillz Per Round, Max. 10":
                    return new Skill(SkillLeader.Morphun);
                case "Tie-break":
                    return new Skill(SkillLeader.Solomon);
                case "Team: +1 Damage":
                    return new Skill(SkillLeader.Timber);
                case "Team: Xp +90%":
                    return new Skill(SkillLeader.Vansaar);
                case "Team: -2 Opp. Damage, Min 4":
                    return new Skill(SkillLeader.Vholt);
            }

            string prefixText, suffixText;
            SkillPrefix prefix = SkillPrefix.None;
            SkillSuffix suffix = SkillSuffix.None;
            string x = "0";
            string y = "0";

            var match = UsedRegex.PrefixAndSuffix.Match(abilityString);

            if (match.Groups["prefix"].Captures.Count != 0)
            {
                // This ability has both prefix and suffix
                prefixText = match.Groups["prefix"].Captures[0].Value;
                suffixText = match.Groups["suffix"].Captures[0].Value;
            }
            else
            {
                // This skill only has suffix
                prefixText = "";
                suffixText = abilityString;
            }

            // -- Prefix --
            switch (prefixText)
            {
                case "Backlash":
                    prefix = SkillPrefix.Backlash;
                    break;
                case "Brawl":
                    prefix = SkillPrefix.Brawl;
                    break;
                case "Confidence":
                    prefix = SkillPrefix.Confidence;
                    break;
                case "Courage":
                    prefix = SkillPrefix.Courage;
                    break;
                case "Defeat":
                    prefix = SkillPrefix.Defeat;
                    break;
                case "Degrowth":
                    prefix = SkillPrefix.Degrowth;
                    break;
                case "Equalizer":
                    prefix = SkillPrefix.Equalizer;
                    break;
                case "Growth":
                    prefix = SkillPrefix.Growth;
                    break;
                case "Killshot":
                    prefix = SkillPrefix.Killshot;
                    break;
                case "Reprisal":
                    prefix = SkillPrefix.Reprisal;
                    break;
                case "Revenge":
                    prefix = SkillPrefix.Revenge;
                    break;
                case "Stop":
                    prefix = SkillPrefix.Stop;
                    break;
                case "Support":
                    prefix = SkillPrefix.Support;
                    break;
                case "Victory Or Defeat":
                case "Vict. Or Def.":
                    prefix = SkillPrefix.VictoryOrDefeat;
                    break;
                default:
                    prefix = SkillPrefix.None;
                    break;
            }
            // -- End Prefix

            // -- Suffix --
            // CancelLeader and ProtectAbility cannot be valid Ability Suffixes, so they are not included here
            if (UsedRegex.Suffix.CancelAttackModifier.IsMatch(suffixText))
            {
                suffix = SkillSuffix.CancelAttackModifier;
            }
            else if (UsedRegex.Suffix.CancelDamageModifier.IsMatch(suffixText))
            {
                suffix = SkillSuffix.CancelDamageModifier;
            }
            else if (UsedRegex.Suffix.CancelLifeModifier.IsMatch(suffixText))
            {
                suffix = SkillSuffix.CancelLifeModifier;
            }
            else if (UsedRegex.Suffix.CancelPillzAndLifeModifier.IsMatch(suffixText))
            {
                suffix = SkillSuffix.CancelPillzAndLifeModifier;
            }
            else if (UsedRegex.Suffix.CancelPillzModifier.IsMatch(suffixText))
            {
                suffix = SkillSuffix.CancelPillzModifier;
            }
            else if (UsedRegex.Suffix.CancelPowerAndDamageModifier.IsMatch(suffixText))
            {
                suffix = SkillSuffix.CancelPowerAndDamageModifier;
            }
            else if (UsedRegex.Suffix.CancelPowerModifier.IsMatch(suffixText))
            {
                suffix = SkillSuffix.CancelPowerModifier;
            }
            else if (UsedRegex.Suffix.ConsumeXMinY.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.ConsumeXMinY.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                y = match.Groups["y"].Captures[0].Value;
                suffix = SkillSuffix.ConsumeXMinY;
            }
            else if (UsedRegex.Suffix.CopyBonus.IsMatch(suffixText))
            {
                suffix = SkillSuffix.CopyBonus;
            }
            else if (UsedRegex.Suffix.CopyDamage.IsMatch(suffixText))
            {
                suffix = SkillSuffix.CopyDamage;
            }
            else if (UsedRegex.Suffix.CopyPower.IsMatch(suffixText))
            {
                suffix = SkillSuffix.CopyPower;
            }
            else if (UsedRegex.Suffix.CopyPowerAndDamage.IsMatch(suffixText))
            {
                suffix = SkillSuffix.CopyPowerAndDamage;
            }
            else if (UsedRegex.Suffix.DecreaseAttackXMinY.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.DecreaseAttackXMinY.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                y = match.Groups["y"].Captures[0].Value;
                suffix = SkillSuffix.DecreaseAttackXMinY;
            }
            else if (UsedRegex.Suffix.DecreaseAttackXPerRemainingLifeMinY.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.DecreaseAttackXPerRemainingLifeMinY.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                y = match.Groups["y"].Captures[0].Value;
                suffix = SkillSuffix.DecreaseAttackXPerRemainingLifeMinY;
            }
            else if (UsedRegex.Suffix.DecreaseAttackXPerRemainingPillzMinY.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.DecreaseAttackXPerRemainingPillzMinY.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                y = match.Groups["y"].Captures[0].Value;
                suffix = SkillSuffix.DecreaseAttackXPerRemainingPillzMinY;
            }
            else if (UsedRegex.Suffix.DecreaseDamageXMinY.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.DecreaseDamageXMinY.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                y = match.Groups["y"].Captures[0].Value;
                suffix = SkillSuffix.DecreaseDamageXMinY;
            }
            else if (UsedRegex.Suffix.DecreaseLifeAndPillzXMinY.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.DecreaseLifeAndPillzXMinY.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                y = match.Groups["y"].Captures[0].Value;
                suffix = SkillSuffix.DecreaseLifeAndPillzXMinY;
            }
            else if (UsedRegex.Suffix.DecreaseLifeXMinY.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.DecreaseLifeXMinY.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                y = match.Groups["y"].Captures[0].Value;
                suffix = SkillSuffix.DecreaseLifeXMinY;
            }
            else if (UsedRegex.Suffix.DecreasePillzXMinY.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.DecreasePillzXMinY.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                y = match.Groups["y"].Captures[0].Value;
                suffix = SkillSuffix.DecreasePillzXMinY;
            }
            else if (UsedRegex.Suffix.DecreasePowerAndDamageXMinY.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.DecreasePowerAndDamageXMinY.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                y = match.Groups["y"].Captures[0].Value;
                suffix = SkillSuffix.DecreasePowerAndDamageXMinY;
            }
            else if (UsedRegex.Suffix.DecreasePowerXMinY.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.DecreasePowerXMinY.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                y = match.Groups["y"].Captures[0].Value;
                suffix = SkillSuffix.DecreasePowerXMinY;
            }
            else if (UsedRegex.Suffix.DopeXMaxY.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.DopeXMaxY.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                y = match.Groups["y"].Captures[0].Value;
                suffix = SkillSuffix.DopeXMaxY;
            }
            else if (UsedRegex.Suffix.ExchangeDamage.IsMatch(suffixText))
            {
                suffix = SkillSuffix.ExchangeDamage;
            }
            else if (UsedRegex.Suffix.ExchangePower.IsMatch(suffixText))
            {
                suffix = SkillSuffix.ExchangePower;
            }
            else if (UsedRegex.Suffix.HealXMaxY.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.HealXMaxY.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                y = match.Groups["y"].Captures[0].Value;
                suffix = SkillSuffix.HealXMaxY;
            }
            else if (UsedRegex.Suffix.IncreaseAttackX.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.IncreaseAttackX.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                suffix = SkillSuffix.IncreaseAttackX;
            }
            else if (UsedRegex.Suffix.IncreaseAttackXPerRemainingLife.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.IncreaseAttackXPerRemainingLife.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                suffix = SkillSuffix.IncreaseAttackXPerRemainingLife;
            }
            else if (UsedRegex.Suffix.IncreaseAttackXPerRemainingPillz.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.IncreaseAttackXPerRemainingPillz.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                suffix = SkillSuffix.IncreaseAttackXPerRemainingPillz;
            }
            else if (UsedRegex.Suffix.IncreaseDamageX.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.IncreaseDamageX.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                suffix = SkillSuffix.IncreaseDamageX;
            }
            else if (UsedRegex.Suffix.IncreaseLifeX.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.IncreaseLifeX.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                suffix = SkillSuffix.IncreaseLifeX;
            }
            else if (UsedRegex.Suffix.IncreaseLifeXMaxY.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.IncreaseLifeXMaxY.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                suffix = SkillSuffix.IncreaseLifeXMaxY;
            }
            else if (UsedRegex.Suffix.IncreaseLifeXPerDamage.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.IncreaseLifeXPerDamage.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                suffix = SkillSuffix.IncreaseLifeXPerDamage;
            }
            else if (UsedRegex.Suffix.IncreaseLifeXPerDamageMaxY.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.IncreaseLifeXPerDamageMaxY.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                y = match.Groups["y"].Captures[0].Value;
                suffix = SkillSuffix.IncreaseLifeXPerDamageMaxY;
            }
            else if (UsedRegex.Suffix.IncreasePillzAndLifeX.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.IncreasePillzAndLifeX.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                suffix = SkillSuffix.IncreasePillzAndLifeX;
            }
            else if (UsedRegex.Suffix.IncreasePillzX.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.IncreasePillzX.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                suffix = SkillSuffix.IncreasePillzX;
            }
            else if (UsedRegex.Suffix.IncreasePillzXMaxY.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.IncreasePillzXMaxY.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                y = match.Groups["y"].Captures[0].Value;
                suffix = SkillSuffix.IncreasePillzXMaxY;
            }
            else if (UsedRegex.Suffix.IncreasePillzXPerDamage.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.IncreasePillzXPerDamage.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                suffix = SkillSuffix.IncreasePillzXPerDamage;
            }
            else if (UsedRegex.Suffix.IncreasePowerAndDamageX.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.IncreasePowerAndDamageX.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                suffix = SkillSuffix.IncreasePowerAndDamageX;
            }
            else if (UsedRegex.Suffix.IncreasePowerX.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.IncreasePowerX.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                suffix = SkillSuffix.IncreasePowerX;
            }
            else if (UsedRegex.Suffix.InfectionXMinY.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.InfectionXMinY.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                y = match.Groups["y"].Captures[0].Value;
                suffix = SkillSuffix.IncreasePowerX;
            }
            else if (UsedRegex.Suffix.PoisonXMinY.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.PoisonXMinY.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                y = match.Groups["y"].Captures[0].Value;
                suffix = SkillSuffix.PoisonXMinY;
            }
            else if (UsedRegex.Suffix.ProtectAttack.IsMatch(suffixText))
            {
                suffix = SkillSuffix.ProtectAttack;
            }
            else if (UsedRegex.Suffix.ProtectBonus.IsMatch(suffixText))
            {
                suffix = SkillSuffix.ProtectBonus;
            }
            else if (UsedRegex.Suffix.ProtectDamage.IsMatch(suffixText))
            {
                suffix = SkillSuffix.ProtectDamage;
            }
            else if (UsedRegex.Suffix.ProtectPower.IsMatch(suffixText))
            {
                suffix = SkillSuffix.ProtectPower;
            }
            else if (UsedRegex.Suffix.ProtectPowerAndDamage.IsMatch(suffixText))
            {
                suffix = SkillSuffix.ProtectPowerAndDamage;
            }
            else if (UsedRegex.Suffix.RecoverXPillzOutOfY.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.RecoverXPillzOutOfY.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                y = match.Groups["y"].Captures[0].Value;
                suffix = SkillSuffix.RecoverXPillzOutOfY;
            }
            else if (UsedRegex.Suffix.RegenXMaxY.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.RegenXMaxY.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                y = match.Groups["y"].Captures[0].Value;
                suffix = SkillSuffix.RegenXMaxY;
            }
            else if (UsedRegex.Suffix.StopAbility.IsMatch(suffixText))
            {
                suffix = SkillSuffix.StopAbility;
            }
            else if (UsedRegex.Suffix.StopBonus.IsMatch(suffixText))
            {
                suffix = SkillSuffix.StopBonus;
            }
            else if (UsedRegex.Suffix.ToxinXMinY.IsMatch(suffixText))
            {
                match = UsedRegex.Suffix.ToxinXMinY.Match(suffixText);
                x = match.Groups["x"].Captures[0].Value;
                y = match.Groups["y"].Captures[0].Value;
                suffix = SkillSuffix.ToxinXMinY;
            }

            // -- End Suffix --

            int X, Y;

            int.TryParse(x, out X);
            int.TryParse(y, out Y);

            return new Skill(prefix, suffix, X, Y);
        }

        /// <summary>
        /// Returns a new instance of <see cref="CardBase"/> using server data.
        /// </summary>
        /// <remark>This method does not do data validation. Parameter names on this function (except <paramref name="cardLevels"/>) use the nomenclature used by the API call "characters.getCharacters" </remark>
        /// <param name="id">Unique identifier of the card.</param>
        /// <param name="name">Name. If this ends in " Rb" it will override <paramref name="rarity"/> and will set it to <seealso cref="UrbanRivalsCore.Model.CardRarity.Rebirth"/>.</param>
        /// <param name="clan_id">Unique identifier of the clan.</param>
        /// <param name="level_min">Level at which the card starts.</param>
        /// <param name="level_max">Maximum level the card can achieve.</param>
        /// <param name="rarity">Rarity.</param>
        /// <param name="ability">Ability text.</param>
        /// <param name="ability_unlock_level">Level at which the card unlocks its ability.</param>
        /// <param name="release_date">Release date of the card.</param>
        /// <param name="cardLevels">Levels of the cards. This must be fabricated using the server data.</param>
        /// <returns></returns>
        public static CardBase ToCardBase(int id, string name, int clan_id, int level_min, int level_max, string rarity, string ability, int ability_unlock_level, int release_date, List<CardLevel> cardLevels)
        {
            if (ability.Contains("Day:") || ability.Contains("Night:")) return null; // TODO: Remove this line if day/night is implemented, or after 11/2018, whatever happens first

            var parsedClan = Clan.GetClanById((ClanId)clan_id);
            var parsedAbility = ParseAbility(ability);
            var parsedRarity = ParseRarity(rarity);
            var parsedReleaseDate = ParseReleaseDate(release_date);

            // UR says that Rebirth cards have "common" rarity, and I feel like breaking the law ;)
            if (name.EndsWith(" Rb"))
                parsedRarity = CardRarity.Rebirth;

            if (ability == null) return null; // TODO: Remove this line if double prefix is implemented, or after 11/2018, whatever happens first

            return new CardBase(id, name, parsedClan, level_min, level_max, cardLevels, parsedAbility, ability_unlock_level, parsedRarity, parsedReleaseDate);
        }
    }
}
