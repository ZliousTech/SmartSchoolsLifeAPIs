using Newtonsoft.Json.Converters;

namespace SmartSchoolLifeAPI.Core.Models.Helpers
{
    public class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            DateTimeFormat = "dd-MM-yyyy hh:mm:ss tt"; // Define your desired date format
        }
    }
}