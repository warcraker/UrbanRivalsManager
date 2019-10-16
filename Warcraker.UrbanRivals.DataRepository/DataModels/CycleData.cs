using SQLite;

namespace Warcraker.UrbanRivals.DataRepository.DataModels
{
    public class CycleData
    {
        [PrimaryKey]
        public int Hash { get; set; }
        public string AbilityHashes { get; set; }
        public string CardHashes { get; set; }
        public string ClanHashes { get; set; }
    }
}
