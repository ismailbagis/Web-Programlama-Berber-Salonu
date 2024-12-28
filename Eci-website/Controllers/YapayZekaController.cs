using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Eci_website.Controllers
{
    public class YapayZekaController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private const string ApiEndpoint = "https://api.openai.com/v1/images/generations"; // OpenAI API Endpoint
        private readonly string _apiKey; // API anahtarı (kullanıcıdan alınacak)

        public YapayZekaController(IWebHostEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            _apiKey = configuration["OpenAI:ApiKey"]; // appsettings.json'dan API anahtarını alıyoruz
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string prompt)
        {
            if (string.IsNullOrEmpty(prompt))
            {
                ViewBag.Error = "just change hair add another hair";
                return View();
            }

            try
            {
                // OpenAI API çağrısı yap
                string imageUrl = await GenerateImage(prompt);
                ViewBag.ImageUrl = imageUrl;
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"API Hatası: {ex.Message}";
            }

            return View();
        }

        private async Task<string> GenerateImage(string prompt)
        {
            using (var client = new HttpClient())
            {
                // API Anahtarını Authorization header'ına ekliyoruz
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

                // İstek verilerini hazırlıyoruz
                var requestBody = new
                {
                    prompt = prompt,
                    n = 1, // 1 adet görsel oluştur
                    size = "1024x1024" // Görsel boyutu
                };

                var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

                // API'ye POST isteği gönderiyoruz
                var response = await client.PostAsync(ApiEndpoint, jsonContent);
                string responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // JSON yanıtını deserialize ediyoruz
                    var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    return jsonResponse?.data[0]?.url;
                }
                else
                {
                    throw new Exception(responseContent);
                }
            }
        }
    }
}