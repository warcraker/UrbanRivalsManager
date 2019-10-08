using SQLite;

namespace Warcraker.UrbanRivals.DataRepository.DataModels
{
    [Table(nameof(CardData))]
    public class CardData
    {
        [PrimaryKey]
        [Column(nameof(Hash))]
        public int Hash { get; set; }

        [Column(nameof(GameId))]
        public int GameId { get; set; }
        [Column(nameof(ClanGameId))]
        public int ClanGameId { get; set; }
        [Column(nameof(Name))]
        public string Name { get; set; }
        [Column(nameof(InitialLevel))]
        public int InitialLevel { get; set; }
        [Column(nameof(AbilityUnlockLevel))]
        public int AbilityUnlockLevel { get; set; }
        [Column(nameof(PowerPerLevel))]
        public int[] PowerPerLevel { get; set; }
        [Column(nameof(DamagePerLevel))]
        public int[] DamagePerLevel { get; set; }
        [Column(nameof(Rarity))]
        public int Rarity { get; set; }
    }
}
