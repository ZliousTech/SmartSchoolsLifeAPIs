using System.ComponentModel.DataAnnotations;

namespace SmartSchoolLifeAPI.Core.Models
{
    public class TimetableItems
    {
        [Key]
        public int TimetableItemID { get; set; }
        public int SchoolID { get; set; }
        public string SchoolYear { get; set; }
        public int SchoolClassID { get; set; }
        public int SectionID { get; set; }
        public int SubjectID { get; set; }
        public string TeacherID { get; set; }
        public string ItemRGBColor { get; set; }

    }
}
