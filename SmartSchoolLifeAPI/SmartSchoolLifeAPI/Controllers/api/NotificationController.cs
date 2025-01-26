using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SmartSchoolLifeAPI.Models;
using System.Collections.Specialized;
using Newtonsoft.Json;

namespace SmartSchoolLifeAPI.Controllers.api
{
    public class NotificationController : ApiController
    {
        SmartSchoolsEntities2 db;

        public NotificationController()
        {
            db = new SmartSchoolsEntities2();
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
        public void UpdateDeviceRegCode(string DeviceRegistrationCode,string OwnerID)
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
                db.Database.ExecuteSqlCommand("update DeviceRegistrar set DeviceRegistrationCode={0}, RegistrationDate={1} where OwnerID={2}", DeviceRegistrationCode,DateTime.Now, OwnerID);
                db.SaveChanges();
            }
        }

        [HttpGet]
        public DeviceRegistrar GetDevRegIdByAttendantId(string OwnerID)
        {
            return db.DeviceRegistrars.SingleOrDefault(x => x.OwnerID == OwnerID);
        }

        [HttpGet]
        public void SendFCM(string ReceiverToken,string MsgTitle,string MsgBody)
        {
            var data = new {
                to = ReceiverToken,

                notification = new

                {
                    body = MsgBody,
                    title = MsgTitle,
                    sound = "horn.mp3"

                }
            };
            using (var client = new WebClient())
            {
                var dataString = JsonConvert.SerializeObject(data);
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                client.Headers.Add(HttpRequestHeader.Accept, "application/json");
                client.Headers.Add("Authorization", "key=AAAAhKNFcBE:APA91bGKl1Vd69fNuf1XJ7jNTjeWM6Pz9UbO5L95YXf9b03LxBBvYW_O7E-KzrjMGV8gp6qkwlfdUn3mkXu6DXMUOxWXSNUTO1ZH2m3nZSKcd1iKxFoycQPICp-tld7e6BpbluAVXlnA");
                client.UploadString(new Uri("https://fcm.googleapis.com/fcm/send"),  dataString);
                
            }

        }
        [HttpGet]
        public IEnumerable<Notification> GetNotificationsByReceiverID(string DesitinationID,int DestinationType)
        {
            return db.Notifications.Where(x => x.DesitinationID == DesitinationID && x.DestinationType == DestinationType).OrderByDescending(x=>x.NotificationID).ToList();
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
