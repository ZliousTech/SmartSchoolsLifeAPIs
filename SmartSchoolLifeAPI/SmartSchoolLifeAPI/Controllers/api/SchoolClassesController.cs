using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SmartSchoolLifeAPI.Models;
using SmartSchoolLifeAPI.Models.Repositories;
using SmartSchoolLifeAPI.Models.Helpers;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using SmartSchoolLifeAPI.Models.Extensions;

namespace SmartSchoolLifeAPI.Controllers.api
{
    [RoutePrefix("api/SchoolClasses")]
    public class SchoolClassesController : ApiController
    {
        private readonly SchoolClassesRepository _schoolClassesRepository;
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
                dynamic schoolClasses = _schoolClassesRepository.GetSchoolClasses(schoolID);

                if (schoolClasses != null)
                    return Ok(schoolClasses);

                return Content(HttpStatusCode.NotFound, Messages.NoData("School Classes"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }
    }
}