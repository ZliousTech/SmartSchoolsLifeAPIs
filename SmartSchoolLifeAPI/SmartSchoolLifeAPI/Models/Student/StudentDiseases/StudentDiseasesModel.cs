using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using SmartSchoolLifeAPI.Models.Shared;

namespace SmartSchoolLifeAPI.Models.Student.StudentDiseases
{
    public class StudentDiseasesModel
    {
        public int DiseasesID { get; set; }
        public int StudentID { get; set; }
        public Nullable<bool> Mumps { get; set; }
        public Nullable<bool> Chickenpox { get; set; }
        public Nullable<bool> rubella { get; set; }
        public Nullable<bool> Rheumaticfever { get; set; }
        public Nullable<bool> Diabetes { get; set; }
        public Nullable<bool> Heartdiseases { get; set; }
        public Nullable<bool> Pissingoff { get; set; }
        public Nullable<bool> Jointbonediseases { get; set; }
        public Nullable<bool> sprayer { get; set; }
        public Nullable<bool> Hearingimpairment { get; set; }
        public Nullable<bool> Visualimpairment { get; set; }
        public Nullable<bool> Speechimpairment { get; set; }
        public Nullable<bool> Bladderdiseases { get; set; }
        public Nullable<bool> Epilepsy { get; set; }
        public Nullable<bool> Hepatitis { get; set; }
        public Nullable<bool> Shakika { get; set; }
        public Nullable<bool> Fainting { get; set; }
        public Nullable<bool> Kidneydisease { get; set; }
        public Nullable<bool> Surgery { get; set; }
        public Nullable<bool> Urinarysystemdiseases { get; set; }
        public string InternalStudentID { get; set; }
    }
}