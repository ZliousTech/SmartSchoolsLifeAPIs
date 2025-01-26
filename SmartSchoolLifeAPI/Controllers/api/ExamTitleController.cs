using SmartSchoolLifeAPI.Core.Models.ExamTitles;
using SmartSchoolLifeAPI.Core.Models.Helpers;
using SmartSchoolLifeAPI.Core.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace SmartSchoolLifeAPI.Controllers.api
{
    [RoutePrefix("api/ExamTitle")]
    public class ExamTitleController : ApiController
    {
        private readonly IExamTitleRepository _examTitleRepository;

        public ExamTitleController()
        {
            _examTitleRepository = new ExamTitleRepository();
        }

        [Route("GetExamTitle")]
        [HttpGet]
        public IHttpActionResult GetExamTitle()
        {
            try
            {
                IEnumerable<ExamTitle> examTitles = _examTitleRepository.GetAll();

                if (examTitles.Any())
                    return Ok(examTitles);

                return Content(HttpStatusCode.NotFound, Messages.NoData("Exam Titles"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }
    }
}
