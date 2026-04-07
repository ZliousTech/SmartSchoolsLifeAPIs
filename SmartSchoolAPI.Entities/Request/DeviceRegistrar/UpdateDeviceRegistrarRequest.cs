namespace SmartSchoolAPI.Entities
{
    public class UpdateDeviceRegistrarRequest
    {
        public string OwnerId { get; set; }
        public int DeviceType { get; set; }
        public string DeviceRegistrationCode { get; set; } = null;
    }
}
