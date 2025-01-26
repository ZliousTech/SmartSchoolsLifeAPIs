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
    [RoutePrefix("api/Sections")]
    public class SectionsController : ApiController
    {
        private readonly SectionsRepository _sectionsRepository;
        public SectionsController()
        {
            _sectionsRepository = new SectionsRepository();
        }

        [Route("GetSchoolClassSections")]
        [HttpGet]
        public IHttpActionResult GetSchoolClassSections([FromUri] int schoolID, [FromUri] int schoolClassID)
        {
            try
            {
                dynamic sections = _sectionsRepository.GetSchoolClassSections(schoolID, schoolClassID);

                if (sections != null)
                    return Ok(sections);

                return Content(HttpStatusCode.NotFound, Messages.NoData("Sections"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }
    }
}
