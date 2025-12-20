using System.Text;
using System.Text.Json;
using WebProje_B231210095.Models;

namespace WebProje_B231210095.Services
{
    public class GroqAiService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public GroqAiService(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public async Task<string> GeneratePlanAsync(AiInputViewModel model)
        {
            var apiKey = _configuration["Groq:ApiKey"];

            if (string.IsNullOrEmpty(apiKey))
            {
                return "API anahtarı bulunamadı. Lütfen yapılandırmayı kontrol edin.";
            }

            // 🧠 Yapay zekâ prompt'u
            var prompt = $@"
Yaş: {model.Yas}
Cinsiyet: {model.Cinsiyet}
Boy: {model.Boy} cm
Kilo: {model.Kilo} kg
Vücut Tipi: {model.VucutTipi}
Hedef: {model.Hedef}
Haftada: {model.HaftadaKacGun} gün

Buna göre:
- Türkçe cevap ver
- Haftalık egzersiz planı oluştur
- Kısa ve sağlıklı diyet önerisi ekle
- Öğrenci seviyesinde, anlaşılır anlat
";

            // 📦 Groq API request body
            var requestBody = new
            {
                
                model = "llama-3.1-8b-instant",
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://api.groq.com/openai/v1/chat/completions"
            );

            request.Headers.Add("Authorization", $"Bearer {apiKey}");
            request.Content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            // ❌ API hata dönerse
            if (!response.IsSuccessStatusCode)
            {
                return $"Yapay zekâ API hatası: {json}";
            }

            // 🛡️ Güvenli JSON okuma
            using var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("choices", out var choices) ||
                choices.GetArrayLength() == 0)
            {
                return "Yapay zekâ şu anda geçerli bir cevap üretemedi.";
            }

            var message = choices[0].GetProperty("message");

            if (!message.TryGetProperty("content", out var content))
            {
                return "Yapay zekâdan içerik alınamadı.";
            }

            return content.GetString();
        }
    }
}
