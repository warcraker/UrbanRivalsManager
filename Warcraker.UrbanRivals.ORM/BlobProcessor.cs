using HashUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UrbanRivalsApiManager;
using Warcraker.UrbanRivals.Core.Model.Cards;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills.Prefixes;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes;
using Warcraker.UrbanRivals.Core.Model.Cycles;
using Warcraker.UrbanRivals.DataRepository;
using Warcraker.UrbanRivals.DataRepository.DataModels;
using Warcraker.Utils;

namespace Warcraker.UrbanRivals.TextProcess
{
    public class BlobProcessor
    {
        private const char COMMA_SEPARATOR = ';';

        private readonly GameDataRepository repository;

        public BlobProcessor(GameDataRepository repository)
        {
            this.repository = repository;
        }

        public Cycle GetCycleData(string blob)
        {
            int blobHash = HashText(blob);
            CycleData cycleData = repository.GetCycleData(blobHash);
            if (cycleData == null)
            {
                IList<string> clanItems = BlobToList<ApiCallList.Characters.GetClans>(blob);
                IList<string> abilityItems = BlobToList<ApiCallList.Characters.GetCharacters>(blob);
                IList<string> cardItems = BlobToList<ApiCallList.Urc.GetCharacters>(blob);

                Asserts.Check(abilityItems.Count == cardItems.Count, $"Characters.GetCharacters and Urc.GetCharacters call counts must be equal");

                IList<int> clanHashes = new List<int>();
                IList<int> abilityHashes = new List<int>();
                IList<int> cardHashes = new List<int>();

                foreach (string clanItem in clanItems)
                {
                    int clanHash = HashText(clanItem);
                    ClanData clanData = repository.GetClanData(clanHash);
                    if (clanData == null)
                    {
                        dynamic decodedClan = JsonConvert.DeserializeObject(clanItem);
                        string bonusText = decodedClan["bonusDescription"].ToString();

                        int bonusTextHash = HashText(bonusText);
                        int bonusHash = repository.GetSkillHashFromTextHash(bonusTextHash);
                        if (bonusHash == repository.SCALAR_VALUE_NOT_FOUND)
                        {
                            Skill bonus = SkillProcessor.ParseSkill(bonusText);
                            SkillData bonusData = SkillToSkillData(bonus);
                            repository.SaveSkillData(bonusData, bonusTextHash);

                            bonusHash = bonusData.Hash;
                        }

                        int clanId = int.Parse(decodedClan["id"].ToString());
                        string clanName = decodedClan["name"].ToString();
                        clanData = new ClanData
                        {
                            Hash = clanHash,
                            BonusHash = bonusHash,
                            Name = clanName,
                            GameId = clanId,
                        };
                        repository.SaveClanData(clanData);
                    }

                    clanHashes.Add(clanHash);
                }

                for (int i = 0, n = cardItems.Count; i < n; i++)
                {
                    string abilityItem = abilityItems[i];
                    dynamic decodedAbility = JsonConvert.DeserializeObject(abilityItem);
                    string abilityText = decodedAbility["ability"].ToString();

                    int abilityTextHash = HashText(abilityText);
                    int abilityHash = repository.GetSkillHashFromTextHash(abilityTextHash);
                    if (abilityHash == repository.SCALAR_VALUE_NOT_FOUND)
                    {
                        Skill ability = SkillProcessor.ParseSkill(abilityText);
                        SkillData abilityData = SkillToSkillData(ability);
                        repository.SaveSkillData(abilityData, abilityTextHash);

                        abilityHash = abilityData.Hash;
                    }

                    int cardGameIdFromAbility = int.Parse(decodedAbility["id"].ToString());

                    string cardItem = cardItems[i];
                    int cardHash = HashText(cardItem);
                    CardData cardData = repository.GetCardData(cardHash);
                    if (cardData == null)
                    {
                        int abilityUnlockLevel = int.Parse(decodedAbility["ability_unlock_level"].ToString());
                        dynamic decodedCard = JsonConvert.DeserializeObject(cardItem);
                        int cardGameId = int.Parse(decodedCard["id"].ToString());
                        string cardName = decodedCard["name"].ToString();
                        int clanId = int.Parse(decodedCard["clanID"].ToString());
                        string rarityText = decodedCard["rarity"].ToString();
                        CardDefinition.ECardRarity rarity = GetRarity(rarityText);

                        IEnumerable<dynamic> levels = decodedCard["levels"].ToList();
                        int initialLevel = int.Parse(levels.First()["level"].ToString());

                        IList<int> powers = new List<int>();
                        IList<int> damages = new List<int>();
                        foreach (dynamic levelItem in levels)
                        {
                            powers.Add(int.Parse(levelItem["power"].ToString()));
                            damages.Add(int.Parse(levelItem["damage"].ToString()));
                        }

                        cardData = new CardData
                        {
                            Hash = cardHash,
                            GameId = cardGameId,
                            Name = cardName,
                            ClanGameId = clanId,
                            InitialLevel = initialLevel,
                            AbilityUnlockLevel = abilityUnlockLevel,
                            Rarity = (int)rarity,
                            PowerPerLevel = FromIntsToCsv(powers),
                            DamagePerLevel = FromIntsToCsv(damages),
                        };
                        repository.SaveCardData(cardData);
                    }

                    Asserts.Check(cardGameIdFromAbility == cardData.GameId, "Characters.GetCharacters and Urc.GetCharacters items must be in the same order");

                    cardHashes.Add(cardHash);
                    abilityHashes.Add(abilityHash);
                }

                cycleData = new CycleData
                {
                    Hash = blobHash,
                    AbilityHashes = FromIntsToCsv(abilityHashes),
                    CardHashes = FromIntsToCsv(cardHashes),
                    ClanHashes = FromIntsToCsv(clanHashes),
                };

                repository.SaveCycleBlobData(cycleData);
            }

            IList<CardDefinition> cards = new List<CardDefinition>();
            IDictionary<int, Clan> clans = new Dictionary<int, Clan>();
            foreach (int clanHash in cycleData.ClanHashes)
            {
                ClanData clanData = repository.GetClanData(clanHash);
                SkillData bonusData = repository.GetSkillData(clanData.BonusHash);
                Skill bonus = SkillDataToSkill(bonusData);
                Clan clan = new Clan(clanData.GameId, clanData.Name, bonus);
                clans[clan.GameId] = clan;
            }

            for (int i = 0, n = cycleData.CardHashes.Length; i < n; i++)
            {
                CardData cardData = repository.GetCardData(cycleData.CardHashes[i]);
                SkillData abilityData = repository.GetSkillData(cycleData.AbilityHashes[i]);
                Skill ability = SkillDataToSkill(abilityData);
                Clan clan = clans[cardData.ClanGameId];
                CardStats stats = new CardStats(cardData.InitialLevel, 
                    FromCsvToInt(cardData.PowerPerLevel), FromCsvToInt(cardData.DamagePerLevel));
                CardDefinition card = new CardDefinition(cardData.GameId, cardData.Name, clan, ability, 
                    cardData.AbilityUnlockLevel, stats, (CardDefinition.ECardRarity)cardData.Rarity);

                cards.Add(card);
            }

            Cycle.ECycleType cycleType = Cycle.ECycleType.Day; // TODO detect day/night
            Cycle cycle = new Cycle(cycleType, cards);

            return cycle;
        }

        private static IList<string> BlobToList<TCall>(string blob) where TCall : ApiCall, new()
        {
            IList<string> result = new List<string>();

            TCall call = new TCall();
            dynamic decoded = JsonConvert.DeserializeObject(blob);
            foreach (dynamic item in decoded[call.Call]["items"])
            {
                result.Add(item.ToString());
            }
            return result;
        }
        private static string FromIntsToCsv(IEnumerable<int> input)
        {
            return input
                .Aggregate(new StringBuilder()
                    , (acc, item) => acc.Append(COMMA_SEPARATOR).Append(item))
                .ToString();
        }
        private static IEnumerable<int> FromCsvToInt(string input)
        {
            return input
                .Split(COMMA_SEPARATOR)
                .Select(item => int.Parse(item));
        }

        private static SkillData SkillToSkillData(Skill skill)
        {
            string[] prefixNames = skill.Prefixes
                .Select(prefix => GetTypeName(prefix))
                .ToArray();

            int skillHash = HashCode
                .OfEach(prefixNames)
                .And(GetTypeName(skill.Suffix))
                .And(skill.X)
                .And(skill.Y);

            string prefixCsv = prefixNames
                .Aggregate(new StringBuilder(), (acc, item) => acc.Append(COMMA_SEPARATOR).Append(item))
                .ToString();

            return new SkillData
            {
                Hash = skillHash,
                PrefixesClassNames = prefixCsv,
                SuffixClassName = GetTypeName(skill.Suffix),
                X = skill.X,
                Y = skill.Y,
            };
        }
        private static Skill SkillDataToSkill(SkillData data)
        {
            Skill skill;

            Suffix suffix = BuildInstance<Suffix>(data.SuffixClassName);
            IEnumerable<Prefix> prefixes = data.PrefixesClassNames
                .Split(COMMA_SEPARATOR)
                .Select(prefix => BuildInstance<Prefix>(prefix));
            skill = Skill.GetStandardSkill(prefixes, suffix, data.X, data.Y);

            return skill;
        }
        private static CardDefinition.ECardRarity GetRarity(string text)
        {
            switch (text)
            {
                case "cr":
                    return CardDefinition.ECardRarity.Collector;
                case "c":
                    return CardDefinition.ECardRarity.Common;
                case "l":
                    return CardDefinition.ECardRarity.Legendary;
                case "m":
                    return CardDefinition.ECardRarity.Mythic;
                case "r":
                    return CardDefinition.ECardRarity.Rare;
                case "u":
                    return CardDefinition.ECardRarity.Uncommon;
                default:
                    Asserts.Fail($"Invalid rarity {text}");
                    return CardDefinition.ECardRarity.Common;
            }
        }

        private static TParent BuildInstance<TParent>(string name)
        {
            return (TParent)Activator.CreateInstance(Type.GetType(name));
        }
        private static string GetTypeName(object o)
        {
            return o.GetType().Name;
        }
        private static int HashText(string text)
        {
            return HashCode.Of(text);
        }
    }
}
