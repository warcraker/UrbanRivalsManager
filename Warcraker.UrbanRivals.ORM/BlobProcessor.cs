using HashUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UrbanRivalsApiManager;
using static Warcraker.UrbanRivals.Common.Constants;
using Warcraker.UrbanRivals.Core.Model.Cards;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills;
using Warcraker.UrbanRivals.Core.Model.Cycles;
using Warcraker.UrbanRivals.DataRepository;
using Warcraker.UrbanRivals.DataRepository.DataModels;
using Warcraker.Utils;
using System.Reflection;

namespace Warcraker.UrbanRivals.ORM
{
    public class BlobProcessor
    {
        private readonly GameDataRepository repository;
        private readonly IDictionary<string, string> fullAssembliesByClassName;

        public BlobProcessor(GameDataRepository repository)
        {
            string leaderEnding = typeof(Leader).Name;
            string prefixEnding = typeof(Prefix).Name;
            string suffixEnding = typeof(Suffix).Name;

            Assembly assembly = typeof(Skill).Assembly;
            IEnumerable<Type> validTypes = assembly
                .GetTypes()
                .Where(t => t.Name.EndsWith(leaderEnding) || t.Name.EndsWith(prefixEnding) || t.Name.EndsWith(suffixEnding));

            this.fullAssembliesByClassName = new Dictionary<string, string>();
            foreach (Type type in validTypes)
            {
                this.fullAssembliesByClassName.Add(type.Name, type.AssemblyQualifiedName);
            }

            this.repository = repository;
        }

        public Cycle GetCycleData(string blob)
        {
            int blobHash = HashText(blob);
            CycleData cycleData = repository.GetCycleData(blobHash);
            if (cycleData == null)
            {
                IList<string> abilityItems = BlobToList<ApiCallList.Characters.GetCharacters>(blob);
                IList<string> cardItems = BlobToList<ApiCallList.Urc.GetCharacters>(blob);
                Asserts.Check(abilityItems.Count == cardItems.Count, $"Characters.GetCharacters and Urc.GetCharacters call counts must be equal");
                IList<string> clanItems = BlobToList<ApiCallList.Characters.GetClans>(blob);

                IList<int> clanHashes = new List<int>();
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

                IList<int> abilityHashes = new List<int>();
                IList<int> cardHashes = new List<int>();
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
                        CardDefinition.ECardRarity rarity = TextToRarity(rarityText);

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
                            PowerPerLevel = IntsToCsv(powers),
                            DamagePerLevel = IntsToCsv(damages),
                        };
                        repository.SaveCardData(cardData);
                    }

                    int cardGameIdFromAbility = int.Parse(decodedAbility["id"].ToString());
                    Asserts.Check(cardGameIdFromAbility == cardData.GameId, 
                        "Characters.GetCharacters and Urc.GetCharacters items must be in the same order");

                    cardHashes.Add(cardHash);
                    abilityHashes.Add(abilityHash);
                }

                cycleData = new CycleData
                {
                    Hash = blobHash,
                    AbilityHashes = IntsToCsv(abilityHashes),
                    CardHashes = IntsToCsv(cardHashes),
                    ClanHashes = IntsToCsv(clanHashes),
                };
                repository.SaveCycleBlobData(cycleData);
            }

            IDictionary<int, Clan> clans = new Dictionary<int, Clan>();
            foreach (int clanHash in cycleData.ClanHashes)
            {
                ClanData clanData = repository.GetClanData(clanHash);
                SkillData bonusData = repository.GetSkillData(clanData.BonusHash);
                Skill bonus = SkillDataToSkill(bonusData);
                Clan clan = new Clan(clanData.GameId, clanData.Name, bonus);
                clans[clan.GameId] = clan;
            }

            IList<CardDefinition> cards = new List<CardDefinition>();
            for (int i = 0, n = cycleData.CardHashes.Length; i < n; i++)
            {
                CardData cardData = repository.GetCardData(cycleData.CardHashes[i]);
                SkillData abilityData = repository.GetSkillData(cycleData.AbilityHashes[i]);
                Skill ability = SkillDataToSkill(abilityData);
                Clan clan = clans[cardData.ClanGameId];
                CardStats stats = new CardStats(cardData.InitialLevel, 
                    CsvToInts(cardData.PowerPerLevel), CsvToInts(cardData.DamagePerLevel));
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
            IEnumerable<dynamic> items = decoded[call.Call]["items"];
            IEnumerable<dynamic> orderedItems = items.OrderBy(x => x["id"].Value);
            foreach (dynamic item in orderedItems)
            {
                string text = item.ToString();
                result.Add(text);
            }
            return result;
        }
        private static IEnumerable<int> CsvToInts(string csv)
        {
            return csv
                .Split(COMMA_SEPARATOR)
                .Select(item => int.Parse(item));
        }
        // TODO avoid duplicate code in CSV methods
        private static string IntsToCsv(IEnumerable<int> input)
        {
            if (!input.Any())
            {
                return "";
            }

            StringBuilder result = input
                .Aggregate(new StringBuilder(), (acc, item) => acc.Append(item).Append(COMMA_SEPARATOR));
            result.Length--;

            return result.ToString();
        }
        private static string StringsToCsv(IEnumerable<string> input)
        {
            if (!input.Any())
            {
                return "";
            }

            StringBuilder result = input
                .Aggregate(new StringBuilder(), (acc, item) => acc.Append(item).Append(COMMA_SEPARATOR));
            result.Length--;

            return result.ToString();
        }
        private static IEnumerable<string> CsvToStrings(string csv)
        {
            return csv.Split(COMMA_SEPARATOR);
        }

        private Skill SkillDataToSkill(SkillData data)
        {
            Skill skill;

            Type suffixType = GetType(data.SuffixClassName);
            Suffix suffix = (Suffix)Activator.CreateInstance(suffixType, new { data.X, data.Y });

            IEnumerable<Prefix> prefixes = data.PrefixesClassNames
                .Split(COMMA_SEPARATOR)
                .Select(prefix => (Prefix)Activator.CreateInstance(GetType(prefix)));

            skill = new Skill(prefixes, suffix);

            return skill;
        }
        private static SkillData SkillToSkillData(Skill skill)
        {
            SkillData data;

            string[] prefixNames = skill.Prefixes
                .Select(prefix => GetTypeName(prefix))
                .ToArray();

            string suffixName = GetTypeName(skill.Suffix);
            int x = skill.Suffix.X;
            int y = skill.Suffix.Y;


            int skillHash = HashCode
                .OfEach(prefixNames)
                .And(suffixName)
                .And(x)
                .And(y);

            string prefixesCsv = StringsToCsv(prefixNames);

            data = new SkillData
            {
                Hash = skillHash,
                PrefixesClassNames = prefixesCsv,
                SuffixClassName = suffixName,
                X = x,
                Y = y,
            };

            return data;
        }

        private static CardDefinition.ECardRarity TextToRarity(string text)
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

        private static int HashText(string text)
        {
            return HashCode.Of(text);
        }
        private static Type GetType(string typeName)
        {
            return Type.GetType(typeName);
        }
        private static string GetTypeName(object o)
        {
            return o.GetType().Name;
        }
    }
}
