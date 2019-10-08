using SQLite;

namespace Warcraker.UrbanRivals.DataRepository.DataModels
{
    [Table(nameof(ClanData))]
    public class ClanData
    {
        [PrimaryKey]
        [Column(nameof(Hash))]
        public int Hash { get; set; }

        [Column(nameof(BonusHash))]
        public int BonusHash { get; set; }
        [Column(nameof(GameId))]
        public int GameId { get; set; }
        [Column(nameof(Name))]
        public string Name { get; set; }
    }
}
