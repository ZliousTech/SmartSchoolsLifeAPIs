using SmartSchoolLifeAPI.Core.Models;
using SmartSchoolLifeAPI.Core.Models.Helpers;
using SmartSchoolLifeAPI.Core.Repos;
using System;
using System.Net;
using System.Web.Http;

namespace SmartSchoolLifeAPI.Controllers.api
{
    [RoutePrefix("api/SchoolSettings")]
    public class SchoolSettingsController : ApiController
    {
        private readonly ISchoolSettingsRepository _schoolSettingsRepository;
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