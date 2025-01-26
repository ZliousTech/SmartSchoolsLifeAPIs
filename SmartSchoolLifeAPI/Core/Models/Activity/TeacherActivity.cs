using System.Collections.Generic;

namespace SmartSchoolLifeAPI.Core.Models.Activity
{
    public class TeacherActivity
    {
        public int ID { get; set; }
        public int SchoolID { get; set; }
        public string SchoolYear { get; set; }
        public int SchoolClassID { get; set; }
        public int SectionID { get; set; }
        public string Photo { get; set; }
        public string ArabicHeader { get; set; }
        public string EnglishHeader { get; set; }
        public string ArabicDescription { get; set; }
        public string EnglishDescription { get; set; }
        public int OccasionType { get; set; }
        public string StartingDate { get; set; }
        public int NumberofDays { get; set; }
        public bool Vacation { get; set; }
        public string TeacherID { get; set; }
        public List<string> Students { get; set; }
    }
}