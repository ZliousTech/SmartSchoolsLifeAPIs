using SmartSchoolAPI.BaseService;
using SmartSchoolAPI.DataService;
using SmartSchoolAPI.Entities;
using SmartSchoolAPI.IDataService;
using SmartSchoolLifeAPI.Core.Models.Extensions;
using SmartSchoolLifeAPI.Core.Models.Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace SmartSchoolLifeAPI.Controllers.api
{
    [RoutePrefix("api/Notification")]
    public class NotificationController : BaseController
    {
        SmartSchoolsEntities2 _db;
        private INotificationDSL _notificationDSL { get; } = new NotificationDSL();

        public NotificationController()
        {
            _db = new SmartSchoolsEntities2();
        }

        [HttpPost]
        public void SaveNotif(Notification notif)
        {
            notif.DateSent = DateTime.Now;
            notif.Status = 2;
            _db.Notifications.Add(notif);
            _db.SaveChanges();
        }

        [HttpPost]
        [Route("UpdateDeviceRegCode")]
        public async Task<IHttpActionResult> UpdateDeviceRegCode(UpdateDeviceRegistrarRequest updateDeviceRegistrarRequest)
        {
            return await ExecuteAsync(async () =>
               await _notificationDSL.UpdateDeviceRegCode(updateDeviceRegistrarRequest)
            );
        }

        [HttpPost]
        [Route("AddDeviceRegCode")]
        public async Task<IHttpActionResult> AddDeviceRegCode(AddDeviceRegistrarRequest addDeviceRegistrarRequest)
        {
            return await ExecuteAsync(async () =>
                await _notificationDSL.AddDeviceRegCode(addDeviceRegistrarRequest)
            , HttpStatusCode.Created);
        }

        [HttpGet]
        [Route("GetDevRegIdByAttendantId")]
        public async Task<IHttpActionResult> GetDevRegIdByAttendantId(string ownerId)
        {
            return await ExecuteAsync(async () =>
               await _notificationDSL.GetDevRegIdByAttendantId(ownerId)
            );
        }

        [HttpGet]
        [Route("SendFCM")]
        public async Task<IHttpActionResult> SendFCM(string receiverToken, string title, string message, string type = "Alert", string sound = "default")
        {
            return await ExecuteAsync(async () =>
               await _notificationDSL.SendFCM(receiverToken, title, message, type, sound)
            );
        }

        [HttpGet]
        public IEnumerable<dynamic> GetNotificationsByReceiverID(string DesitinationID, int DestinationType)
        {
            List<dynamic> notifications = new List<dynamic>();
            string query = "SELECT * FROM Notifications " +
                "WHERE DesitinationID = @DesitinationID AND DestinationType = @DestinationType " +
                "ORDER BY NotificationID DESC";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@DesitinationID", DesitinationID);
                    comm.Parameters.AddWithValue("@DestinationType", DestinationType);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        notifications = reader.MapAll("dd/MM/yyyy hh:mm:ss tt");
                    }
                }
                conn.Close();
                conn.Dispose();
            }
            return notifications;
        }

        [HttpGet]
        public Notification GetNotificationByID(int NotificationID)
        {
            _db.Database.ExecuteSqlCommand("update Notifications set Status={0},DateSeen={1} where NotificationID={2}", 4, DateTime.Now, NotificationID);
            _db.SaveChanges();
            return _db.Notifications.SingleOrDefault(x => x.NotificationID == NotificationID);
        }
    }
}
