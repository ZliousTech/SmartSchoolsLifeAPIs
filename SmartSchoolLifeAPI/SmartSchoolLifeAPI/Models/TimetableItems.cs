using System.ComponentModel.DataAnnotations;

namespace SmartSchoolLifeAPI.Models
{
    public class TimetableItems
    {
        [Key]
        public int TimetableItemID { get; set; }
        public string TeacherID { get; set; }
        public int SubjectID { get; set; }
        public int SectionID { get; set; }
        public int SchoolClassID { get; set; }
        public string ItemRGBColor { get; set; }

    }
}
