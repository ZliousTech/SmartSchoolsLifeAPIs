using System.ComponentModel.DataAnnotations;

namespace SmartSchoolLifeAPI.Core.Models
{
    public class Subjects
    {
        [Key]
        public int SubjectID { get; set; }
        public string SubjectArabicName { get; set; }
        public string SubjectEnglishName { get; set; }
    }
}
