using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class HairChangerController : Controller
{
    private readonly HairChangerService _hairChangerService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public HairChangerController(IWebHostEnvironment webHostEnvironment)
    {
        _hairChangerService = new HairChangerService();
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    public ActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> ChangeHair(string hairColor, IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            ViewBag.Error = "Lütfen bir görüntü yükleyin.";
            return View("Index");
        }

        try
        {
            // Dosya boyutunu kontrol et (10MB sınırı)
            if (imageFile.Length > 10 * 1024 * 1024) // 10MB'dan büyük dosyaları kabul etmiyoruz
            {
                ViewBag.Error = "Dosya boyutu çok büyük. Lütfen 10MB'dan küçük bir dosya yükleyin.";
                return View("Index");
            }

            // Benzersiz dosya adı oluşturma
            var fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
            var fileExtension = Path.GetExtension(imageFile.FileName);
            var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{fileExtension}";

            // wwwroot dizininde Uploads klasörünün yolu
            var uploadsPath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads");

            // Eğer klasör yoksa, oluşturuyoruz
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            var filePath = Path.Combine(uploadsPath, uniqueFileName);

            // Dosyayı kaydediyoruz
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // API çağrısını yap
            var result = await _hairChangerService.ChangeHairAsync(filePath, hairColor);

            // Sonuçları View'e gönder
            ViewBag.Result = result;
        }
        catch (Exception ex)
        {
            // Hata mesajını daha ayrıntılı şekilde kaydedelim
            ViewBag.Error = $"Dosya kaydedilirken bir hata oluştu: {ex.Message}\n{ex.StackTrace}";
        }

        return View("Index");
    }


}

