using SmartSchoolLifeAPI.Core.Models.Helpers;
using SmartSchoolLifeAPI.Core.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace SmartSchoolLifeAPI.Controllers.api
{
    [RoutePrefix("api/SchoolClasses")]
    public class SchoolClassesController : ApiController
    {
        private readonly ISchoolClassesRepository _schoolClassesRepository;
        public SchoolClassesController()
        {
            _schoolClassesRepository = new SchoolClassesRepository();
        }

        [Route("GetSchoolClasses")]
        [HttpGet]
        public IHttpActionResult GetSchoolClasses([FromUri] int schoolID)
        {
            try
            {
                IEnumerable<dynamic> schoolClasses = _schoolClassesRepository.GetSchoolClasses(schoolID);

                if (schoolClasses.Any())
                    return Ok(schoolClasses);

                return Content(HttpStatusCode.NotFound, Messages.NoData("School Classes"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }

        [Route("GetTeacherSchoolClasses")]
        [HttpGet]
        public IHttpActionResult GetTeacherSchoolClasses([FromUri] string teacherId, [FromUri] string schoolYear)
        {
            try
            {
                IEnumerable<dynamic> teacherSchoolClasses = _schoolClassesRepository.GetTeacherSchoolClasses(teacherId, schoolYear);

                if (teacherSchoolClasses.Any())
                    return Ok(teacherSchoolClasses);

                return Content(HttpStatusCode.NotFound, Messages.NoData("School Classes"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }
    }
}