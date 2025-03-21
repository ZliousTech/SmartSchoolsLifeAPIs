using SmartSchool.FireBase.Service;
using SmartSchoolLifeAPI.Core.Models;
using SmartSchoolLifeAPI.Core.Models.Helpers;
using SmartSchoolLifeAPI.Core.Repos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace SmartSchoolLifeAPI.Controllers.api
{
    [RoutePrefix("api/TeacherExam")]
    public class TeacherExamController : ApiController
    {
        private readonly ITeacherExamRepository _teacherExamRepository;

        public TeacherExamController()
        {
            _teacherExamRepository = new TeacherExamRepository();
        }

        [Route("GetExam")]
        [HttpGet]
        public IHttpActionResult GetExam(int examId)
        {
            try
            {
                Exam exam = _teacherExamRepository.GetById(examId);

                if (exam != null)
                    return Ok(exam);

                return Content(HttpStatusCode.NotFound, Messages.NoData("Exam"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }

        [Route("GetExamsForTeacher")]
        [HttpGet]
        public IHttpActionResult GetExamsForTeacher([Required] string teacherId, [Required] int schoolID,
            string semesterID, string schoolClassId, string sectionId, string subjectId, string examTypeId, string schoolYear,
            int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                IEnumerable<dynamic> teacherExams = _teacherExamRepository.GetExamsForTeacher(teacherId, schoolID,
                    semesterID, schoolClassId, sectionId, subjectId, examTypeId, schoolYear, pageNumber, pageSize);

                if (teacherExams.Any())
                    return Ok(teacherExams);

                return Content(HttpStatusCode.NotFound, Messages.NoData("Exams for Teacher"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }

        [Route("CreateExam")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateExam(Exam model, short deviceType = 1)
        {
            try
            {
                Exam addedexam = await _teacherExamRepository.AddAsync(model, (DeviceType)deviceType);
                return Created($"TeacherExam/GetExam?examId={addedexam.ID}", Messages.Inserted(addedexam));

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }

        [Route("AddOrUpdateExamsDates")]
        [HttpPost]
        public IHttpActionResult AddOrUpdateExamsDates([FromBody] Dictionary<int, string> examsDates)
        {
            try
            {
                _teacherExamRepository.AddOrUpdateExamsDates(examsDates);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }

        [Route("UpdateExam")]
        [HttpPost]
        public IHttpActionResult UpdateExam([FromUri][Required] int examId, Exam model)
        {
            try
            {
                if (examId <= 0)
                    return Content(HttpStatusCode.BadRequest, Messages.ErrorMessage("Invalid examId"));

                if (examId != model.ID)
                    return Content(HttpStatusCode.BadRequest, Messages.ErrorMessage($"examId: {examId} in the URI does not match ID: {model.ID} in the request body"));

                _teacherExamRepository.Update(model);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }

        [Route("DeleteExam")]
        [HttpGet]
        public IHttpActionResult DeleteExam([FromUri][Required] int examId)
        {
            try
            {
                if (examId <= 0)
                    Content(HttpStatusCode.BadRequest, Messages.ErrorMessage("Invalid examId"));

                _teacherExamRepository.Delete(examId);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }
    }
}
