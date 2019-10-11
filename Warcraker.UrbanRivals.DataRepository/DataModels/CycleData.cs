using SQLite;

namespace Warcraker.UrbanRivals.DataRepository.DataModels
{
    [Table(nameof(CycleData))]
    public class CycleData
    {
        [PrimaryKey]
        [Column(nameof(Hash))]
        public int Hash { get; set; }

        [Column(nameof(AbilityHashes))]
        public string AbilityHashes { get; set; }
        [Column(nameof(CardHashes))]
        public string CardHashes { get; set; }
        [Column(nameof(ClanHashes))]
        public string ClanHashes { get; set; }
    }
}
