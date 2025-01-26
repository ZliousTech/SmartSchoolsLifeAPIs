﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartSchoolLifeAPI.Models.Attendance
{
    public class AttendanceTypeModel : AbsenceReasonsModel
    {
        public int AttendanceTypeID { get; set; }
        public string AttendanceTypeArabicText { get; set; }
        public string AttendanceTypeEnglishText { get; set; }

        public bool ShouldSerializeAttendanceTypeID() => AttendanceTypeID != default(int);
        public bool ShouldSerializeAttedanceTypeArabicText() => !string.IsNullOrEmpty(AttendanceTypeArabicText);
        public bool ShouldSerializeAttedanceTypeEnglishText() => !string.IsNullOrEmpty(AttendanceTypeEnglishText);
    }
}