using System.ComponentModel.DataAnnotations;

namespace SmartSchoolLifeAPI.Core.Models
{
    public class Staff
    {
        [Key]
        public string StaffID { get; set; }
        public string StaffArabicName { get; set; }
        public string StaffEnglishName { get; set; }
    }
}
