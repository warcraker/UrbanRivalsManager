namespace Warcraker.UrbanRivals.DataRepository.DataModels
{
    public class CardData
    {
        public int Hash { get; set; }
        public int GameId { get; set; }
        public int ClanGameId { get; set; }
        public string Name { get; set; }
        public int InitialLevel { get; set; }
        public int AbilityUnlockLevel { get; set; }
        public string PowerPerLevel { get; set; }
        public string DamagePerLevel { get; set; }
        public int Rarity { get; set; }
    }
}
