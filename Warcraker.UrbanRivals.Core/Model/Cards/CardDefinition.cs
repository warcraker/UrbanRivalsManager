using Warcraker.UrbanRivals.Core.Model.Cards.Skills;

namespace Warcraker.UrbanRivals.Core.Model.Cards
{
    public class CardDefinition
    {
        public enum ECardRarity
        {
            Unknown = 0,
            Common,
            Uncommon,
            Rare,
            Collector,
            Legendary,
            Mythic,
        }

        public int GameId { get; private set; }
        public string Name { get; private set; }
        public Clan Clan { get; private set; }
        public Skill Ability { get; private set; }
        public int AbilityUnlockLevel { get; set; }
        public CardStats Stats { get; private set; }
        public ECardRarity Rarity { get; private set; }

        public CardDefinition(int gameId, string name, Clan clan, Skill ability, int abilityUnlockLevel, CardStats stats, ECardRarity rarity)
        {
            this.GameId = gameId;
            this.Name = name;
            this.Clan = clan;
            this.Ability = ability;
            this.AbilityUnlockLevel = abilityUnlockLevel;
            this.Stats = stats;
            this.Rarity = rarity;
        }
    }
}
