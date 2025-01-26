using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartSchoolLifeAPI.ViewModels
{
    public class SchoolBanchesMV
    {
        public int SchoolID { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public string SchoolArabicName { get; set; }
        public string SchoolEnglishName { get; set; }
    }
}