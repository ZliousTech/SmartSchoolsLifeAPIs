using SmartSchoolLifeAPI.Core.Models.Shared;

namespace SmartSchoolLifeAPI.Core.Models.Settings.Subject
{
    public class SubjectModel : SharedModel
    {
        public int SubjectID { get; set; }
        public string SubjectArabicName { set; get; }
        public string SubjectEnglishName { set; get; }
        public int? MaxMark { set; get; }
        public int? FailMark { set; get; }
        public int? NumberOfSessionsPerWeek { set; get; }
        public int SchedulingCondition { set; get; }
        public bool IsOptional { set; get; }
    }
}