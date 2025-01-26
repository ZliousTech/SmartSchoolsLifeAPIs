using SmartSchoolLifeAPI.Core.Models.ExamTypes;
using SmartSchoolLifeAPI.Core.Models.Helpers;
using SmartSchoolLifeAPI.Core.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace SmartSchoolLifeAPI.Controllers.api
{
    [RoutePrefix("api/ExamType")]
    public class ExamTypeController : ApiController
    {
        private readonly IExamTypeRepository _examTypeRepository;

        public ExamTypeController()
        {
            _examTypeRepository = new ExamTypeRepository();
        }

        [Route("GetExamTypes")]
        [HttpGet]
        public IHttpActionResult GetExamTypes()
        {
            try
            {
                IEnumerable<ExamType> examTypes = _examTypeRepository.GetAll();

                if (examTypes.Any())
                    return Ok(examTypes);

                return Content(HttpStatusCode.NotFound, Messages.NoData("Exam Types"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }
    }
}
