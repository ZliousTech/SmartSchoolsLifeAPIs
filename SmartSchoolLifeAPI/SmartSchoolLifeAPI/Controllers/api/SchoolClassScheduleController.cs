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
    [RoutePrefix("api/SchoolClassSchedule")]
    public class SchoolClassScheduleController : ApiController
    {
        private readonly SchoolClassScheduleRepository _schoolClassScheduleRepository;

        public SchoolClassScheduleController()
        {
            _schoolClassScheduleRepository = new SchoolClassScheduleRepository();
        }

        [Route("GetSchoolClassSchedule")]
        [HttpGet]
        public IHttpActionResult GetSchoolClassSchedule([FromUri] int schoolID, [FromUri] int schoolClassID, 
            [FromUri] int sectionID, [FromUri] int timeTableType)
        {
            try
            {
                IEnumerable<dynamic> schoolClassSchedule = _schoolClassScheduleRepository.GetSchoolClassSchedule(schoolID, schoolClassID, sectionID, timeTableType);

                if (schoolClassSchedule.Any())
                    return Ok(schoolClassSchedule);

                return Content(HttpStatusCode.NotFound, Messages.NoData("Schedule"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }
    }
}