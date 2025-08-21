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
        SmartSchoolsEntities2 _db;
        private readonly IFireBaseService _fireBaseService;

        public NotificationController()
        {
            _db = new SmartSchoolsEntities2();
            _fireBaseService = new FireBaseService();
        }

        [HttpPost]
        public void SaveNotif(Notification notif)
        {
            notif.DateSent = DateTime.Now;
            notif.Status = 2;
            _db.Notifications.Add(notif);
            _db.SaveChanges();
        }

        [HttpGet]
        public void UpdateDeviceRegCode(string deviceRegistrationCode, short deviceType, string ownerId)
        {
            _db.Database.ExecuteSqlCommand("UPDATE DeviceRegistrar SET deviceRegistrationCode = {0}, LastLoggedDeviceType = {1} WHERE ownerID = {2}",
                                            deviceRegistrationCode, deviceType, ownerId);
            _db.SaveChanges();
        }

        [HttpGet]
        public void AddDeviceRegCode(string ownerID,
            string ownerMobileNumber, int ownerType, short deviceType, string deviceRegistrationCode = null)
        {
            string commandToExecute = string.Empty;
            var isRegisterd = _db.DeviceRegistrars.FirstOrDefault(d => d.OwnerID == ownerID);

            if (isRegisterd == null)
            {
                commandToExecute = $"INSERT INTO DeviceRegistrar " +
                                   $"(deviceRegistrationCode, IsDeviceRegistrationActive, ownerID, ownerMobileNumber, ownerType, RegistrationDate, LastLoggedDeviceType) " +
                                   $"VALUES ({deviceRegistrationCode}, {-1}, {ownerID}, {ownerMobileNumber}, {ownerType}, {DateTime.Now}, {deviceType})";

            }
            else
            {
                commandToExecute = $"UPDATE DeviceRegistrar SET " +
                                   $"DeviceRegistrationCode = {deviceRegistrationCode}, RegistrationDate = {DateTime.Now}, LastLoggedDeviceType = {deviceType} " +
                                   $"WHERE OwnerID = {ownerID}";
            }

            _db.Database.ExecuteSqlCommand(commandToExecute);
            _db.SaveChanges();
        }

        [HttpGet]
        public DeviceRegistrar GetDevRegIdByAttendantId(string OwnerID)
        {
            return _db.DeviceRegistrars.SingleOrDefault(x => x.OwnerID == OwnerID);
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
        public async Task SendFCM(string receiverToken, string title, string message, string type = "Alert")
        {
            await _fireBaseService.SendNotificationAsync(receiverToken, type, message, title);
        }

        //[HttpGet]
        //public IEnumerable<Notification> GetNotificationsByReceiverID(string DesitinationID, int DestinationType)
        //{
        //    return _db.Notifications.Where(x => x.DesitinationID == DesitinationID && x.DestinationType == DestinationType).OrderByDescending(x => x.NotificationID).ToList();
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
            _db.Database.ExecuteSqlCommand("update Notifications set Status={0},DateSeen={1} where NotificationID={2}", 4, DateTime.Now, NotificationID);
            _db.SaveChanges();
            return _db.Notifications.SingleOrDefault(x => x.NotificationID == NotificationID);
        }
    }
}
