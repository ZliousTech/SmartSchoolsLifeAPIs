using System.ComponentModel.DataAnnotations;

namespace SmartSchoolLifeAPI.Core.Models
{
    public class SystemSettings
    {
        [Key]
        public string CurrentAcademicYear { get; set; }

        public string DocumentsPath { get; set; }

        public int LastDocumentID { get; set; }

        public int LastStaffID { get; set; }

        public int LastStudentID { get; set; }

    }
}
