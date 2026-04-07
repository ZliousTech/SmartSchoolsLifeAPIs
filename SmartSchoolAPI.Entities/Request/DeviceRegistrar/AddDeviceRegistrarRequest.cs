namespace SmartSchoolAPI.Entities
{
    public class AddDeviceRegistrarRequest
    {
        public string OwnerId { get; set; }
        public string OwnerMobileNumber { get; set; }
        public int OwnerType { get; set; }
        public int DeviceType { get; set; }
        public string DeviceRegistrationCode { get; set; } = null;
    }
}
