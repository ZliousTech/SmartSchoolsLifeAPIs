using SmartSchoolLifeAPI.Core.Models.Helpers;
using SmartSchoolLifeAPI.Core.Models.HomeWork;
using SmartSchoolLifeAPI.Core.Repos;
using SmartSchoolLifeAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace SmartSchoolLifeAPI.Controllers.api
{
    [RoutePrefix("api/HomeWork")]
    public class HomeWorkController : ApiController
    {
        private readonly IHomeWorkRepository _homeworkRepository;
        public HomeWorkController()
        {
            _homeworkRepository = new HomeWorkRepository();
        }

        [Route("GetHomeWork")]
        [HttpGet]
        public IHttpActionResult GetHomeWork(int homeworkId)
        {
            try
            {
                HomeWorkVM homeWorkViewModel = _homeworkRepository.GetViewById(homeworkId);

                if (homeWorkViewModel != null)
                    return Ok(homeWorkViewModel);

                return Content(HttpStatusCode.NotFound, Messages.NoData("Homework"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }

        [Route("CreateHomeWork")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateHomeWork(HomeWorkModel model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.HomeWorkAttachment) && !string.IsNullOrEmpty(model.Ext))
                {
                    HomeWorkModel addedHomework = await _homeworkRepository.AddAsync(model);
                    return Created($"HomeWork/GetHomeWork?homeworkId={addedHomework.HomeWorkID}", Messages.Inserted(addedHomework));
                }

                return Content(HttpStatusCode.BadRequest, Messages.ErrorMessage("The Homework Attachment and its Extesion are required"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }

        [Route("UpdateHomeWork")]
        [HttpPost]
        public IHttpActionResult UpdateHomeWork([FromUri][Required] int homeworkId, HomeWorkModel model)
        {
            try
            {
                if (homeworkId <= 0)
                    return Content(HttpStatusCode.BadRequest, Messages.ErrorMessage("Invalid homeworkId"));

                if (homeworkId != model.HomeWorkID)
                    return Content(HttpStatusCode.BadRequest, Messages.ErrorMessage($"homeworkId: {homeworkId} in the URI does not match HomeWorkID: {model.HomeWorkID} in the request body"));

                _homeworkRepository.Update(model);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }

        [Route("DeleteHomeWork")]
        [HttpGet]
        public IHttpActionResult DeleteHomeWork([FromUri][Required] int homeworkId)
        {
            try
            {
                if (homeworkId <= 0)
                    Content(HttpStatusCode.BadRequest, Messages.ErrorMessage("Invalid homeworkId"));

                _homeworkRepository.Delete(homeworkId);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }

        [Route("GetAttachmentByHomeworkId")]
        [HttpGet]
        public IHttpActionResult GetAttachmentByHomeworkId([FromUri] int homeworkId)
        {
            try
            {
                dynamic attachment = _homeworkRepository.GetAttachmentByHomeworkId(homeworkId);

                if (attachment != null)
                    return Ok(attachment);

                return Content(HttpStatusCode.NotFound, Messages.NoData("Attachment"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }

        [Route("GetStudentHomeWorks")]
        [HttpGet]
        public IHttpActionResult GetStudentHomeWorks([FromUri] int sectionID, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                IEnumerable<dynamic> homeworkList = _homeworkRepository.GetStudentHomeWork(sectionID, pageNumber, pageSize);

                if (homeworkList.Any())
                    return Ok(homeworkList);

                return Content(HttpStatusCode.NotFound, Messages.NoData("Homeworks"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }

        [Route("GetTeacherHomeWorks")]
        [HttpGet]
        public IHttpActionResult GetTeacherHomeWorks([Required] string teacherId, string schoolClassId, string sectionId, string subjectId,
            int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                IEnumerable<dynamic> teacherHomeworkList = _homeworkRepository.GetTeacherHomeWorks(teacherId, schoolClassId, sectionId, subjectId, pageNumber, pageSize);

                if (teacherHomeworkList.Any())
                    return Ok(teacherHomeworkList);

                return Content(HttpStatusCode.NotFound, "No Homeworks added by this teacher");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }
    }
}