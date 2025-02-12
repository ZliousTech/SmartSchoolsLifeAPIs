using System.Threading.Tasks;

namespace FireBase.Service.DataService
{
    public interface IFireBaseService
    {
        Task SendNotificationAsync(string receiverToken, string type, string notificationText, string title, DeviceType deviceType);
    }
}
