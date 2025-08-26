using SmartSchool.Core.DataService;
using SmartSchool.Core.IDataService;
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
        private readonly ISmartSchoolDataServiceFactory _smartSchoolDataServiceFactory;

        public NotificationController()
        {
            _db = new SmartSchoolsEntities2();
            _fireBaseService = new FireBaseService();
            _smartSchoolDataServiceFactory = new SmartSchoolDataServiceFactory();
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
        public async Task UpdateDeviceRegCode(string deviceRegistrationCode, int deviceType, string ownerId)
        {
            var deviceRegistrarObj = (await _smartSchoolDataServiceFactory.CreateDeviceRegistrarService.Where(d => d.OwnerId == ownerId)).FirstOrDefault();
            if (deviceRegistrarObj is null)
            {
                return;
            }

            deviceRegistrarObj.DeviceRegistrationCode = deviceRegistrationCode;
            deviceRegistrarObj.LastLoggedDeviceType = deviceType;

            await _smartSchoolDataServiceFactory.CreateDeviceRegistrarService.UpdateAsync(deviceRegistrarObj);
        }

        [HttpGet]
        public async Task AddDeviceRegCode(string ownerId, string ownerMobileNumber, int ownerType, int deviceType, string deviceRegistrationCode = null)
        {
            var isTokenRegistredBefor = (await _smartSchoolDataServiceFactory.CreateDeviceRegistrarService.Where(d => d.OwnerId == ownerId)).FirstOrDefault();
            var deviceRegistrarObj = new DeviceRegistrar_DTO
            {
                DeviceRegistrationCode = deviceRegistrationCode,
                IsDeviceRegistrationActive = -1,
                OwnerId = ownerId,
                OwnerMobileNumber = ownerMobileNumber,
                OwnerType = ownerType.ToString(),
                RegistrationDate = DateTime.Now,
                LastLoggedDeviceType = deviceType
            };

            if (isTokenRegistredBefor == null)
            {
                await _smartSchoolDataServiceFactory.CreateDeviceRegistrarService.AddAsync(deviceRegistrarObj);
            }
            else
            {
                deviceRegistrarObj.Id = isTokenRegistredBefor.Id;
                await _smartSchoolDataServiceFactory.CreateDeviceRegistrarService.UpdateAsync(deviceRegistrarObj);
            }
        }

        [HttpGet]
        public async Task<DeviceRegistrar_DTO> GetDevRegIdByAttendantId(string ownerId)
        {
            return (await _smartSchoolDataServiceFactory.CreateDeviceRegistrarService.Where(d => d.OwnerId == ownerId)).FirstOrDefault();
        }

        [HttpGet]
        public async Task SendFCM(string receiverToken, string title, string message, string type = "Alert")
        {
            await _fireBaseService.SendNotificationAsync(receiverToken, type, message, title);
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
