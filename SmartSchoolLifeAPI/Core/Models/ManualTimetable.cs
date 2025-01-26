using System.ComponentModel.DataAnnotations;

namespace SmartSchoolLifeAPI.Core.Models
{
    public class ManualTimetable
    {
        [Key]
        public int TimetableItemID { get; set; }
        public int TimetableEntityID { get; set; }
        public int SchoolID { get; set; }
        public string SchoolYear { get; set; }
        public int SchoolClassID { get; set; }
        public int SectionID { get; set; }
        public int SessionID { get; set; }
        public string TeacherID { get; set; }
    }
}
