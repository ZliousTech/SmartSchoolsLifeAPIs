using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SmartSchoolLifeAPI.Models.Attendance;
using SmartSchoolLifeAPI.Models.Repositories;
using SmartSchoolLifeAPI.Models.Helpers;
using System.Threading.Tasks;
using System.Net.Http.Formatting;

namespace SmartSchoolLifeAPI.Controllers.api
{
    [RoutePrefix("api/StudentAttendance")]
    public class StudentAttendanceController : ApiController
    {
        private readonly AttendanceRepository _attendanceRepository;
        public StudentAttendanceController()
        {
            _attendanceRepository = new AttendanceRepository();
        }

        [Route("GetStudentAttendance")]
        [HttpGet]
        public IHttpActionResult GetStudentAttendance([FromUri] string studentID, [FromUri] string date)
        {
            try
            {
                dynamic studentAttendance = _attendanceRepository.GetStudentAttendance(studentID, date);

                if (studentAttendance != null)
                    return Ok(studentAttendance);
                    

                return Content(HttpStatusCode.NotFound, Messages.NoData("Attendance"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }
    }
}