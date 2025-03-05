namespace FireBase.Service
{
    internal class Android_PushNotificationJson : IPushNotificationJsonFactory
    {
        public dynamic BuildPushNotificationJson(string receiverToken, string type, string notificationText, string title, string sound)
        {
            var androidJson = new
            {
                message = new
                {
                    token = receiverToken,
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

            return androidJson;
        }
    }
}
