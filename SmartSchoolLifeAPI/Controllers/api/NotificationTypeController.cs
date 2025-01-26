using SmartSchoolLifeAPI.Core.Models;
using SmartSchoolLifeAPI.Core.Models.Helpers;
using SmartSchoolLifeAPI.Core.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace SmartSchoolLifeAPI.Controllers.api
{
    [RoutePrefix("api/NotificationType")]
    public class NotificationTypeController : ApiController
    {
        private readonly INotificationTypeRepository _notificationTypeRepository;
        public NotificationTypeController()
        {
            _notificationTypeRepository = new NotificationTypeRepository();
        }

        [Route("GetNotificationsTypes")]
        [HttpGet]
        public IHttpActionResult GetNotificationsTypes()
        {
            try
            {
                IEnumerable<NotificationsTypesModel> notificationsTypes = _notificationTypeRepository.GetAll();

                if (notificationsTypes.Any())
                    return Ok(notificationsTypes);

                return Content(HttpStatusCode.NotFound, Messages.NoData("Notifications Types"));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, Messages.Exception(ex));
            }
        }
    }
}