using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FireBase.Service.DataService
{
    public class FireBaseService : IFireBaseService
    {
        private readonly GoogleCredential _credential;

        public FireBaseService()
        {
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

                // Check for 404 Not Found or 400 Bad Request (invalid token)
                if (!response.IsSuccessStatusCode)
                {
                    return; // Skip this invalid token and continue with the loop
                }

                response.EnsureSuccessStatusCode();
            }
        }
    }
}
