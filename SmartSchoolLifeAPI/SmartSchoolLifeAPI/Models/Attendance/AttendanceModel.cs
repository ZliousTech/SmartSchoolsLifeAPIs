using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using SmartSchoolLifeAPI.Models.Shared;
using SmartSchoolLifeAPI.Models.Helpers;

namespace SmartSchoolLifeAPI.Models.Attendance
{
    public class AttendanceModel : AttendanceTypeModel
    {
        public int AttendanceItemID { set; get; }
        public int SchoolID { set; get; }
        public string SchoolYear { set; get; }
        public string StudentID { set; get; }
        public string AttendanceDate { set; get; }
        public int AttendanceType { set; get; }
        public int AbsenceReason { set; get; }
        public string Description { set; get; } = null;
        public bool ParentInformed { set; get; }
        public string ParentMessage { set; get; } = null;
        public string AttendanceNote { set; get; } = null;
        public bool FirstSession { set; get; } 
        public bool SecondSession { set; get; }
        public bool ThirdSession { set; get; }
        public bool FourthSession { set; get; }
        public bool FifthSession { set; get; }
        public bool SixthSession { set; get; }
        public bool SeventhSession { set; get; }
        public bool EighthSession { set; get; }

        // prop for helping frontends.
        public int NumberofSessionsPerDay { set; get; }

        //public string FirstSessionTimeStamp { set; get; } = null;
        //public string SecondSessionTimeStamp { set; get; } = null;
        //public string ThirdSessionTimeStamp { set; get; } = null;
        //public string FourthSessionTimeStamp { set; get; } = null;
        //public string FifthSessionTimeStamp { set; get; } = null;
        //public string SixthSessionTimeStamp { set; get; } = null;
        //public string SeventhSessionTimeStam { set; get; } = null;
        //public string EighthSessionTimeStamp { set; get; } = null;

        public bool ShouldSerializeSchoolID() => SchoolID != default(int);
        public bool ShouldSerializeSchoolYear() => !string.IsNullOrEmpty(SchoolYear);
        public bool ShouldSerializeAttendanceType() => AttendanceType != default(int);
        public bool ShouldSerializeAbsenceReason() => AbsenceReason != default(int);
    }
}