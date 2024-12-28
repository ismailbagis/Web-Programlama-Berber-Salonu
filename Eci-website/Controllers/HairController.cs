using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Drawing;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;

namespace Eci_website.Controllers
{
    public class HairController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IWebHostEnvironment _env; // IWebHostEnvironment eklendi

        public HairController(IHttpClientFactory clientFactory, IWebHostEnvironment env)
        {
            _clientFactory = clientFactory;
            _env = env; // IWebHostEnvironment ataması yapıldı
        }

        public IActionResult Index()
        {
            // Saç tipi kodları ve isimleri için bir sözlük oluşturun
            var hairTypeNames = new Dictionary<string, string>
            {
                { "101", "Bangs" },
                { "201", "Long hair" },
                { "301", "Bangs with long hair" },
                { "401", "Medium hair increase" },
                { "402", "Light hair increase" },
                { "403", "Heavy hair increase" },
                { "502", "Light curling" },
                { "503", "Heavy curling" },
                { "603", "Short hair" },
                { "801", "Blonde" },
                { "901", "Straight hair" },
                { "1001", "Oil-free hair" },
                { "1101", "Hairline fill" },
                { "1201", "Smooth hair" },
                { "1301", "Fill hair gap" }
            };
            ViewBag.HairTypeNames = hairTypeNames;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadHairstyle(IFormFile imageFile, string hairType1, string hairType2, string hairType3)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                ViewBag.Error = "Lütfen bir resim dosyası seçin.";
                return View("Index");
            }

            // Resmi geçici bir dosyaya kaydet
            var tempFilePath = Path.GetTempFileName();
            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // BURASI DEĞİŞTİ - BAŞLANGIÇ

            // Seçilen saç tiplerini kullanarak API isteklerini hazırla
            var hairTypes = new List<string> { hairType1, hairType2, hairType3 }.Where(s => !string.IsNullOrEmpty(s)).ToList();
            var client = _clientFactory.CreateClient("HairstyleClient");
            var tasks = new List<Task<string>>();

            foreach (var hairType in hairTypes)
            {
                tasks.Add(SendApiRequestAndGetBase64(client, tempFilePath, imageFile.FileName, hairType));
            }

            await Task.WhenAll(tasks);

            // Sonuçları ve seçilen saç tiplerini ViewBag'e ekle
            ViewBag.ResultImagesBase64 = tasks.Where(t => t.Result != null).Select(t => t.Result).ToList();
            ViewBag.SelectedHairTypes = hairTypes;

            // Geçici dosyayı sil
            System.IO.File.Delete(tempFilePath);

            // BURASI DEĞİŞTİ - SON

            return View("Result");
        }

        // YENİ METOD EKLENDİ - BAŞLANGIÇ
        private async Task<string> SendApiRequestAndGetBase64(HttpClient client, string filePath, string fileName, string hairType)
        {
            try
            {
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StreamContent(System.IO.File.OpenRead(filePath)), "image_target", fileName);
                    content.Add(new StringContent(hairType), "hair_type");

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri("huoshan/facebody/hairstyle", UriKind.Relative),
                        Headers =
                        {
                            // x-rapidapi-key ve x-rapidapi-host Program.cs'de HttpClient'a eklendi
                        },
                        Content = content
                    };

                    using (var response = await client.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();
                        var body = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<dynamic>(body);
                        return apiResponse.data.image;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API isteği hatası: {ex.Message}");
                // Hata mesajını ViewBag'e ekle (Result.cshtml'de göstermek için)
                if (ViewBag.Error == null)
                {
                    ViewBag.Error = new List<string>();
                }
                ViewBag.Error.Add($"Saç tipi '{hairType}' için API isteği başarısız oldu: {ex.Message}");
                return null;
            }
        }

    }
}