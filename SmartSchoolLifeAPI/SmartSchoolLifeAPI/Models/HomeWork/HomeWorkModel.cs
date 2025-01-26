using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartSchoolLifeAPI.Models.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SmartSchoolLifeAPI.Models.Helpers;
using System.Data.SqlClient;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;

namespace SmartSchoolLifeAPI.Models.HomeWork
{
    public class HomeWorkModel : SharedModel
    {
        public int HomeWorkID { set; get; }
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
    