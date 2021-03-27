using HashUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UrbanRivalsApiManager;
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
        private static readonly char COMMA_SEPARATOR = ';';

        private readonly GameDataRepository repository;
        private readonly IDictionary<string, string> fullAssembliesByClassName;
        private readonly IDictionary<string, Prefix> prefixesByName;

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

            this.prefixesByName = new Dictionary<string, Prefix>();
            IEnumerable<KeyValuePair<string, string>> prefixesToCreate = this.fullAssembliesByClassName.Where(a => a.Key.EndsWith(prefixEnding) && a.Key != prefixEnding);
            foreach (KeyValuePair<string, string> prefixAssemblyName in prefixesToCreate)
            {
                Prefix prefix = (Prefix)Type.GetType(prefixAssemblyName.Value).GetConstructor(Type.EmptyTypes).Invoke(null);
                this.prefixesByName.Add(prefixAssemblyName.Key, prefix);
            }

            this.repository = repository;
        }

        // TODO use localizable values for IProgress calls
        public Cycle GetCycleData(string blob, IProgress<string> progress)
        {
            int blobHash = HashText(blob);
            CycleData cycleData = this.repository.GetCycleData(blobHash);
            if (cycleData == null)
            {
                progress.Report("Blob NOT found on database");
                cycleData = ParseAndStoreCycle(blob, progress);
            }
            else
            {
                progress.Report("Blob found on database");
            }

            IDictionary<int, Clan> clans = new Dictionary<int, Clan>();
            int[] clanHashes = CsvToInts(cycleData.ClanHashes).ToArray();
            foreach (int clanHash in clanHashes)
            {
                ClanData clanData = this.repository.GetClanData(clanHash);
                SkillData bonusData = this.repository.GetSkillData(clanData.BonusHash);
                Skill bonus = SkillDataToSkill(bonusData);
                var clan = new Clan(clanData.GameId, clanData.Name, bonus);
                clans[clan.GameId] = clan;
            }

            IList<CardDefinition> cards = new List<CardDefinition>();
            int[] cardHashes = CsvToInts(cycleData.CardHashes).ToArray();
            int[] abilityHashes = CsvToInts(cycleData.AbilityHashes).ToArray();

            for (int i = 0, n = cardHashes.Length; i < n; i++)
            {
                CardData cardData = this.repository.GetCardData(cardHashes[i]);
                SkillData abilityData = this.repository.GetSkillData(abilityHashes[i]);
                Skill ability = SkillDataToSkill(abilityData);
                Clan clan = clans[cardData.ClanGameId];
                var stats = new CardStats(cardData.InitialLevel,
                    CsvToInts(cardData.PowerPerLevel), CsvToInts(cardData.DamagePerLevel));
                var card = new CardDefinition(cardData.GameId, cardData.Name, clan, ability,
                    cardData.AbilityUnlockLevel, stats, (CardDefinition.ECardRarity)cardData.Rarity);

                cards.Add(card);
            }

            Cycle.ECycleType cycleType = Cycle.ECycleType.Day; // TODO detect day/night
            var cycle = new Cycle(cycleType, cards);

            return cycle;
        }

        private CycleData ParseAndStoreCycle(string blob, IProgress<string> progress)
        {
            CycleData cycleData;
            Dictionary<int, string> abilityItems = BlobToDictionary<ApiCallList.Characters.GetCharacters>(blob);
            Dictionary<int, string> cardItems = BlobToDictionary<ApiCallList.Urc.GetCharacters>(blob);
            Asserts.Check(abilityItems.Count == cardItems.Count, $"Characters.GetCharacters and Urc.GetCharacters call counts must be equal");
            Dictionary<int, string> clanItems = BlobToDictionary<ApiCallList.Characters.GetClans>(blob);
            progress.Report($"Cards: {cardItems.Count}. Clans: {clanItems.Count}");

            IList<int> clanHashes = new List<int>();
            foreach (int clanId in clanItems.Keys)
            {
                string clanItem = clanItems[clanId];
                int clanHash = HashText(clanItem);
                ClanData clanData = this.repository.GetClanData(clanHash);
                if (clanData == null)
                {
                    progress.Report("Clan NOT found");

                    dynamic decodedClan = JsonConvert.DeserializeObject(clanItem);
                    string bonusText = decodedClan["bonusDescription"].ToString();

                    int bonusTextHash = HashText(bonusText);
                    int bonusHash;
                    if (!this.repository.TryGetSkillHashFromTextHash(bonusTextHash, out bonusHash))
                    {
                        progress.Report("Bonus NOT found");
                        Skill bonus = SkillProcessor.ParseSkill(bonusText);
                        SkillData bonusData = SkillToSkillData(bonus);
                        this.repository.SaveSkillData(bonusData, bonusTextHash);
                        bonusHash = bonusData.Hash;

                        progress.Report($"SkillText: {bonusText}");
                        progress.Report($"Prefixes: {bonusData.PrefixesClassNames}");
                        progress.Report($"Suffix: {bonusData.SuffixClassName} ({bonusData.X}.{bonusData.Y})");
                    }
                    else
                    {
                        progress.Report($"Bonus found [#{bonusHash}]");
                    }

                    string clanName = decodedClan["name"].ToString();
                    clanData = new ClanData
                    {
                        Hash = clanHash,
                        BonusHash = bonusHash,
                        Name = clanName,
                        GameId = clanId,
                    };
                    this.repository.SaveClanData(clanData);
                }
                else
                {
                    progress.Report($"Clan found [#{clanHash}]");
                }

                progress.Report($"Clan: [{clanData.GameId}] {clanData.Name}");
                clanHashes.Add(clanHash);
            }

            IList<int> abilityHashes = new List<int>();
            IList<int> cardHashes = new List<int>();

            int numberOfCardsRecovered = cardItems.Count;
            int currentCardIndex = 0;

            foreach (int cardId in cardItems.Keys)
            {
                currentCardIndex++;
                progress.Report($"Processing card {currentCardIndex}/{numberOfCardsRecovered}. Id {cardId}.");

                string abilityItem = abilityItems[cardId];
                dynamic decodedAbility = JsonConvert.DeserializeObject(abilityItem);
                string abilityText = decodedAbility["ability"].ToString();

                int abilityTextHash = HashText(abilityText);
                int abilityHash;
                if (this.repository.TryGetSkillHashFromTextHash(abilityTextHash, out abilityHash))
                {
                    progress.Report($"Ability found [#{abilityHash}]");
                }
                else
                {
                    progress.Report("Ability NOT found");
                    Skill ability = SkillProcessor.ParseSkill(abilityText);
                    SkillData abilityData = SkillToSkillData(ability);
                    this.repository.SaveSkillData(abilityData, abilityTextHash);
                    abilityHash = abilityData.Hash;

                    progress.Report($"SkillText: {abilityText}");
                    progress.Report($"Prefixes: {abilityData.PrefixesClassNames}");
                    progress.Report($"Suffix: {abilityData.SuffixClassName} ({abilityData.X}.{abilityData.Y})");
                }

                string cardItem = cardItems[cardId];
                int cardHash = HashText(cardItem);
                CardData cardData = this.repository.GetCardData(cardHash);
                if (cardData == null)
                {
                    progress.Report("Card NOT found");
                    int abilityUnlockLevel = int.Parse(decodedAbility["ability_unlock_level"].ToString());
                    dynamic decodedCard = JsonConvert.DeserializeObject(cardItem);
                    string cardName = decodedCard["name"].ToString();
                    int clanId = int.Parse(decodedCard["clanID"].ToString());
                    string rarityText = decodedCard["rarity"].ToString();
                    CardDefinition.ECardRarity rarity = TextToRarity(rarityText);

                    IEnumerable<dynamic> levels = decodedCard["levels"];
                    int initialLevel = int.Parse(levels.First()["level"].ToString());

                    var powers = new List<int>();
                    var damages = new List<int>();
                    foreach (dynamic levelItem in levels)
                    {
                        powers.Add(int.Parse(levelItem["power"].ToString()));
                        damages.Add(int.Parse(levelItem["damage"].ToString()));
                    }

                    cardData = new CardData
                    {
                        Hash = cardHash,
                        GameId = cardId,
                        Name = cardName,
                        ClanGameId = clanId,
                        InitialLevel = initialLevel,
                        AbilityUnlockLevel = abilityUnlockLevel,
                        Rarity = (int)rarity,
                        PowerPerLevel = IntsToCsv(powers),
                        DamagePerLevel = IntsToCsv(damages),
                    };
                    this.repository.SaveCardData(cardData);
                }
                else
                {
                    progress.Report($"Card found [#{cardHash}]");
                }

                progress.Report($"Card: [{cardData.GameId}]{cardData.Name}");
                int levelVariations = cardData.PowerPerLevel.Split(COMMA_SEPARATOR).Count();
                progress.Report($"Levels: {cardData.InitialLevel}-{cardData.InitialLevel + levelVariations}");
                progress.Report($"Powers: {cardData.PowerPerLevel}");
                progress.Report($"Damages: {cardData.DamagePerLevel}");
                progress.Report($"Ability unlock level: {cardData.AbilityUnlockLevel}");

                int cardGameIdFromAbility = int.Parse(decodedAbility["id"].ToString());
                Asserts.Check(cardGameIdFromAbility == cardData.GameId,
                    "Characters.GetCharacters and Urc.GetCharacters items must be in the same order");

                cardHashes.Add(cardHash);
                abilityHashes.Add(abilityHash);
            }

            cycleData = new CycleData
            {
                Hash = HashText(blob),
                AbilityHashes = IntsToCsv(abilityHashes),
                CardHashes = IntsToCsv(cardHashes),
                ClanHashes = IntsToCsv(clanHashes),
            };
            this.repository.SaveCycleBlobData(cycleData);

            return cycleData;
        }

        private static Dictionary<int, string> BlobToDictionary<TCall>(string blob) where TCall : ApiCall, new()
        {
            var result = new Dictionary<int, string>();

            var call = new TCall();
            dynamic decoded = JsonConvert.DeserializeObject(blob);
            IEnumerable<dynamic> items = decoded[call.Call]["items"];
            IEnumerable<dynamic> orderedItems = items.OrderBy(x => x["id"].Value);
            foreach (dynamic item in items)
            {
                int id = int.Parse(item["id"].ToString());
                string text = item.ToString();
                result.Add(id, text);
            }
            return result;
        }
        private static IEnumerable<int> CsvToInts(string csv)
        {
            return csv
                .Split(COMMA_SEPARATOR)
                .Select(item => int.Parse(item));
        }
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

            if (data.SuffixClassName == nameof(NoAbility))
            {
                skill = NoAbility.INSTANCE;
            }
            else if (data.SuffixClassName == nameof(UnknownSkill))
            {
                skill = UnknownSkill.INSTANCE;
            }
            else
            {
                object[] arguments;
                if (data.X == -1)
                {
                    arguments = new object[] { };
                }
                else if (data.Y == -1)
                {
                    arguments = new object[] { data.X };
                }
                else
                {
                    arguments = new object[] { data.X, data.Y };
                }

                Type suffixType = GetType(data.SuffixClassName);
                var suffix = (Suffix)Activator.CreateInstance(suffixType, arguments);

                IEnumerable<Prefix> prefixes;
                if (String.IsNullOrEmpty(data.PrefixesClassNames))
                {
                    prefixes = new Prefix[] { };
                }
                else
                {
                    prefixes = data.PrefixesClassNames
                        .Split(COMMA_SEPARATOR)
                        .Select(prefixName => this.prefixesByName[prefixName]);
                }

                skill = new Skill(prefixes, suffix);
            }

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
                    return CardDefinition.ECardRarity.Unknown;
            }
        }

        private static int HashText(string text)
        {
            return HashCode.Of(text);
        }
        private Type GetType(string typeName)
        {
            string fullAssemblyName;
            try
            {
                fullAssemblyName = this.fullAssembliesByClassName[typeName];
                return Type.GetType(fullAssemblyName);
            }
            catch (Exception ex)
            {
                Asserts.Fail($"Unable to parse type {typeName}. Reason: {ex.Message}");
                return typeof(UnknownSkill);
            }
        }
        private static string GetTypeName(object o)
        {
            return o.GetType().Name;
        }
    }
}
