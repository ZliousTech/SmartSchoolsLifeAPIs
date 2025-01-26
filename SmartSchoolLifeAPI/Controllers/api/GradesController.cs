using SmartSchoolLifeAPI.Core.Models.Helpers;
using SmartSchoolLifeAPI.Core.Repos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace SmartSchoolLifeAPI.Controllers.api
{
    [RoutePrefix("api/Grades")]
    public class GradesController : ApiController
    {
        private readonly IGradesRepository _gradesRepository;
        public GradesController()
        {
            _gradesRepository = new GradesRepository();
        }

        [Route("GetStudentGrades")]
        [HttpGet]
        public IHttpActionResult GetStudentGrades([FromUri] string studentId)
        {
            try
            {
                IEnumerable<dynamic> gradesList = _gradesRepository.GetStudentGrades(studentId);

                if (gradesList.Any())
                    return Ok(gradesList);

                return Content(HttpStatusCode.NotFound, Messages.NoData("Grades"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }

        [Route("GetTeacherGrades")]
        [HttpGet]
        public IHttpActionResult GetTeacherGrades([Required] int semesterID, [Required] int sectionID,
            [Required] int subjectID, [Required] int examTypeID, [Required] int examTitleID)
        {
            try
            {
                IEnumerable<dynamic> gradesList = _gradesRepository.GetTeacherGrades(semesterID, sectionID,
                    subjectID, examTypeID, examTitleID);

                if (gradesList.Any())
                    return Ok(gradesList);

                return Content(HttpStatusCode.NotFound, Messages.NoData("Grades"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }

        [Route("AddStudentsGrades")]
        [HttpPost]
        public IHttpActionResult AddStudentsGrades([FromBody] Dictionary<int, double> studentsGrades)
        {
            try
            {
                _gradesRepository.AddStudentsGrades(studentsGrades);
                return Created("", Messages.Inserted("Students Grades are inserted 😎"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }
    }
}