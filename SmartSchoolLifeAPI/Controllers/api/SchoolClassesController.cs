using SmartSchoolAPI.BaseService;
using SmartSchoolAPI.DataService;
using SmartSchoolAPI.IDataService;
using SmartSchoolLifeAPI.Core.Models.Helpers;
using SmartSchoolLifeAPI.Core.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace SmartSchoolLifeAPI.Controllers.api
{
    [RoutePrefix("api/SchoolClasses")]
    public class SchoolClassesController : BaseController
    {
        private readonly ISchoolClassesRepository _schoolClassesRepository;
        private ISchoolClassDSL _schoolClassDSL { get; } = new SchoolClassDSL();

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
                {
                    return Ok(schoolClasses);
                }

                return Content(HttpStatusCode.NotFound, Messages.NoData("School Classes"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }

        [Route("GetTeacherSchoolClasses")]
        [HttpGet]
        public async Task<IHttpActionResult> GetTeacherSchoolClasses([FromUri] string teacherId)
        {
            return await ExecuteAsync(
                () => _schoolClassDSL.GetTeacherSchoolClasses(teacherId)
            );
        }
    }
}