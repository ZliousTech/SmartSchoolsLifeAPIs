using Newtonsoft.Json;
using SmartSchoolLifeAPI.Core.Models.Shared;
using System;

namespace SmartSchoolLifeAPI.ViewModels
{
    public class HomeWorkVM : SharedModel
    {
        public int HomeWorkID { set; get; }
        public int SubjectID { set; get; }
        public string SubjectArabicName { set; get; }
        public string SubjectEnglishName { set; get; }
        public string HomeWorkTitle { set; get; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime HomeWorkCreationDate { set; get; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime HomeWorkDeadLine { set; get; }

        public string HomeWorkAttachment { set; get; }
        public string Ext { set; get; }
        public string HomeWorkNote { set; get; }
        public string TeacherID { set; get; }
        public string TeacherArabicName { set; get; }
        public string TeacherEnglishName { set; get; }
    }
}