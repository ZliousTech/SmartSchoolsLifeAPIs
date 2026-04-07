using SmartSchool.Core.DataEntityTier;
using System.Collections.Generic;

namespace SmartSchoolAPI.Entities
{
    public class QuickAttendanceRequest
    {
        public List<QuickAttendanceData> AbsenceStudentData { get; set; }
        public int SchoolId { get; set; }
        public string TeacherId { get; set; }
    }
}
