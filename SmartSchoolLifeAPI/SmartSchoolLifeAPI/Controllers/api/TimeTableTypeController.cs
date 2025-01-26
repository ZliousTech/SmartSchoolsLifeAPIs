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
    [RoutePrefix("api/SystemSettingsperSchool")]
    public class TimeTableTypeController : ApiController
    {
        private readonly SystemSettingsperSchoolRepository _systemSettingsperSchoolRepository;
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

                return Content(HttpStatusCode.NotFound, Messages.NoData("System Settings per School"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }
    }
}