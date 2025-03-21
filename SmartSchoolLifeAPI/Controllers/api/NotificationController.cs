using SmartSchool.FireBase.Service;
using SmartSchool.FireBase.Service.DataService;
using SmartSchoolLifeAPI.Core.Models.Extensions;
using SmartSchoolLifeAPI.Core.Models.Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace SmartSchoolLifeAPI.Controllers.api
{
    public class NotificationController : ApiController
    {
        SmartSchoolsEntities2 db;
        private readonly IFireBaseService fireBaseService;

        public NotificationController()
        {
            db = new SmartSchoolsEntities2();
            fireBaseService = new FireBaseService();
        }

        [HttpPost]
        public void SaveNotif(Notification notif)
        {
            notif.DateSent = DateTime.Now;
            notif.Status = 2;
            db.Notifications.Add(notif);
            db.SaveChanges();
        }

        [HttpGet]
        public void UpdateDeviceRegCode(string DeviceRegistrationCode, string OwnerID)
        {
            db.Database.ExecuteSqlCommand("update DeviceRegistrar set DeviceRegistrationCode={0} where OwnerID={1}", DeviceRegistrationCode, OwnerID);
            db.SaveChanges();
        }

        [HttpGet]
        public void AddDeviceRegCode(string OwnerID,
            string OwnerMobileNumber, int OwnerType, string DeviceRegistrationCode = null)
        {
            var isRegisterd = db.DeviceRegistrars.FirstOrDefault(d => d.OwnerID == OwnerID);
            if (isRegisterd == null)
            {
                db.Database.ExecuteSqlCommand("INSERT INTO DeviceRegistrar " +
                    "(DeviceRegistrationCode, IsDeviceRegistrationActive, OwnerID, OwnerMobileNumber, OwnerType, RegistrationDate) " + "VALUES ({0},{1},{2},{3},{4},{5})",
                    DeviceRegistrationCode, -1, OwnerID, OwnerMobileNumber, OwnerType, DateTime.Now);
                db.SaveChanges();
            }
            else
            {
                db.Database.ExecuteSqlCommand("update DeviceRegistrar set DeviceRegistrationCode={0}, RegistrationDate={1} where OwnerID={2}", DeviceRegistrationCode, DateTime.Now, OwnerID);
                db.SaveChanges();
            }
        }

        [HttpGet]
        public DeviceRegistrar GetDevRegIdByAttendantId(string OwnerID)
        {
            return db.DeviceRegistrars.SingleOrDefault(x => x.OwnerID == OwnerID);
        }

        //[HttpGet]
        //public void SendFCM(string receiverToken, string MsgTitle, string MsgBody)
        //{
        //    var data = new
        //    {
        //        to = receiverToken,
        //        notification = new
        //        {
        //            body = MsgBody,
        //            title = MsgTitle,
        //            sound = "horn.mp3"
        //        }
        //    };

        //    using (var client = new WebClient())
        //    {
        //        //var dataString = JsonConvert.SerializeObject(data);
        //        //client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
        //        //client.Headers.Add(HttpRequestHeader.Accept, "application/json");
        //        //client.Headers.Add("Authorization", "key=AAAAhKNFcBE:APA91bGKl1Vd69fNuf1XJ7jNTjeWM6Pz9UbO5L95YXf9b03LxBBvYW_O7E-KzrjMGV8gp6qkwlfdUn3mkXu6DXMUOxWXSNUTO1ZH2m3nZSKcd1iKxFoycQPICp-tld7e6BpbluAVXlnA");
        //        //client.UploadString(new Uri("https://fcm.googleapis.com/v1/projects/busfirebaseproject-c4384/messages:send"), dataString);
        //    }
        //}

        [HttpGet]
        public async Task SendFCM(string receiverToken, string title, string message, string type = "Alert", short deviceType = 1)
        {
            await fireBaseService.SendNotificationAsync(receiverToken, type, message, title, (DeviceType)deviceType);
        }

        //[HttpGet]
        //public IEnumerable<Notification> GetNotificationsByReceiverID(string DesitinationID, int DestinationType)
        //{
        //    return db.Notifications.Where(x => x.DesitinationID == DesitinationID && x.DestinationType == DestinationType).OrderByDescending(x => x.NotificationID).ToList();
        //}

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
            db.Database.ExecuteSqlCommand("update Notifications set Status={0},DateSeen={1} where NotificationID={2}", 4, DateTime.Now, NotificationID);
            db.SaveChanges();
            return db.Notifications.SingleOrDefault(x => x.NotificationID == NotificationID);
        }
    }
}
