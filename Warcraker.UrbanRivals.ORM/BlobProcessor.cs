using HashUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private GameDataRepository repository;

        public BlobProcessor(GameDataRepository repository)
        {
            this.repository = repository;
        }

        public Cycle GetCycleData(string blob)
        {
            Cycle cycle;
            Cycle.ECycleType cycleType;
            IList<CardDefinition> cards = new List<CardDefinition>();

            int blobHash = HashText(blob);
            CycleData cycleData = repository.GetCycleData(blobHash);
            if (cycleData != null)
            {
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
                    CardStats stats = new CardStats(cardData.InitialLevel, cardData.PowerPerLevel, cardData.DamagePerLevel);

                    CardDefinition card = new CardDefinition(cardData.GameId, cardData.Name, clan, ability, cardData.AbilityUnlockLevel,
                        stats, (CardDefinition.ECardRarity)cardData.Rarity);
                    cards.Add(card);
                }

                cycleType = Cycle.ECycleType.Day; // TODO detect day/night
            }
            else
            {
                IList<string> clanItems = BlobToList<ApiCallList.Characters.GetClans>(blob);
                IList<string> abilityItems = BlobToList<ApiCallList.Characters.GetCharacters>(blob);
                IList<string> cardItems = BlobToList<ApiCallList.Urc.GetCharacters>(blob);

                Asserts.Check(abilityItems.Count == cardItems.Count, $"Characters.GetCharacters and Urc.GetCharacters call counts must be equal");

                IList<int> clanHashes = new List<int>();
                IList<int> abilityHashes = new List<int>();
                IList<int> cardHashes = new List<int>();

                IDictionary<int, Clan> clans = new Dictionary<int, Clan>();
                foreach (string clanItem in clanItems)
                {
                    Clan clan;
                    Skill bonus;
                    int clanHash = HashText(clanItem);
                    ClanData clanData = repository.GetClanData(clanHash);
                    if (clanData != null)
                    {
                        SkillData bonusData = repository.GetSkillData(clanData.BonusHash);
                        bonus = SkillDataToSkill(bonusData);
                    }
                    else
                    {
                        dynamic decodedClan = JsonConvert.DeserializeObject(clanItem);
                        string bonusText = decodedClan["bonusDescription"].ToString();

                        int bonusTextHash = HashText(bonusText);
                        int bonusHash = repository.GetSkillHashFromTextHash(bonusTextHash);
                        if (bonusHash == repository.SCALAR_VALUE_NOT_FOUND)
                        {
                            bonus = SkillProcessor.ParseSkill(bonusText);
                            SkillData bonusData = SkillToSkillData(bonus);
                            bonusHash = bonusData.Hash;

                            repository.SaveSkillData(bonusData);
                            repository.SaveSkillTextHash(bonusTextHash, bonusHash);
                        }
                        else
                        {
                            SkillData bonusData = repository.GetSkillData(bonusHash);
                            bonus = SkillDataToSkill(bonusData);
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

                    clan = new Clan(clanData.GameId, clanData.Name, bonus);
                    clans.Add(clan.GameId, clan);
                    clanHashes.Add(clanHash);
                }

                for (int i = 0, n = cardItems.Count; i < n; i++)
                {
                    Skill ability;

                    string abilityItem = abilityItems[i];
                    dynamic decodedAbility = JsonConvert.DeserializeObject(abilityItem);

                    int cardGameIdFromAbility = int.Parse(decodedAbility["id"].ToString());
                    string abilityText = decodedAbility["ability"].ToString();

                    int abilityTextHash = HashText(abilityText);
                    int abilityHash = repository.GetSkillHashFromTextHash(abilityTextHash);
                    if (abilityHash == repository.SCALAR_VALUE_NOT_FOUND)
                    {
                        ability = SkillProcessor.ParseSkill(abilityText);
                        SkillData abilityData = SkillToSkillData(ability);
                        abilityHash = abilityData.Hash;

                        repository.SaveSkillData(abilityData);
                        repository.SaveSkillTextHash(abilityTextHash, abilityHash);
                    }
                    else
                    {
                        SkillData abilityData = repository.GetSkillData(abilityHash);
                        ability = SkillDataToSkill(abilityData);
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

                        dynamic levels = decodedCard["levels"];
                        int initialLevel = int.Parse(levels[0]["level"].ToString());
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
                            Rarity = RarityTextToInt(rarityText),
                            PowerPerLevel = powers.ToArray(),
                            DamagePerLevel = damages.ToArray(),
                        };
                        repository.SaveCardData(cardData);
                    }

                    Clan clan = clans[cardData.ClanGameId];
                    CardStats stats = new CardStats(cardData.InitialLevel, cardData.PowerPerLevel, cardData.DamagePerLevel);
                    CardDefinition card = new CardDefinition(cardData.GameId, cardData.Name, clan, ability, cardData.AbilityUnlockLevel, stats, 0);

                    Asserts.Check(cardGameIdFromAbility == cardData.GameId, "Characters.GetCharacters and Urc.GetCharacters must be in the same order");

                    cards.Add(card);
                    cardHashes.Add(cardHash);
                    abilityHashes.Add(abilityHash);
                }

                cycleType = Cycle.ECycleType.Day; // TODO detect day/night
                cycleData = new CycleData
                {
                    BlobHash = blobHash,
                    AbilityHashes = abilityHashes.ToArray(),
                    CardHashes = cardHashes.ToArray(),
                    ClanHashes = clanHashes.ToArray(),
                };

                repository.SaveCycleBlobData(cycleData);
            }

            cycle = new Cycle(cycleType, cards);

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
        private static SkillData SkillToSkillData(Skill skill)
        {
            return new SkillData
            {
                Hash = HashSkill(skill),
                PrefixesClassNames = skill.Prefixes.Select(prefix => GetTypeName(prefix)).ToArray(),
                SuffixClassName = GetTypeName(skill.Suffix),
                X = skill.X,
                Y = skill.Y,
            };
        }
        private static Skill SkillDataToSkill(SkillData data)
        {
            Skill skill;

            Suffix suffix = (Suffix)BuildInstance(data.SuffixClassName);
            IEnumerable<Prefix> prefixes = data.PrefixesClassNames.Select(prefix => (Prefix)BuildInstance(prefix));
            skill = Skill.GetStandardSkill(prefixes, suffix, data.X, data.Y);

            return skill;
        }
        private static int RarityTextToInt(string text)
        {
            throw new NotImplementedException();
        }

        private static void SaveNewSkill()
        {

        }

        private static object BuildInstance(string name)
        {
            return Activator.CreateInstance(Type.GetType(name));
        }
        private static string GetTypeName(object o)
        {
            return o.GetType().Name;
        }
        private static int HashText(string text)
        {
            int hashValue = HashCode.Of(text);

            return hashValue;
        }
        private static int HashSkill(Skill skill)
        {
            int hashValue = HashCode
                .OfEach(skill.Prefixes.Select(prefix => GetTypeName(prefix)))
                .And(GetTypeName(skill.Suffix))
                .And(skill.X)
                .And(skill.Y);

            return hashValue;
        }
    }
}
