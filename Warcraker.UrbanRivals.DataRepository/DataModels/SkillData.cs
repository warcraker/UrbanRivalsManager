using SQLite;

namespace Warcraker.UrbanRivals.DataRepository.DataModels
{
    public class SkillData
    {
        [PrimaryKey]
        public int Hash { get; set; }
        public string SuffixClassName { get; set; }
        public string PrefixesClassNames { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
