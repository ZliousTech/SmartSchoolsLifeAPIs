using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SmartSchoolLifeAPI.Models
{
    public class SystemSettingsperSchool
    {
        [Key]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int SchoolID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int LastInvoiceNumber { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int OptionalArabicFields { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int OptionalEnglishFields { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int OptionalFamilyDetails { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int OptionalThirdName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int BusScheduleDateType { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int BusScheduleType { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int NumberofBusTrips { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int TimetableType { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int SubjectDistributionMethod { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int SubjectforClassorSection { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int CurrencyOptions { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int BusAttendantAssigningMethod { get; set; }
    }
}
