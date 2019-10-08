using SQLite;

namespace Warcraker.UrbanRivals.DataRepository.DataModels
{
    [Table(nameof(TextToSkillData))]
    public class TextToSkillData
    {
        [PrimaryKey]
        [Column(nameof(TextHash))]
        public int TextHash { get; set; }

        [Column(nameof(SkillHash))]
        public int SkillHash { get; set; }
    }
}
