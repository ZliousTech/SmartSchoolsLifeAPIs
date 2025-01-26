using SmartSchoolLifeAPI.Core.Models.Helpers;
using SmartSchoolLifeAPI.Core.Models.Student.StudentDiseases;
using SmartSchoolLifeAPI.Core.Repos;
using System;
using System.Net;
using System.Web.Http;

namespace SmartSchoolLifeAPI.Controllers.api
{
    [RoutePrefix("api/Student")]
    public class StudentController : ApiController
    {
        private readonly IStudentDiseasesRepository _studentDiseasesRepository;
        private readonly IStudentRepository _studentRepository;
        public StudentController()
        {
            _studentDiseasesRepository = new StudentDiseasesRepository();
            _studentRepository = new StudentRepository();
        }

        [Route("GetStudentsData")]
        [HttpGet]
        public IHttpActionResult GetStudentsData()
        {
            try
            {
                dynamic students = _studentRepository.GetStudentsData();

                if (students != null)
                    return Ok(students);

                return Content(HttpStatusCode.NotFound, Messages.NoData("Students"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }

        [Route("GetStudentMedicalReport")]
        [HttpGet]
        public IHttpActionResult GetStudentDiseases([FromUri] string studentID)
        {
            try
            {
                dynamic studentDiseases = _studentDiseasesRepository.GetStudentDiseases(studentID);

                if (studentDiseases != null)
                    return Ok(studentDiseases);

                return Content(HttpStatusCode.NotFound, Messages.NoData("Student Diseases"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }

        // HTTP PUT and DELETE don't work on the server side.
        [Route("UpdateStudentMedicalReport")]
        [HttpGet]
        public IHttpActionResult UpdateStudentDiseases(string studentID, bool Mumps, bool Chickenpox, bool rubella, bool Rheumaticfever,
        bool Diabetes, bool Heartdiseases, bool Pissingoff, bool Jointbonediseases,
        bool sprayer, bool Hearingimpairment, bool Visualimpairment, bool Speechimpairment,
        bool Bladderdiseases, bool Epilepsy, bool Hepatitis, bool Shakika, bool Fainting,
        bool Kidneydisease, bool Surgery, bool Urinarysystemdiseases)
        {
            try
            {

                _studentDiseasesRepository.Update(new StudentDiseasesModel()
                {
                    InternalStudentID = studentID,
                    Mumps = Mumps,
                    Chickenpox = Chickenpox,
                    rubella = rubella,
                    Rheumaticfever = Rheumaticfever,
                    Diabetes = Diabetes,
                    Heartdiseases = Heartdiseases,
                    Pissingoff = Pissingoff,
                    Jointbonediseases = Jointbonediseases,
                    sprayer = sprayer,
                    Hearingimpairment = Hearingimpairment,
                    Visualimpairment = Visualimpairment,
                    Speechimpairment = Speechimpairment,
                    Bladderdiseases = Bladderdiseases,
                    Epilepsy = Epilepsy,
                    Hepatitis = Hepatitis,
                    Shakika = Shakika,
                    Fainting = Fainting,
                    Kidneydisease = Kidneydisease,
                    Surgery = Surgery,
                    Urinarysystemdiseases = Urinarysystemdiseases
                });

                return Content(HttpStatusCode.OK, Messages.Updated());
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }
    }
}