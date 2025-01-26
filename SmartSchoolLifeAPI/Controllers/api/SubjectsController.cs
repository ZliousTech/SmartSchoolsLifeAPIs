using SmartSchoolLifeAPI.Core.Models.Helpers;
using SmartSchoolLifeAPI.Core.Repos;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Web.Http;

namespace SmartSchoolLifeAPI.Controllers.api
{
    [RoutePrefix("api/Subjects")]
    public class SubjectsController : ApiController
    {
        private readonly ISubjectReopsitory _subjectsRepository;
        public SubjectsController()
        {
            _subjectsRepository = new SubjectsRepository();
        }

        [Route("GetTeacherSubjects")]
        [HttpGet]
        public IHttpActionResult GetTeacherSubjects([FromUri][Required] string teacherId, [FromUri][Required] int schoolClassId)
        {
            try
            {
                dynamic teacherSubjects = _subjectsRepository.GetTeacherSubjects(teacherId, schoolClassId);

                if (teacherSubjects != null)
                    return Ok(teacherSubjects);

                return Content(HttpStatusCode.NotFound, Messages.NoData("Subjects"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }
    }
}
