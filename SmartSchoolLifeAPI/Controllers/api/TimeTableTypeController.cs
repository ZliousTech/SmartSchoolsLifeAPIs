using SmartSchoolLifeAPI.Core.Models.Helpers;
using SmartSchoolLifeAPI.Core.Repos;
using System;
using System.Net;
using System.Web.Http;

namespace SmartSchoolLifeAPI.Controllers.api
{
    [RoutePrefix("api/SystemSettingsperSchool")]
    public class TimeTableTypeController : ApiController
    {
        private readonly ISystemSettingsperSchoolRepository _systemSettingsperSchoolRepository;
        public TimeTableTypeController()
        {
            _systemSettingsperSchoolRepository = new SystemSettingsperSchoolRepository();
        }

        [Route("GetTimeTableType")]
        [HttpGet]
        public IHttpActionResult GetTimeTableType([FromUri] int schoolID)
        {
            try
            {
                dynamic systemSettingsperSchool = _systemSettingsperSchoolRepository.GetTimeTableType(schoolID);

                if (systemSettingsperSchool != null)
                    return Ok(systemSettingsperSchool);

                return Content(HttpStatusCode.NotFound, Messages.NoData("Time Table Type per School"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }
    }
}