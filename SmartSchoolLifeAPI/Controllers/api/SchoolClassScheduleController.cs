using SmartSchoolLifeAPI.Core.Models;
using SmartSchoolLifeAPI.Core.Models.Helpers;
using SmartSchoolLifeAPI.Core.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace SmartSchoolLifeAPI.Controllers.api
{
    [RoutePrefix("api/SchoolClassSchedule")]
    public class SchoolClassScheduleController : ApiController
    {
        private readonly ISchoolClassScheduleRepository _schoolClassScheduleRepository;

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
                Dictionary<string, List<SchoolClassScheduleModel>> schoolClassSchedule = _schoolClassScheduleRepository.GetSchoolClassSchedule(schoolID, schoolClassID, sectionID, timeTableType);

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