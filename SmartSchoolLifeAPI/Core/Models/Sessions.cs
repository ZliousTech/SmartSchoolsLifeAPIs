using System.ComponentModel.DataAnnotations;

namespace SmartSchoolLifeAPI.Core.Models
{
    public class Sessions
    {
        [Key]
        public int SessionID { get; set; }
        public int SchoolID { get; set; }
        public string SchoolYear { get; set; }
        public int SchoolClassID { get; set; }
        public int SectionID { get; set; }
        public int WeekDay { get; set; }
        public int SessionDayOrder { get; set; }
        public string SessionTime { get; set; }
        public int Available { get; set; }

    }
}
