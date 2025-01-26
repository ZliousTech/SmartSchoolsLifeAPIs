using SmartSchoolLifeAPI.Core.Models;
using SmartSchoolLifeAPI.Core.Models.Helpers;
using SmartSchoolLifeAPI.Core.Repos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Web.Http;

namespace SmartSchoolLifeAPI.Controllers.api
{
    [RoutePrefix("api/TeacherClassSchedule")]
    public class TeacherClassScheduleController : ApiController
    {
        private readonly ITeacherClassScheduleRepository _teacherClassScheduleRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        public TeacherClassScheduleController()
        {
            _teacherClassScheduleRepository = new TeacherClassScheduleRepository();
            _attendanceRepository = new AttendanceRepository();
        }

        [Route("GetTeacherClassSchedule")]
        [HttpGet]
        public IHttpActionResult GetTeacherClassSchedule([FromUri] int schoolID, [FromUri] string staffID, [FromUri] int timeTableType)
        {
            try
            {
                Dictionary<string, List<TeacherClassScheduleModel>> teacherClassSchedule = _teacherClassScheduleRepository.GetTeacherClassSchedule(schoolID, staffID, timeTableType);

                if (teacherClassSchedule != null)
                    return Ok(teacherClassSchedule);

                return Content(HttpStatusCode.NotFound, Messages.NoData("Schedule for this teacher"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }

        [Route("GetTeacherScheduleDays")]
        [HttpGet]
        public IHttpActionResult GetTeacherScheduleDays([Required] int schoolId, [Required] string schoolYear,
            [Required] string teacherId, [Required] int sectionID, [Required] int timeTableType)
        {
            try
            {
                dynamic teacherScheduleDays =
                    _attendanceRepository.GetTeacherScheduleDays(schoolId, schoolYear, teacherId, sectionID, timeTableType);

                if (teacherScheduleDays != null)
                    return Ok(teacherScheduleDays);

                return Content(HttpStatusCode.NotFound, Messages.NoData("Schedule Days for this teacher"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }

        [Route("GetTeacherScheduleSessionOrders")]
        [HttpGet]
        public IHttpActionResult GetTeacherScheduleSessionOrders([Required] int schoolId, [Required] string schoolYear,
            [Required] string teacherId, [Required] int sectionID, [Required] int weekDay, [Required] int timeTableType)
        {
            try
            {
                dynamic teacherScheduleSessionOrders =
                    _attendanceRepository.GetTeacherScheduleSessionOrders(schoolId, schoolYear, teacherId, sectionID, weekDay, timeTableType);

                if (teacherScheduleSessionOrders != null)
                    return Ok(teacherScheduleSessionOrders);

                return Content(HttpStatusCode.NotFound, Messages.NoData("Schedule Sessions for this teacher"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }
    }
}