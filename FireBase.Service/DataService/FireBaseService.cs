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

        public async Task SendNotificationAsync(string receiverToken, string type, string notificationText, string title = "")
        {
            var accessToken = await _credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

            var data = new
            {
                message = new
                {
                    token = receiverToken,
                    data = new
                    {
                        title = $"{type} Notification",
                        body = notificationText,
                        type = string.IsNullOrWhiteSpace(title) ? $"{type} Notification" : title,
                        sound = string.Empty // horn.mp3 (the Bus voice).
                    },
                    apns = new
                    {
                        payload = new
                        {
                            aps = new
                            {
                                sound = string.Empty // horn.mp3 (the Bus voice).
                            }
                        }
                    }
                }
            };

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://fcm.googleapis.com/v1/projects/busfirebaseproject-c4384/messages:send", content);
                string responseContent = await response.Content.ReadAsStringAsync();

                var requestLoggingInfo = new StringBuilder();
                requestLoggingInfo.AppendLine($"Status Code: {(int)response.StatusCode} ({response.ReasonPhrase})");
                requestLoggingInfo.AppendLine("Request Data:");
                requestLoggingInfo.AppendLine(JsonConvert.SerializeObject(data, Formatting.Indented));
                requestLoggingInfo.AppendLine("Response Headers:");
                foreach (var header in response.Headers)
                {
                    requestLoggingInfo.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
                }
                requestLoggingInfo.AppendLine("Response Body:");
                requestLoggingInfo.AppendLine(responseContent);

                // Check for 404 Not Found or 400 Bad Request (invalid token)
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    {
                        _logger.Fatal(nameof(SendNotificationAsync), requestLoggingInfo.ToString());
                    }
                    else
                    {
                        _logger.Error(nameof(SendNotificationAsync), requestLoggingInfo.ToString());
                    }

                    return; // Skip this invalid token and continue with the loop
                }

                _logger.Info(nameof(SendNotificationAsync), $"Success: {requestLoggingInfo}");

                response.EnsureSuccessStatusCode();
            }
        }
    }
}
