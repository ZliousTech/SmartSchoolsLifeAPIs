namespace FireBase.Service
{
    internal class IOS_PushNotificationJson : IPushNotificationJsonFactory
    {
        public dynamic BuildPushNotificationJson(string receiverToken, string type, string notificationText, string title, string sound)
        {
            var iosJson = new
            {
                message = new
                {
                    token = receiverToken,
                    notification = new
                    {
                        title = title,
                        body = notificationText,
                    },
                    data = new
                    {
                        title = title,
                        body = notificationText,
                        type = type,
                        sound = sound // horn.mp3 (the Bus voice).
                    },
                    apns = new
                    {
                        payload = new
                        {
                            aps = new
                            {
                                sound = sound // horn.mp3 (the Bus voice).
                            }
                        }
                    }
                }
            };

            return iosJson;
        }
    }
}
