using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace SmartSchoolLifeAPI.Models
{
    public class ManualTimetable
    {
        [Key]
        public int TimetableItemID { get; set; }
        public int SchoolID { get; set; }
        public string SchoolYear { get; set; }
        public int SessionID { get; set; }
    }
}
