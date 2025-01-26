using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SmartSchoolLifeAPI.Models;
using SmartSchoolLifeAPI.Models.Repositories;
using SmartSchoolLifeAPI.Models.Helpers;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using SmartSchoolLifeAPI.Models.Extensions;

namespace SmartSchoolLifeAPI.Controllers.api
{
    [RoutePrefix("api/SchoolSettings")]
    public class SchoolSettingsController : ApiController
    {
        private readonly SchoolSettingsRepository _schoolSettingsRepository;
        public SchoolSettingsController()
        {
            _schoolSettingsRepository = new SchoolSettingsRepository();
        }

        [Route("GetSchoolSettings")]
        [HttpGet]
        public IHttpActionResult GetSchoolSettings([FromUri] int schoolID)
        {
            try
            {
                SchoolSettings schoolSettings = _schoolSettingsRepository.GetById(schoolID);

                if (schoolSettings != null)
                    return Ok(schoolSettings);

                return Content(HttpStatusCode.NotFound, Messages.NoData("School Settings"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }
    }
}