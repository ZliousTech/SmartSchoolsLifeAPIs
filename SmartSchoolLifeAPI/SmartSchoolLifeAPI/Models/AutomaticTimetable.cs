using System.ComponentModel.DataAnnotations;

namespace SmartSchoolLifeAPI.Models
{
    public class AutomaticTimetable
    {
        [Key]
        public int TimetableItemID { get; set; }
        public int SchoolID { get; set; }
        public string SchoolYear { get; set; }
        public int SessionID { get; set; }
    }
}
