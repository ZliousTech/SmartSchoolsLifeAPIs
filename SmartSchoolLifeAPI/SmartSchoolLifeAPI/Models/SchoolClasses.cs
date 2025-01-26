using System.ComponentModel.DataAnnotations;

namespace SmartSchoolLifeAPI.Models
{
    public class SchoolClasses
    {
        [Key]
        public int SchoolClassID { get; set; }
        public int SchoolID { get; set; }
        public string SchoolClassArabicName { get; set; }
        public string SchoolClassEnglishName { get; set; }
    }
}
