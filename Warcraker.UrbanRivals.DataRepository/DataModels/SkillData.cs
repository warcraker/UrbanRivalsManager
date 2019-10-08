using SQLite;

namespace Warcraker.UrbanRivals.DataRepository.DataModels
{
    [Table(nameof(SkillData))]
    public class SkillData
    {
        [PrimaryKey]
        [Column(nameof(Hash))]
        public int Hash { get; set; }

        [Column(nameof(SuffixClassName))]
        public string SuffixClassName { get; set; }
        [Column(nameof(PrefixesClassNames))]
        public string[] PrefixesClassNames { get; set; }
        [Column(nameof(X))]
        public int X { get; set; }
        [Column(nameof(Y))]
        public int Y { get; set; }
    }
}
