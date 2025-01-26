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
    [RoutePrefix("api/Sections")]
    public class SectionsController : ApiController
    {
        private readonly ISectionsRepository _sectionsRepository;
        public SectionsController()
        {
            _sectionsRepository = new SectionsRepository();
        }

        [Route("GetSchoolClassSections")]
        [HttpGet]
        public IHttpActionResult GetSchoolClassSections([FromUri][Required] int schoolID, [FromUri][Required] int schoolClassID)
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

        [Route("GetTeacherSections")]
        [HttpGet]
        public IHttpActionResult GetTeacherSections([FromUri][Required] string teacherId, [FromUri][Required] int schoolClassId)
        {
            try
            {
                dynamic teacherSections = _sectionsRepository.GetTeacherSections(teacherId, schoolClassId);

                if (teacherSections != null)
                    return Ok(teacherSections);

                return Content(HttpStatusCode.NotFound, Messages.NoData("Sections"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }

        [Route("PrepareTeacherAttendance")]
        [HttpGet]
        public IHttpActionResult PrepareTeacherAttendance([FromUri] int sectionId)
        {
            try
            {
                List<dynamic> sectionStudents = _sectionsRepository.GetSectionStudents(sectionId);

                if (sectionStudents.Any())
                    return Ok(sectionStudents);


                return Content(HttpStatusCode.NotFound, Messages.NoData("Students on this section"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }
    }
}
