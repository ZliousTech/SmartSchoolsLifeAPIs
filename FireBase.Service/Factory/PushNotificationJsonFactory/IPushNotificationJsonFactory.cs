namespace FireBase.Service
{
    internal interface IPushNotificationJsonFactory
    {
        dynamic BuildPushNotificationJson(string receiverToken, string type, string notificationText, string title, string sound);
    }
}
