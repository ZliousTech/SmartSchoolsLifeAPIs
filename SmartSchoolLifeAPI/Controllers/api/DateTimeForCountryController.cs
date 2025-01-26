using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Http;

namespace SmartSchoolLifeAPI.Controllers.api
{
    public class DateTimeForCountryController : ApiController
    {
        DateTime thisTime;
        TimeZoneInfo tst;
        DateTime tstTime;

        public DateTimeForCountryController()
        {
            thisTime = DateTime.Now;
        }

        [HttpGet]
        public string GetDateTimeForCountry(string TimeZoneID)
        {
            try
            {
                tst = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneID);
                tstTime = TimeZoneInfo.ConvertTime(thisTime, TimeZoneInfo.Local, tst);

                string[] TimeStr = tstTime.ToString().Split(' ');
                string TimeStr24 = TimeStr[1] + TimeStr[2];
                DateTime TimeStr_Date = DateTime.Parse(TimeStr24);
                TimeStr24 = TimeStr_Date.ToString("HH:mm:ss");

                return TimeStr24;
            }
            catch
            {
                return "00:00:00";
            }
        }

        [HttpGet]
        public string CountryDateTime(string lat, string lng)
        {
            try
            {
                int i = 0;
                string CountryShortName = "xxx";
                string CountryTimeZone = "Jordan Standard Time";
                string jsonData = GetCountryShortNameByLocation(lat, lng, ConfigurationManager.AppSettings["GoogleAPIKey"]);
                JObject JObj = JObject.Parse(jsonData);

                bool Looking = true;
                string sss = "JO";
                for (i = 0; Looking && (string)JObj.SelectToken("results[0].address_components[" + i + "].types[0]") != "country"; i++)
                {
                    sss = (string)JObj.SelectToken("results[0].address_components[" + i + "].types[0]");
                    if (sss == null) { sss = "NotFound"; Looking = false; }
                }

                if (sss == "NotFound") CountryShortName = "JO";
                else CountryShortName = (string)JObj.SelectToken("results[0].address_components[" + i + "].short_name");

                CountryTimeZone = GetTimeZoneID(CountryShortName);

                tst = TimeZoneInfo.FindSystemTimeZoneById(CountryTimeZone);
                tstTime = TimeZoneInfo.ConvertTime(thisTime, TimeZoneInfo.Local, tst);

                string[] TimeStr = tstTime.ToString().Split(' ');
                string TimeStr24 = TimeStr[1] + TimeStr[2];
                DateTime TimeStr_Date = DateTime.Parse(TimeStr24);
                TimeStr24 = TimeStr_Date.ToString("HH:mm:ss");

                return TimeStr24;
            }
            catch
            {
                return "00:00:00";
            }
        }

        public string GetCountryShortNameByLocation(string lat, string lng, string keyString)
        {
            try
            {
                string urlRequest = "";
                urlRequest = @"https://maps.googleapis.com/maps/api/geocode/json?components=locality:" + lat + "," + lng + "&key=" + keyString;

                WebRequest request = WebRequest.Create(urlRequest);
                request.Method = "POST";
                string postData = "This is a test that posts this string to a Web server.";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();
                dataStream = response.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);
                string resp = reader.ReadToEnd();

                reader.Close();
                dataStream.Close();
                response.Close();

                return resp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetTimeZoneID(string CountryShortName)
        {
            string TimeZoneID = "Jordan Standard Time";

            switch (CountryShortName)
            {
                case "JO":
                    TimeZoneID = "Jordan Standard Time";
                    break;

                case "SA":
                    TimeZoneID = "Arabic Standard Time";
                    break;

                case "EG":
                    TimeZoneID = "Egypt Standard Time";
                    break;

                case "TR":
                    TimeZoneID = "Turkey Standard Time";
                    break;
            };

            return TimeZoneID;
        }
    }
}