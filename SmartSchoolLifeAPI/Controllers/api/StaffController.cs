using SmartSchoolLifeAPI.Core.Models.Helpers;
using SmartSchoolLifeAPI.Core.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace SmartSchoolLifeAPI.Controllers.api
{
    [RoutePrefix("api/Staff")]
    public class StaffController : ApiController
    {
        private readonly IStaffRepository _staffRepository;

        public StaffController()
        {
            _staffRepository = new StaffRepository();
        }

        [Route("GetStaffData")]
        [HttpGet]
        public IHttpActionResult GetStaffData()
        {
            try
            {
                IEnumerable<dynamic> staff = _staffRepository.GetStaffData();

                if (staff.Any())
                    return Ok(staff);

                return Content(HttpStatusCode.NotFound, Messages.NoData("staff"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }
    }
}
