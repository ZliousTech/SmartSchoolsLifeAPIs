using System;

namespace FireBase.Service
{
    internal class PushNotificationJsonFactory
    {
        public static dynamic CreatePushNotificationJson(IPushNotificationJsonFactory pushNotificationJsonType, string receiverToken, string type,
            string notificationText, string title, string sound)
        {
            return pushNotificationJsonType.BuildPushNotificationJson(receiverToken, type, notificationText, title, sound);
        }

        public static IPushNotificationJsonFactory GetPushNotificationJsonType(DeviceType deviceType)
        {
            switch (deviceType)
            {
                case DeviceType.Android:
                    return new Android_PushNotificationJson();
                case DeviceType.IOS:
                    return new IOS_PushNotificationJson();
                default:
                    throw new Exception("UnSupported Device Type!");
            }
        }
    }
}
