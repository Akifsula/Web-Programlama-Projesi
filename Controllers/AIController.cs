using KuaforYonetim.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class AIController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public AIController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    // GET: AI/Result
    public IActionResult Result(string imageUrl, string processedImageUrl, string suggestions)
    {
        var viewModel = new AIHairRecommendationViewModel
        {
            OriginalImageUrl = imageUrl,
            ProcessedImageUrl = processedImageUrl,
            Suggestions = suggestions
        };

        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new AIHairRecommendationViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> AnalyzeImage(AIHairRecommendationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Lütfen tüm alanları doldurun.";
            return View("Index", model);
        }

        var apiKey = _configuration["MagicApi:ApiKey"];
        var apiUrl = $"{_configuration["MagicApi:BaseUrl"]}/hair";

        var requestData = new
        {
            image = model.ImageUrl,
            editing_type = model.EditingType,
            color_description = model.ColorDescription,
            hairstyle_description = model.HairstyleDescription
        };

        Console.WriteLine($"Request Data: {JsonConvert.SerializeObject(requestData)}");

        var client = _httpClientFactory.CreateClient();
        var requestContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
        requestContent.Headers.Add("x-magicapi-key", apiKey);

        var response = await client.PostAsync(apiUrl, requestContent);

        if (response.IsSuccessStatusCode)
        {
            // API yanıtını oku
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"API Response Content: {responseContent}");

            // Yanıtı işleme
            dynamic json = JsonConvert.DeserializeObject(responseContent);

            // Yanıtın içeriğini kontrol et
            if (json?.result == null || string.IsNullOrEmpty(json?.result?.imageUrl?.ToString()))
            {
                model.ProcessedImageUrl = "https://via.placeholder.com/300"; // Geçici resim
                model.Suggestions = "Daha Kısa bir saç modeli düşünebilirsiniz ve Sacınızı griye boyama cesareti gösterirseniz pişman olmazsınız^^";
            }
            else
            {
                model.ProcessedImageUrl = json?.result?.imageUrl?.ToString();
                model.Suggestions = json?.result?.suggestions?.ToString();
            }

            model.OriginalImageUrl = model.ImageUrl; // Orijinal resim URL'sini sakla

            return View("Result", model);
        }
        else
        {
            // Hata durumunda yanıtı oku ve logla
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"API Error Response: {responseContent}");

            TempData["ErrorMessage"] = "Bir hata oluştu. Lütfen tekrar deneyiniz.";
            return View("Index", model);
        }
    }

}
