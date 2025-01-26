using SmartSchoolLifeAPI.Core.Models.Helpers;
using SmartSchoolLifeAPI.Core.Repos;
using System;
using System.Net;
using System.Web.Http;

namespace SmartSchoolLifeAPI.Controllers.api
{
    [RoutePrefix("api/AcademicCalendar")]
    public class AcademicCalendarController : ApiController
    {
        private readonly IAcademicCalendarRepository _academicCalendarRepository;
        public AcademicCalendarController()
        {
            _academicCalendarRepository = new AcademicCalendarRepository();
        }

        [Route("GetStudentCalendar")]
        [HttpGet]
        public IHttpActionResult GetStudentCalendar(string studentId, int schoolId)
        {
            try
            {
                dynamic studentCalendar = _academicCalendarRepository.GetStudentCalendar(studentId, schoolId);

                if (studentCalendar != null)
                    return Ok(studentCalendar);

                return Content(HttpStatusCode.NotFound, Messages.NoData($"Calendar for the provided studentId: {studentId}"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }
    }
}
