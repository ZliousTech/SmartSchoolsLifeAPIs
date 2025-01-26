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
    [RoutePrefix("api/Semester")]
    public class SemestersController : ApiController
    {
        private readonly ISemestersRepository _semestersRepository;

        public SemestersController()
        {
            _semestersRepository = new SemesterRepository();
        }

        [Route("GetSchoolSemesters")]
        [HttpGet]
        public IHttpActionResult GetSchoolSemesters([FromUri][Required] int schoolId, [FromUri] string schoolYear)
        {
            try
            {
                IEnumerable<dynamic> semestersList = _semestersRepository.GetSchoolSemesters(schoolId, schoolYear);

                if (semestersList.Any())
                    return Ok(semestersList);

                return Content(HttpStatusCode.NotFound, Messages.NoData("Semesters"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }
    }
}
