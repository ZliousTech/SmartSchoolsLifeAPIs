using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SmartSchoolLifeAPI.Models.HomeWork;
using SmartSchoolLifeAPI.Models.Repositories;
using SmartSchoolLifeAPI.Models.Helpers;
using System.Threading.Tasks;

namespace SmartSchoolLifeAPI.Controllers.api
{
    [RoutePrefix("api/HomeWork")]
    public class HomeWorkController : ApiController
    {
        private readonly HomeWorkRepository _homeworkRepository;
        public HomeWorkController()
        {
            _homeworkRepository = new HomeWorkRepository();
        }

        [Route("GetStudentHomeWorks")]
        [HttpGet]
        public IHttpActionResult GetStudentHomeWorks([FromUri] int sectionID)
        {
            try
            {
                IEnumerable<dynamic> homeworkList = _homeworkRepository.GetStudentHomeWork(sectionID);

                if (homeworkList.Any())
                    return Ok(homeworkList);

                return Content(HttpStatusCode.NotFound, Messages.NoData("Homeworks"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }
    }
}