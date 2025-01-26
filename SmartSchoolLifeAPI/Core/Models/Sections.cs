using System.ComponentModel.DataAnnotations;

namespace SmartSchoolLifeAPI.Core.Models
{
    public class Sections
    {
        [Key]
        public int SectionID { get; set; }
        public int SchoolID { get; set; }
        public int SchoolClassID { get; set; }
        public string SectionCode { get; set; }
        public string SectionArabicName { get; set; }
        public string SectionEnglishName { get; set; }
    }
}
