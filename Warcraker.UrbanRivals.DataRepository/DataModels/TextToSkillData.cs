using SQLite;

namespace Warcraker.UrbanRivals.DataRepository.DataModels
{
    public class TextToSkillData
    {
        [PrimaryKey]
        public int TextHash { get; set; }
        public int SkillHash { get; set; }
    }
}
