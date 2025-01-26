using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartSchoolLifeAPI.Models
{
    public class Teacher
    {
        public int schoolID { get; set; }
        public Nullable<int> WeekStartingDay { get; set; }
    }
}
