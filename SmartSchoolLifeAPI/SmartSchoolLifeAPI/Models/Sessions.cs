using System.ComponentModel.DataAnnotations;

namespace SmartSchoolLifeAPI.Models
{
    public class Sessions
    {
        [Key]
        public int SessionID { get; set; }
        public int WeekDay { get; set; }
        public int SessionDayOrder { get; set; }

    }
}
