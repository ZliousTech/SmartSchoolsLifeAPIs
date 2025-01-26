using System.Collections.Generic;

namespace SmartSchoolLifeAPI.Core.DTOs
{
    public class StudentAttendanceByParentDTO
    {
        public int SchoolId { set; get; }
        public string SchoolYear { set; get; }
        public string StudentId { set; get; }
        public string FromDate { set; get; }
        public string ToDate { set; get; }
        public string Note { set; get; }
        public IEnumerable<int> Sessions { set; get; }
        public string Attachment { set; get; }
        public string Ext { set; get; }
    }
}