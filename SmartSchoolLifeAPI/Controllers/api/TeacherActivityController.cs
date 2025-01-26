using SmartSchoolLifeAPI.Core.Models.Activity;
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
    [RoutePrefix("api/TeacherActivity")]
    public class TeacherActivityController : ApiController
    {
        private readonly ITeacherActivityRepository _teacherActivityRepository;

        public TeacherActivityController()
        {
            _teacherActivityRepository = new TeacherActivityRepository();
        }

        [Route("PrepareTeacherActivity")]
        [HttpGet]
        public IHttpActionResult PrepareTeacherActivity([FromUri] string teacherId, [FromUri] string schoolClassId, [FromUri] string sectionId,
            int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                List<dynamic> teacherActivities = _teacherActivityRepository.PrepareTeacherActivity(teacherId, schoolClassId, sectionId, pageNumber, pageSize);

                if (teacherActivities.Any())
                    return Ok(teacherActivities);


                return Content(HttpStatusCode.NotFound, Messages.NoData("Teacher Activities"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }

        [Route("GetTeacherActivity")]
        [HttpGet]
        public IHttpActionResult GetTeacherActivity(int activityId)
        {
            try
            {
                TeacherActivity teacherActivity = _teacherActivityRepository.GetById(activityId);

                if (teacherActivity != null)
                    return Ok(teacherActivity);

                return Content(HttpStatusCode.NotFound, Messages.NoData("Teacher Activity"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }

        [Route("CreateTeacherActivity")]
        [HttpPost]
        public IHttpActionResult CreateTeacherActivity([FromBody] TeacherActivity model)
        {
            try
            {
                TeacherActivity addedactivity = _teacherActivityRepository.Add(model);
                return Created($"TeacherActivity/GetTeacherActivity?activityId={addedactivity.ID}", Messages.Inserted(addedactivity));

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }

        [Route("UpdateTeacherActivity")]
        [HttpPost]
        public IHttpActionResult UpdateTeacherActivity([FromUri][Required] int activityId, TeacherActivity model)
        {
            try
            {
                if (activityId <= 0)
                    return Content(HttpStatusCode.BadRequest, Messages.ErrorMessage("Invalid activityId"));

                if (activityId != model.ID)
                    return Content(HttpStatusCode.BadRequest, Messages.ErrorMessage($"activityId: {activityId} in the URI does not match HomeWorkID: {model.ID} in the request body"));

                _teacherActivityRepository.Update(model);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }

        [Route("DeleteTeacherActivity")]
        [HttpGet]
        public IHttpActionResult DeleteTeacherActivity([FromUri][Required] int activityId)
        {
            try
            {
                if (activityId <= 0)
                    Content(HttpStatusCode.BadRequest, Messages.ErrorMessage("Invalid activityId"));

                _teacherActivityRepository.Delete(activityId);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }
    }
}
