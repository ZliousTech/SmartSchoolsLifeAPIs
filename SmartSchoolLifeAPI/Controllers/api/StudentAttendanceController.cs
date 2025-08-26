using SmartSchoolLifeAPI.Core.DTOs;
using SmartSchoolLifeAPI.Core.Models.Helpers;
using SmartSchoolLifeAPI.Core.Repos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace SmartSchoolLifeAPI.Controllers.api
{
    [RoutePrefix("api/StudentAttendance")]
    public class StudentAttendanceController : ApiController
    {
        private readonly IAttendanceRepository _attendanceRepository;
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

        [Route("GetAbsenceReasons")]
        [HttpGet]
        public IHttpActionResult GetAbsenceReasons()
        {
            try
            {
                dynamic absenceReasons = _attendanceRepository.GetAbsenceReasons();

                if (absenceReasons != null)
                    return Ok(absenceReasons);


                return Content(HttpStatusCode.NotFound, Messages.NoData("Absence Reasons"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }

        [Route("AddAttendanceByTeacher")]
        [HttpPost]
        public async Task<IHttpActionResult> AddAttendanceByTeacher([FromBody][Required] List<AttendanceInsertDTO> model,
            [Required] int schoolId, [Required] string schoolYear, [Required] int sessionNumber,
            [Required] string date, [Required] string teacherId)
        {
            try
            {
                if (model.Any())
                {
                    await _attendanceRepository.AddAttendanceByTeacher(model, schoolId, schoolYear, sessionNumber, date, teacherId);
                    return Content(HttpStatusCode.Created, Messages.Inserted("Students Attendance"));
                }

                return Content(HttpStatusCode.NotFound, Messages.NoData("Attendance"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }

        [Route("AddAttendanceByParent")]
        [HttpPost]
        public IHttpActionResult AddAttendanceByParent([FromBody][Required] StudentAttendanceByParentDTO absenceModel)
        {
            try
            {
                if (!absenceModel.Sessions.Any())
                    return Content(HttpStatusCode.BadRequest, Messages.ErrorMessage("Sessions list is null or empty."));

                _attendanceRepository.AddAttendanceByParent(absenceModel);
                return Content(HttpStatusCode.Created, Messages.Inserted("Student Attendance"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }

        [Route("GetStudentAbsenceOnBusStatus")]
        [HttpGet]
        public IHttpActionResult GetStudentAbsenceOnBusStatus([FromUri][Required] int schoolId, [FromUri][Required] string studentId,
            [FromUri][Required] string busNumber, [FromUri][Required] string tripDate, [FromUri][Required] string tripTime,
            [FromUri][Required] int direction)
        {
            try
            {
                var status = _attendanceRepository.GetStudentAbsenceOnBusStatus(schoolId, studentId, busNumber, tripDate, tripTime, direction);
                return Content(HttpStatusCode.OK, status);
            }
            catch (ObjectNotFoundException ex)
            {
                return Content(HttpStatusCode.NotFound, Messages.Exception(ex));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }

        [Route("SwitchStudentAttendanceOnBus")]
        [HttpPost]
        public IHttpActionResult SwitchStudentAttendanceOnBus([FromUri][Required] int schoolId, [FromUri][Required] string studentId,
            [FromUri][Required] string busNumber, [FromUri][Required] string tripDate, [FromUri][Required] string tripTime,
            [FromUri][Required] int direction)
        {
            try
            {
                _attendanceRepository.SwitchStudentAttendanceOnBus(schoolId, studentId, busNumber, tripDate, tripTime, direction);
                return Content(HttpStatusCode.Created, Messages.Updated());
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }
    }
}