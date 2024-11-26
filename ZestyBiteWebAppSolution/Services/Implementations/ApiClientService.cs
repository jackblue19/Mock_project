using System.Net.Http;
using System.Threading.Tasks;

namespace ZestyBiteWebAppSolution.Services.Implementations
{
    public class ApiClientService
    {
        private readonly HttpClient _httpClient;

        public ApiClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetProfileDataAsync(string token)
        {
            // Tạo HttpRequestMessage với header Authorization chứa Bearer token
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://example.com/api/profile"),
                Headers =
                {
                    { "Authorization", "Bearer " + token }
                }
            };

            // Gửi yêu cầu tới API
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return data;
            }
            else
            {
                return $"Failed to fetch profile: {response.StatusCode}";
            }
        }
    }
}
