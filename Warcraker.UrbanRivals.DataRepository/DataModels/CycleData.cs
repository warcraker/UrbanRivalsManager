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
        public int[] AbilityHashes { get; set; }
        [Column(nameof(CardHashes))]
        public int[] CardHashes { get; set; }
        [Column(nameof(ClanHashes))]
        public int[] ClanHashes { get; set; }
    }
}
