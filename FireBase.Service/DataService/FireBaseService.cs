using Google.Apis.Auth.OAuth2;
using Logger.Service;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FireBase.Service.DataService
{
    public class FireBaseService : IFireBaseService
    {
        private readonly ILogger _logger;
        private readonly GoogleCredential _credential;

        public FireBaseService()
        {
            _logger = new Logger.Service.Logger();
            _credential = GoogleCredential.FromJson(FireBase_Helper.Instance.GetPrivateKey())
                .CreateScoped("https://www.googleapis.com/auth/firebase.messaging");
        }

        public async Task SendNotificationAsync(string receiverToken, string type, string notificationText, string title, DeviceType deviceType)
        {
            var accessToken = await _credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

            title = $"{type} Notification";
            type = string.IsNullOrWhiteSpace(title) ? $"{type} Notification" : title;

            IPushNotificationJsonFactory pushNotificationJsonType = PushNotificationJsonFactory.GetPushNotificationJsonType(deviceType);
            var data = PushNotificationJsonFactory.CreatePushNotificationJson(pushNotificationJsonType, receiverToken, type, notificationText, title, string.Empty);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://fcm.googleapis.com/v1/projects/busfirebaseproject-c4384/messages:send", content);
                string responseContent = await response.Content.ReadAsStringAsync();

                var requestLoggerInfo = new StringBuilder();
                requestLoggerInfo.AppendLine($"Status Code: {(int)response.StatusCode} ({response.ReasonPhrase})");
                requestLoggerInfo.AppendLine("Request Data:");
                requestLoggerInfo.AppendLine(JsonConvert.SerializeObject(data, Formatting.Indented));
                requestLoggerInfo.AppendLine("Response Headers:");
                foreach (var header in response.Headers)
                {
                    requestLoggerInfo.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
                }
                requestLoggerInfo.AppendLine("Response Body:");
                requestLoggerInfo.AppendLine(responseContent);

                // Check for 404 Not Found or 400 Bad Request (invalid token)
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    {
                        _logger.Fatal(nameof(SendNotificationAsync), requestLoggerInfo.ToString());
                    }
                    else
                    {
                        _logger.Error(nameof(SendNotificationAsync), requestLoggerInfo.ToString());
                    }

                    return; // Skip this invalid token and continue with other tokens.
                }

                _logger.Info($"{this.GetType().Name}_{nameof(SendNotificationAsync)}", $"Success: {requestLoggerInfo}");

                response.EnsureSuccessStatusCode();
            }
        }
    }
}
