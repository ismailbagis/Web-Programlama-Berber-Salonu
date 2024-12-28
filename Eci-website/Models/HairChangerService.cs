using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class HairChangerService
{
    private readonly HttpClient _httpClient;

    public HairChangerService()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer hqW8vDugrJUzo0LHVN3FtyHS0ml4ZGFKqXjiBw1zbnPto85yEi1nPCweSX7uJUhQ"); // API anahtarı
        _httpClient.Timeout = TimeSpan.FromMinutes(5); // Zaman aşımını 5 dakika olarak ayarladık
    }

    public async Task<string> ChangeHairAsync(string imagePath, string hairColor)
    {
        var url = "https://www.ailabapi.com/api/portrait/effects/hairstyle-editor"; // API URL

        // Dosyayı doğrudan okuyup gönderelim
        byte[] fileBytes = System.IO.File.ReadAllBytes(imagePath);
        var fileContent = new ByteArrayContent(fileBytes);
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg"); // Dosya türünü belirtiyoruz

        var formData = new MultipartFormDataContent();
        formData.Add(fileContent, "image", "image.jpg"); // Dosyayı form verisi olarak ekliyoruz
        formData.Add(new StringContent(hairColor), "color"); // Renk parametresini ekliyoruz

        // API'ye POST isteği gönder
        var response = await _httpClient.PostAsync(url, formData);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"API çağrısı başarısız: {response.StatusCode}");
        }

        var result = await response.Content.ReadAsStringAsync();
        return result; // Dönen sonucu JSON olarak alın (örneğin, yeni görüntü URL'si)
    }
}



