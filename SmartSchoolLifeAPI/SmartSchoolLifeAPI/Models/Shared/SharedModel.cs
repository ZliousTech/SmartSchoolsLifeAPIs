using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SmartSchoolLifeAPI.Models.Shared
{
    public class SharedModel
    {
        public int ID { get; set; }
        public int SchoolID { get; set; }
        public string SchoolYear { get; set; }
        public int? SchoolClassID { get; set; }
        public string SchoolClassArabicName { get; set; }
        public string SchoolClassEnglishName { get; set; }
        public int? SectionID { get; set; }
        public string SectionArabicName { get; set; }
        public string SectionEnglishName { get; set; }

        public bool ShouldSerializeID() => ID != default(int);
        public bool ShouldSerializeSchoolID() => SchoolID != default(int);
        public bool ShouldSerializeSchoolYear() => !string.IsNullOrEmpty(SchoolYear);
        public bool ShouldSerializeSchoolClassID() => SchoolClassID != default(int);
        public bool ShouldSerializeSchoolClassArabicName() => !string.IsNullOrEmpty(SchoolClassArabicName);
        public bool ShouldSerializeSchoolClassEnglishName() => !string.IsNullOrEmpty(SchoolClassEnglishName);
        public bool ShouldSerializeSectionID() => SectionID != default(int);
        public bool ShouldSerializeSectionArabicName() => !string.IsNullOrEmpty(SectionArabicName);
        public bool ShouldSerializeSectionEnglishName() => !string.IsNullOrEmpty(SectionEnglishName);
    }
}
