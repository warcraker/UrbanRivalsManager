using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UrbanRivalsCore.Model;
using UrbanRivalsUtils;

namespace UrbanRivalsApiToCoreAdapter
{
    public static class ApiToCardDefinitionAdapter
    {
        private static class PrvUsedRegex
        {
            public static readonly Regex PREFIX_AND_SUFFIX_REGEX =
                new Regex(@"^(?<prefix>Backlash|Brawl|Confidence|Courage|Defeat|Degrowth|Equalizer|Growth|Killshot|Reprisal|Revenge|Stop|Support|Victory Or Defeat|Vict[.] Or Def[.]) ?: (?<suffix>[a-zA-Z0-9 .,:=&+-]+)$");
            public static class Suffix
            {
                public static readonly Regex CANCEL_ATTACK_MODIFIER_REGEX = new Regex(@"^Cancel Opp[.] Attack Modif[.]$");
                public static readonly Regex CANCEL_DAMAGE_MODIFIER_REGEX = new Regex(@"^Cancel Opp[.] Damage Modif[.]$");
                public static readonly Regex CANCEL_LIFE_MODIFIER_REGEX = new Regex(@"^Cancel Opp[.] Life Modif[.]$");
                public static readonly Regex CANCEL_PILLZ_AND_LIFE_MODIFIER_REGEX = new Regex(@"^Cancel Opp[.] Pillz & Life Modif[.]$");
                public static readonly Regex CANCEL_PILLZ_MODIFIER_REGEX = new Regex(@"^Cancel Opp[.] Pillz Modif[.]$");
                public static readonly Regex CANCEL_POWER_AND_DAMAGE_MODIFIER_REGEX = new Regex(@"^Cancel Opp[.]? Power And Damage( Modif[.]| Mod)?$");
                public static readonly Regex CANCEL_POWER_MODIFIER_REGEX = new Regex(@"^Cancel Opp[.] Power Modif[.]$");

                public static readonly Regex CONSUME_X_MIN_Y_REGEX = new Regex(@"^Consume (?<x>[0-9]+), Min (?<y>[0-9]+)$");

                public static readonly Regex COPY_BONUS_REGEX = new Regex(@"^Copy:? Opp[.] Bonus$");
                public static readonly Regex COPY_DAMAGE_REGEX = new Regex(@"^Copy: Opp[.] Damage|Damage = Damage Opp[.]$");
                public static readonly Regex COPY_POWER_REGEX = new Regex(@"^Copy: Opp[.] Power|Power = Power Opp$");
                public static readonly Regex COPY_POWER_AND_DAMAGE_REGEX = new Regex(@"^Copy: Power And Damage Opp[.]?$");

                public static readonly Regex CORROSION_X_MIN_Y_REGEX = new Regex(@"^Corrosion (?<x>[0-9]+), Min (?<y>[0-9]+)$");

                public static readonly Regex DECREASE_ATTACK_X_MIN_Y_REGEX = new Regex(@"^- ?(?<x>[0-9]+) (Opp[.]? )?Attack,? Min (?<y>[0-9]+)$");
                public static readonly Regex DECREASE_ATTACK_X_PER_REMAINING_LIFE_MIN_Y_REGEX = new Regex(@"^- ?(?<x>[0-9]+) (Opp[.]? )?(Attack|Att[.]) Per Life Left,? Min (?<y>[0-9]+)$");
                public static readonly Regex DECREASE_ATTACK_X_PER_REMAINING_PILLZ_MIN_Y_REGEX = new Regex(@"^-(?<x>[0-9]+) Opp Att[.] Per Pillz Left, Min (?<y>[0-9]+)$");
                public static readonly Regex DECREASE_DAMAGE_X_MIN_Y_REGEX = new Regex(@"^- ?(?<x>[0-9]+) (Opp[.]? )?D(?:amage|mg),? Min (?<y>[0-9]+)$");
                public static readonly Regex DECREASE_LIFE_AND_PILLZ_X_MIN_Y_REGEX = new Regex(@"^-(?<x>[0-9]+) Opp( Life & Pillz Min|[.] Pillz And Life, Min) (?<y>[0-9]+)$");
                public static readonly Regex DECREASE_LIFE_X_MIN_Y_REGEX = new Regex(@"^- ?(?<x>[0-9]+) (Opp[.]? )?Life[.,]? (Opp[.]? )?Min (?<y>[0-9]+)$");
                public static readonly Regex DECREASE_PILLZ_X_MIN_Y_REGEX = new Regex(@"^- ?(?<x>[0-9]+) (Opp[.]? )?Pillz[.,]? (Opp[.]? )?Min (?<y>[0-9]+)$");
                public static readonly Regex DECREASE_POWER_AND_DAMAGE_X_MIN_Y_REGEX = new Regex(@"^- ?(?<x>[0-9]+) (Opp[.]? )?Pow(er)?[.]? (And|&) D(amage|mg|am)[.]?,? ?[Mm]in (?<y>[0-9]+)$");
                public static readonly Regex DECREASE_POWER_X_MIN_Y_REGEX = new Regex(@"^- ?(?<x>[0-9]+) (Opp[.]? )?Pow(er)?[,.]? Min (?<y>[0-9]+)$");

                public static readonly Regex DOPE_X_MAX_Y_REGEX = new Regex(@"^Dope (?<x>[0-9]), Max[.] (?<y>[0-9]+)$");

                public static readonly Regex EXCHANGE_DAMAGE_REGEX = new Regex(@"^Damage Exchange$");
                public static readonly Regex EXCHANGE_POWER_REGEX = new Regex(@"^Power Exchange$");

                public static readonly Regex HEAL_X_MAX_Y_REGEX = new Regex(@"^Heal (?<x>[0-9]+) Max[.]? (?<y>[0-9]+)$");

                public static readonly Regex INCREASE_ATTACK_X_REGEX = new Regex(@"^At(tac)?k[.]? [+](?<x>[0-9]+)$");
                public static readonly Regex INCREASE_ATTACK_X_PER_REMAINING_LIFE_REGEX = new Regex(@"^[+] ?(?<x>[0-9]+) At(tac)?k Per Life Left$");
                public static readonly Regex INCREASE_ATTACK_X_PER_REMAINING_PILLZ_REGEX = new Regex(@"^[+] ?(?<x>[0-9]+) At(tac)?k Per Pillz Left$");
                public static readonly Regex INCREASE_DAMAGE_X_REGEX = new Regex(@"^D(amage|mg[.]?) [+] ?(?<x>[0-9]+)$");
                public static readonly Regex INCREASE_LIFE_X_REGEX = new Regex(@"^[+] ?(?<x>[0-9]+) Life$");
                public static readonly Regex INCREASE_LIFE_X_MAX_Y_REGEX = new Regex(@"^[+] ?(?<x>[0-9]+) Life[,]? Max[.] (?<y>[0-9]+)$");
                public static readonly Regex INCREASE_LIFE_X_PER_DAMAGE_REGEX = new Regex(@"^[+] ?(?<x>[0-9]+) Life Per D(amage|mg[.]?)$");
                public static readonly Regex INCREASE_LIFE_X_PER_DAMAGE_MAX_Y_REGEX = new Regex(@"^[+] ?(?<x>[0-9]+) Life Per D(amage|mg[.]?) Max[.] (?<y>[0-9]+)$");
                public static readonly Regex INCREASE_PILLZ_AND_LIFE_X_REGEX = new Regex(@"^[+](?<x>[0-9]+) Pillz And Life$");
                public static readonly Regex INCREASE_PILLZ_X_REGEX = new Regex(@"^[+] ?(?<x>[0-9]+) Pillz$");
                public static readonly Regex INCREASE_PILLZ_X_MAX_Y_REGEX = new Regex(@"^[+] ?(?<x>[0-9]+) Pillz Max[.] (?<y>[0-9]+)$");
                public static readonly Regex INCREASE_PILLZ_X_PER_DAMAGE_REGEX = new Regex(@"^[+] ?(?<x>[0-9]+) Pillz Per D(amage|mg[.]?)$");
                public static readonly Regex INCREASE_POWER_AND_DAMAGE_X_REGEX = new Regex(@"^Pow(er[.]?) (And|&) D(amage|mg[.]?) ?[+] ?(?<x>[0-9]+)$");
                public static readonly Regex INCREASE_POWER_X_REGEX = new Regex(@"^Pow(er[.]?) [+] ?(?<x>[0-9]+)$");

                public static readonly Regex INFECTION_X_MIN_Y_REGEX = new Regex(@"^Infection (?<x>[0-9]), Min (?<y>[0-9])$");

                public static readonly Regex POISON_X_MIN_Y_REGEX = new Regex(@"^Poison (?<x>[0-9]),? Min (?<y>[0-9])$");

                public static readonly Regex PROTECT_ABILITY_REGEX = new Regex(@"^Protection ?: Ability$");
                public static readonly Regex PROTECT_ATTACK_REGEX = new Regex(@"^Protection ?: Attack$");
                public static readonly Regex PROTECT_BONUS_REGEX = new Regex(@"^Protection ?: Bonus|Bonus Protection$");
                public static readonly Regex PROTECT_DAMAGE_REGEX = new Regex(@"^Protection ?: Damage$");
                public static readonly Regex PROTECT_POWER_REGEX = new Regex(@"^Protection ?: Power$");
                public static readonly Regex PROTECT_POWER_AND_DAMAGE_REGEX = new Regex(@"^(Prot[.]:|Protec[.]|Protect[.]|Protection:) Power (And|&) (Dmg|Damage)$");

                public static readonly Regex REANIMATE_X_REGEX = new Regex(@"^Reanimate: [+](?<x>[0-9]) Life$");

                public static readonly Regex REBIRTH_X_MAX_Y_REGEX = new Regex(@"^Rebirth (?<x>[0-9]), Max[.] (?<y>[0-9])$");

                public static readonly Regex RECOVER_X_PILLZ_OUT_OF_Y_REGEX = new Regex(@"^Recover (?<x>[0-9]) Pillz Out Of (?<y>[0-9])$");

                public static readonly Regex REGEN_X_MAX_Y_REGEX = new Regex(@"^Regen (?<x>[0-9]),? Max[.]? (?<y>[0-9]+)$");

                public static readonly Regex STOP_ABILITY_REGEX = new Regex(@"^Stop( Opp[.]?)? Ability$");
                public static readonly Regex STOP_BONUS_REGEX = new Regex(@"^Stop( Opp[.]?)? Bonus$");

                public static readonly Regex TOXIN_X_MIN_Y_REGEX = new Regex(@"^Toxin (?<x>[0-9]),? Min (?<y>[0-9])$");
            }
        }

        private static CardRarity prv_parseRarity(string valueToParse)
        {
            CardRarity rarity;

            switch (valueToParse)
            {
                case "c":
                    rarity = CardRarity.Common;
                    break;
                case "u":
                    rarity = CardRarity.Uncommon;
                    break;
                case "r":
                    rarity = CardRarity.Rare;
                    break;
                case "l":
                    rarity = CardRarity.Legendary;
                    break;
                case "cr":
                    rarity = CardRarity.Collector;
                    break;
                case "rb":
                    rarity = CardRarity.Rebirth;
                    break;
                case "m":
                    rarity = CardRarity.Mythic;
                    break;
                default:
                    rarity = CardRarity.Common;
                    AssertArgument.fail($"Rarity has a not defined value = {valueToParse}", nameof(valueToParse));
                    break;
            }

            return rarity;
        }
        private static Skill prv_parseAbility(string abilityString)
        {
            Skill ability;

            if (abilityString == "No ability")
            {
                ability = Skill.NO_ABILITY;
            }
            else
            {
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

                // Exceptional case: Xantiax Robb Cr (ID = 1573). Is the only card with this prefix.
                // Why: It is a new prefix that resembles to Backlash and Victory or Defeat. Needs to much work for a single card by now.
                if (abilityString == "Xantiax: -3 Life, Min. 0")
                    return null;

                // Exceptional case: Silvano (ID = 1714). Every card has a single prefix, or none. This one has a double prefix
                // Why: The alternative is add a loop that calls (at least) two times the regex "PrefixAndSuffix" for each parse
                if (abilityString == "Defeat: Brawl: +1 Life")
                    return null;
                //


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
                    case "Team: Reprisal: -2 Pow. & Dmg, Min 3":
                        return new Skill(SkillLeader.JonhDoom);
                    case "Team: Defeat: Rec. 1 Pillz Out Of 2":
                        return new Skill(SkillLeader.Melody);
                    case "+1 Pillz Per Round, Max. 10":
                        return new Skill(SkillLeader.Morphun);
                    case "Nuke":
                        return new Skill(SkillLeader.MrBigDuke);
                    case "Bypass":
                        return new Skill(SkillLeader.RobbertCobb);
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

                var match = PrvUsedRegex.PREFIX_AND_SUFFIX_REGEX.Match(abilityString);

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
                if (PrvUsedRegex.Suffix.CANCEL_ATTACK_MODIFIER_REGEX.IsMatch(suffixText))
                {
                    suffix = SkillSuffix.CancelAttackModifier;
                }
                else if (PrvUsedRegex.Suffix.CANCEL_DAMAGE_MODIFIER_REGEX.IsMatch(suffixText))
                {
                    suffix = SkillSuffix.CancelDamageModifier;
                }
                else if (PrvUsedRegex.Suffix.CANCEL_LIFE_MODIFIER_REGEX.IsMatch(suffixText))
                {
                    suffix = SkillSuffix.CancelLifeModifier;
                }
                else if (PrvUsedRegex.Suffix.CANCEL_PILLZ_AND_LIFE_MODIFIER_REGEX.IsMatch(suffixText))
                {
                    suffix = SkillSuffix.CancelPillzAndLifeModifier;
                }
                else if (PrvUsedRegex.Suffix.CANCEL_PILLZ_MODIFIER_REGEX.IsMatch(suffixText))
                {
                    suffix = SkillSuffix.CancelPillzModifier;
                }
                else if (PrvUsedRegex.Suffix.CANCEL_POWER_AND_DAMAGE_MODIFIER_REGEX.IsMatch(suffixText))
                {
                    suffix = SkillSuffix.CancelPowerAndDamageModifier;
                }
                else if (PrvUsedRegex.Suffix.CANCEL_POWER_MODIFIER_REGEX.IsMatch(suffixText))
                {
                    suffix = SkillSuffix.CancelPowerModifier;
                }
                else if (PrvUsedRegex.Suffix.CONSUME_X_MIN_Y_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.CONSUME_X_MIN_Y_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    y = match.Groups["y"].Captures[0].Value;
                    suffix = SkillSuffix.ConsumeXMinY;
                }
                else if (PrvUsedRegex.Suffix.COPY_BONUS_REGEX.IsMatch(suffixText))
                {
                    suffix = SkillSuffix.CopyBonus;
                }
                else if (PrvUsedRegex.Suffix.COPY_DAMAGE_REGEX.IsMatch(suffixText))
                {
                    suffix = SkillSuffix.CopyDamage;
                }
                else if (PrvUsedRegex.Suffix.COPY_POWER_REGEX.IsMatch(suffixText))
                {
                    suffix = SkillSuffix.CopyPower;
                }
                else if (PrvUsedRegex.Suffix.COPY_POWER_AND_DAMAGE_REGEX.IsMatch(suffixText))
                {
                    suffix = SkillSuffix.CopyPowerAndDamage;
                }
                else if (PrvUsedRegex.Suffix.CORROSION_X_MIN_Y_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.CORROSION_X_MIN_Y_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    y = match.Groups["y"].Captures[0].Value;
                    suffix = SkillSuffix.CorrosionXMinY;
                }
                else if (PrvUsedRegex.Suffix.DECREASE_ATTACK_X_MIN_Y_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.DECREASE_ATTACK_X_MIN_Y_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    y = match.Groups["y"].Captures[0].Value;
                    suffix = SkillSuffix.DecreaseAttackXMinY;
                }
                else if (PrvUsedRegex.Suffix.DECREASE_ATTACK_X_PER_REMAINING_LIFE_MIN_Y_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.DECREASE_ATTACK_X_PER_REMAINING_LIFE_MIN_Y_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    y = match.Groups["y"].Captures[0].Value;
                    suffix = SkillSuffix.DecreaseAttackXPerRemainingLifeMinY;
                }
                else if (PrvUsedRegex.Suffix.DECREASE_ATTACK_X_PER_REMAINING_PILLZ_MIN_Y_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.DECREASE_ATTACK_X_PER_REMAINING_PILLZ_MIN_Y_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    y = match.Groups["y"].Captures[0].Value;
                    suffix = SkillSuffix.DecreaseAttackXPerRemainingPillzMinY;
                }
                else if (PrvUsedRegex.Suffix.DECREASE_DAMAGE_X_MIN_Y_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.DECREASE_DAMAGE_X_MIN_Y_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    y = match.Groups["y"].Captures[0].Value;
                    suffix = SkillSuffix.DecreaseDamageXMinY;
                }
                else if (PrvUsedRegex.Suffix.DECREASE_LIFE_AND_PILLZ_X_MIN_Y_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.DECREASE_LIFE_AND_PILLZ_X_MIN_Y_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    y = match.Groups["y"].Captures[0].Value;
                    suffix = SkillSuffix.DecreaseLifeAndPillzXMinY;
                }
                else if (PrvUsedRegex.Suffix.DECREASE_LIFE_X_MIN_Y_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.DECREASE_LIFE_X_MIN_Y_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    y = match.Groups["y"].Captures[0].Value;
                    suffix = SkillSuffix.DecreaseLifeXMinY;
                }
                else if (PrvUsedRegex.Suffix.DECREASE_PILLZ_X_MIN_Y_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.DECREASE_PILLZ_X_MIN_Y_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    y = match.Groups["y"].Captures[0].Value;
                    suffix = SkillSuffix.DecreasePillzXMinY;
                }
                else if (PrvUsedRegex.Suffix.DECREASE_POWER_AND_DAMAGE_X_MIN_Y_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.DECREASE_POWER_AND_DAMAGE_X_MIN_Y_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    y = match.Groups["y"].Captures[0].Value;
                    suffix = SkillSuffix.DecreasePowerAndDamageXMinY;
                }
                else if (PrvUsedRegex.Suffix.DECREASE_POWER_X_MIN_Y_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.DECREASE_POWER_X_MIN_Y_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    y = match.Groups["y"].Captures[0].Value;
                    suffix = SkillSuffix.DecreasePowerXMinY;
                }
                else if (PrvUsedRegex.Suffix.DOPE_X_MAX_Y_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.DOPE_X_MAX_Y_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    y = match.Groups["y"].Captures[0].Value;
                    suffix = SkillSuffix.DopeXMaxY;
                }
                else if (PrvUsedRegex.Suffix.EXCHANGE_DAMAGE_REGEX.IsMatch(suffixText))
                {
                    suffix = SkillSuffix.ExchangeDamage;
                }
                else if (PrvUsedRegex.Suffix.EXCHANGE_POWER_REGEX.IsMatch(suffixText))
                {
                    suffix = SkillSuffix.ExchangePower;
                }
                else if (PrvUsedRegex.Suffix.HEAL_X_MAX_Y_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.HEAL_X_MAX_Y_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    y = match.Groups["y"].Captures[0].Value;
                    suffix = SkillSuffix.HealXMaxY;
                }
                else if (PrvUsedRegex.Suffix.INCREASE_ATTACK_X_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.INCREASE_ATTACK_X_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    suffix = SkillSuffix.IncreaseAttackX;
                }
                else if (PrvUsedRegex.Suffix.INCREASE_ATTACK_X_PER_REMAINING_LIFE_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.INCREASE_ATTACK_X_PER_REMAINING_LIFE_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    suffix = SkillSuffix.IncreaseAttackXPerRemainingLife;
                }
                else if (PrvUsedRegex.Suffix.INCREASE_ATTACK_X_PER_REMAINING_PILLZ_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.INCREASE_ATTACK_X_PER_REMAINING_PILLZ_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    suffix = SkillSuffix.IncreaseAttackXPerRemainingPillz;
                }
                else if (PrvUsedRegex.Suffix.INCREASE_DAMAGE_X_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.INCREASE_DAMAGE_X_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    suffix = SkillSuffix.IncreaseDamageX;
                }
                else if (PrvUsedRegex.Suffix.INCREASE_LIFE_X_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.INCREASE_LIFE_X_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    suffix = SkillSuffix.IncreaseLifeX;
                }
                else if (PrvUsedRegex.Suffix.INCREASE_LIFE_X_MAX_Y_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.INCREASE_LIFE_X_MAX_Y_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    suffix = SkillSuffix.IncreaseLifeXMaxY;
                }
                else if (PrvUsedRegex.Suffix.INCREASE_LIFE_X_PER_DAMAGE_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.INCREASE_LIFE_X_PER_DAMAGE_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    suffix = SkillSuffix.IncreaseLifeXPerDamage;
                }
                else if (PrvUsedRegex.Suffix.INCREASE_LIFE_X_PER_DAMAGE_MAX_Y_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.INCREASE_LIFE_X_PER_DAMAGE_MAX_Y_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    y = match.Groups["y"].Captures[0].Value;
                    suffix = SkillSuffix.IncreaseLifeXPerDamageMaxY;
                }
                else if (PrvUsedRegex.Suffix.INCREASE_PILLZ_AND_LIFE_X_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.INCREASE_PILLZ_AND_LIFE_X_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    suffix = SkillSuffix.IncreasePillzAndLifeX;
                }
                else if (PrvUsedRegex.Suffix.INCREASE_PILLZ_X_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.INCREASE_PILLZ_X_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    suffix = SkillSuffix.IncreasePillzX;
                }
                else if (PrvUsedRegex.Suffix.INCREASE_PILLZ_X_MAX_Y_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.INCREASE_PILLZ_X_MAX_Y_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    y = match.Groups["y"].Captures[0].Value;
                    suffix = SkillSuffix.IncreasePillzXMaxY;
                }
                else if (PrvUsedRegex.Suffix.INCREASE_PILLZ_X_PER_DAMAGE_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.INCREASE_PILLZ_X_PER_DAMAGE_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    suffix = SkillSuffix.IncreasePillzXPerDamage;
                }
                else if (PrvUsedRegex.Suffix.INCREASE_POWER_AND_DAMAGE_X_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.INCREASE_POWER_AND_DAMAGE_X_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    suffix = SkillSuffix.IncreasePowerAndDamageX;
                }
                else if (PrvUsedRegex.Suffix.INCREASE_POWER_X_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.INCREASE_POWER_X_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    suffix = SkillSuffix.IncreasePowerX;
                }
                else if (PrvUsedRegex.Suffix.INFECTION_X_MIN_Y_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.INFECTION_X_MIN_Y_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    y = match.Groups["y"].Captures[0].Value;
                    suffix = SkillSuffix.IncreasePowerX;
                }
                else if (PrvUsedRegex.Suffix.POISON_X_MIN_Y_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.POISON_X_MIN_Y_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    y = match.Groups["y"].Captures[0].Value;
                    suffix = SkillSuffix.PoisonXMinY;
                }
                else if (PrvUsedRegex.Suffix.PROTECT_ATTACK_REGEX.IsMatch(suffixText))
                {
                    suffix = SkillSuffix.ProtectAttack;
                }
                else if (PrvUsedRegex.Suffix.PROTECT_BONUS_REGEX.IsMatch(suffixText))
                {
                    suffix = SkillSuffix.ProtectBonus;
                }
                else if (PrvUsedRegex.Suffix.PROTECT_DAMAGE_REGEX.IsMatch(suffixText))
                {
                    suffix = SkillSuffix.ProtectDamage;
                }
                else if (PrvUsedRegex.Suffix.PROTECT_POWER_REGEX.IsMatch(suffixText))
                {
                    suffix = SkillSuffix.ProtectPower;
                }
                else if (PrvUsedRegex.Suffix.PROTECT_POWER_AND_DAMAGE_REGEX.IsMatch(suffixText))
                {
                    suffix = SkillSuffix.ProtectPowerAndDamage;
                }
                else if (PrvUsedRegex.Suffix.REANIMATE_X_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.REANIMATE_X_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    suffix = SkillSuffix.ReanimateX;
                }
                else if (PrvUsedRegex.Suffix.REBIRTH_X_MAX_Y_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.REBIRTH_X_MAX_Y_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    y = match.Groups["y"].Captures[0].Value;
                    suffix = SkillSuffix.RebirthXMaxY;
                }
                else if (PrvUsedRegex.Suffix.RECOVER_X_PILLZ_OUT_OF_Y_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.RECOVER_X_PILLZ_OUT_OF_Y_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    y = match.Groups["y"].Captures[0].Value;
                    suffix = SkillSuffix.RecoverXPillzOutOfY;
                }
                else if (PrvUsedRegex.Suffix.REGEN_X_MAX_Y_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.REGEN_X_MAX_Y_REGEX.Match(suffixText);
                    x = match.Groups["x"].Captures[0].Value;
                    y = match.Groups["y"].Captures[0].Value;
                    suffix = SkillSuffix.RegenXMaxY;
                }
                else if (PrvUsedRegex.Suffix.STOP_ABILITY_REGEX.IsMatch(suffixText))
                {
                    suffix = SkillSuffix.StopAbility;
                }
                else if (PrvUsedRegex.Suffix.STOP_BONUS_REGEX.IsMatch(suffixText))
                {
                    suffix = SkillSuffix.StopBonus;
                }
                else if (PrvUsedRegex.Suffix.TOXIN_X_MIN_Y_REGEX.IsMatch(suffixText))
                {
                    match = PrvUsedRegex.Suffix.TOXIN_X_MIN_Y_REGEX.Match(suffixText);
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

            return ability;
        }

        public static CardDefinition createCardDefinitionByServerData(int id, string name, int clan_id, string rarity, string ability, int ability_unlock_level, List<CardStats> cardStatsPerLevel)
        {
            CardDefinition cardDefinition;

            if (ability.Contains("Day:") || ability.Contains("Night:")) return null; // TODO: Remove this line if day/night is implemented, or after 11/2018, whatever happens first

            var parsedClan = Clan.getClanById((ClanId)clan_id);
            var parsedAbility = prv_parseAbility(ability);
            var parsedRarity = prv_parseRarity(rarity);

            // UR says that Rebirth cards have "common" rarity, and I feel like breaking the law ;)
            if (name.EndsWith(" Rb"))
                parsedRarity = CardRarity.Rebirth;

            if (ability == null) // TODO: Remove this line if double prefix is implemented, or after 11/2018, whatever happens first
            {
                cardDefinition = CardDefinition.createCardWithoutAbility(id, name, parsedClan, cardStatsPerLevel, parsedRarity);
            }
            else
            {
                cardDefinition = CardDefinition.createCardWithAbility(id, name, parsedClan, cardStatsPerLevel, parsedRarity, parsedAbility, ability_unlock_level);
            }

            return cardDefinition;
        }
    }
}
