using SQLite;

namespace Warcraker.UrbanRivals.DataRepository.DataModels
{
    public class ClanData
    {
        [PrimaryKey]
        public int Hash { get; set; }
        public int BonusHash { get; set; }
        public int GameId { get; set; }
        public string Name { get; set; }
    }
}
