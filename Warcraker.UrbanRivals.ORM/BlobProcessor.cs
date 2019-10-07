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

namespace Warcraker.UrbanRivals.TextProcess
{
    public class BlobProcessor
    {
        private StaticGameDataRepository repository; 

        public BlobProcessor(StaticGameDataRepository repository)
        {
            this.repository = repository;
        }

        public Cycle GetCycleData(string blob) 
        {
            Cycle cycle;

            int blobHash = HashText(blob);
            BlobCycleData blobData = repository.GetBlobCycleData(blobHash);
            if (blobData != null)
            {
                IDictionary<int, Clan> clans = new Dictionary<int, Clan>();
                foreach (int clanHash in blobData.ClanHashes)
                {
                    ClanData clanData = repository.GetClanData(clanHash);
                    SkillData bonusData = repository.GetSkillData(clanData.BonusHash);
                    Skill bonus = GetSkillFromData(bonusData);
                    Clan clan = new Clan(clanData.GameId, clanData.Name, bonus);
                    clans[clan.GameId] = clan;
                }
                var cards = new List<CardDefinition>();
                for (int i = 0, n = blobData.CardDefinitionHashes.Length; i < n; i++)
                {
                    CardDefinitionData cardData = repository.GetCardDefinitionData(blobData.CardDefinitionHashes[i]);
                    SkillData abilityData = repository.GetSkillData(blobData.AbilityHashes[i]);
                    Skill ability = GetSkillFromData(abilityData);
                    Clan clan = clans[cardData.ClanGameId];
                    CardStats stats = new CardStats(cardData.InitialLevel, cardData.PowerPerLevel, cardData.DamagePerLevel);

                    CardDefinition card = new CardDefinition(cardData.GameId, cardData.Name, clan, ability, cardData.AbilityUnlockLevel, 
                        stats, (CardDefinition.ECardRarity)cardData.Rarity);
                    cards.Add(card);
                }

                cycle = new Cycle(Cycle.ECycleType.Day, cards); // TODO detect day/night
            }
            else
            {
                var clanItems = SortBlobIntoDictionary(new ApiCallList.Characters.GetClans(), blob);
                var abilityItems = SortBlobIntoDictionary(new ApiCallList.Characters.GetCharacters(), blob);
                var cardItems = SortBlobIntoDictionary(new ApiCallList.Urc.GetCharacters(), blob);

                foreach (var item in clanItems)
                {
                    dynamic decoded = JsonConvert.DeserializeObject(item.Value);
                    int id = int.Parse(decoded["id"].ToString());
                    string name = decoded["name"].ToString();
                    string bonusText = decoded["bonusDescription"].ToString();

                    Skill bonus;
                    SkillData bonusData;
                    int bonusTextHash = HashText(bonusText);
                    int bonusHash = repository.GetSkillHashFromTextHash(bonusTextHash);
                    if (bonusHash == -1)
                    {
                        bonus = SkillProcessor.ParseSkill(bonusText);
                        bonusHash = HashSkill(bonus);
                        bonusData = new SkillData
                        {
                            Hash = bonusHash,
                            PrefixesClassNames = bonus.Prefixes.Select(prefix => prefix.GetType().Name).ToArray(),
                            SuffixClassName = bonus.Suffix.GetType().Name,
                            X = bonus.X,
                            Y = bonus.Y,
                        };
                    }
                    else
                    {
                        bonusData = repository.GetSkillData(bonusHash);
                        bonus = GetSkillFromData(bonusData);
                    }
                }

                throw new NotImplementedException();
            }

            return cycle;
        }

        private static Dictionary<int, string> SortBlobIntoDictionary(ApiCall call, string blob)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            dynamic decoded = JsonConvert.DeserializeObject(blob);
            foreach (dynamic item in decoded[call.Call]["items"])
            {
                int id = int.Parse(item["id"].ToString());
                result.Add(id, item.ToString());
            }
            return result;
        }
        private static Skill GetSkillFromData(SkillData data)
        {
            Skill skill;

            Suffix suffix = (Suffix)BuildInstance(data.SuffixClassName);
            IEnumerable<Prefix> prefixes = data.PrefixesClassNames.Select(prefix => (Prefix)BuildInstance(prefix));
            skill = Skill.GetStandardSkill(prefixes, suffix, data.X, data.Y);

            return skill;
        }
        private static object BuildInstance(string name)
        {
            return Activator.CreateInstance(Type.GetType(name));
        }
        public static int HashText(string text)
        {
            int hashValue = HashCode.Of(text);

            return hashValue;
        }
        public static int HashSkill(Skill skill)
        {
            int hashValue = HashCode
                .OfEach(skill.Prefixes.Select(p => p.GetType().Name))
                .And(skill.Suffix.GetType().Name)
                .And(skill.X)
                .And(skill.Y);

            return hashValue;
        }
    }
}
