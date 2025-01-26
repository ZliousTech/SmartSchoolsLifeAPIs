using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SmartSchoolLifeAPI.Models.Grades;
using SmartSchoolLifeAPI.Models.Repositories;
using SmartSchoolLifeAPI.Models.Helpers;
using System.Threading.Tasks;

namespace SmartSchoolLifeAPI.Controllers.api
{
    [RoutePrefix("api/Grades")]
    public class GradesController : ApiController
    {
        private readonly GradesRepository _gradesRepository;
        public GradesController()
        {
            _gradesRepository = new GradesRepository();
        }

        [Route("GetStudentGrades")]
        [HttpGet]
        public IHttpActionResult GetStudentGrades([FromUri] string studentID)
        {
            try
            {
                IEnumerable<dynamic> gradesList = _gradesRepository.GetStudentGrades(studentID);

                if (gradesList.Any())
                    return Ok(gradesList);

                return Content(HttpStatusCode.NotFound, Messages.NoData("Grades"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }
    }
}