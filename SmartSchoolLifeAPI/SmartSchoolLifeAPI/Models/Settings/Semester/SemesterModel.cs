using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartSchoolLifeAPI.Models.Shared;

namespace SmartSchoolLifeAPI.Models.Settings.Semester
{
    public class SemesterModel : SharedModel
    {
        public string SemesterArabicName { get; set; }
        public string SemesterEnglishName { get; set; }
        public string SemesterName { get; set; }
    }
}