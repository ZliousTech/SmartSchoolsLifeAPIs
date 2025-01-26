using SmartSchoolLifeAPI.ViewModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;

namespace SmartSchoolLifeAPI.Controllers.api
{
    public class CountriesController : ApiController
    {
        SmartSchoolsEntities2 db;
        public CountriesController()
        {
            db = new SmartSchoolsEntities2();
        }

        [HttpGet]
        public IEnumerable<CountriesVM> GetCountriesList(string Lang)
        {
            if (Lang.Contains("en"))
            {
                Lang = "en";
            }
            else
            {
                Lang = "ar";
            }

            var paramLang = new SqlParameter("@Lang", Lang);
            var CountriesInfo = db.Database.SqlQuery<CountriesVM>("CountriesList @Lang", paramLang).ToList();
            int x = CountriesInfo.Count;

            return CountriesInfo;
        }
    }
}