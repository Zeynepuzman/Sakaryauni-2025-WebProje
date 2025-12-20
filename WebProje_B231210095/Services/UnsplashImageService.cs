using System.Text.Json;

namespace WebProje_B231210095.Services
{
    public class UnsplashImageService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public UnsplashImageService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string?> GetFitnessImageAsync(string cinsiyet, string hedef)
        {
            var accessKey = _configuration["Unsplash:AccessKey"];
            if (string.IsNullOrEmpty(accessKey))
                return null;

            string query = BuildQuery(cinsiyet, hedef);

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"https://api.unsplash.com/search/photos?query={query}&per_page=1&orientation=portrait"
            );

            request.Headers.Add("Authorization", $"Client-ID {accessKey}");

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            return doc.RootElement
                .GetProperty("results")[0]
                .GetProperty("urls")
                .GetProperty("regular")
                .GetString();
        }

        private string BuildQuery(string cinsiyet, string hedef)
        {
            string gender = cinsiyet == "Kadın" ? "woman" : "man";

            string goal = hedef switch
            {
                "Kilo Vermek" => "fitness weight loss body",
                "Kas Geliştirmek" => "muscular fitness body",
                _ => "healthy fitness body"
            };

            return $"{gender} {goal}";
        }
    }
}
