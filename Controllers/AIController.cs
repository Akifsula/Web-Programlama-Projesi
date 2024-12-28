using KuaforYonetim.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

//yapay zekayla alakalı mehtotları ve API çekimi için bu controllerı olusturdum

public class AIController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public AIController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> AnalyzeImage(string imageUrl, string editingType, string colorDescription, string hairstyleDescription)
    {
        string ApiUrl = $"{_configuration["MagicApi:BaseUrl"]}/hair";
        string ApiKey = _configuration["MagicApi:ApiKey"];

        var requestData = new
        {
            image = imageUrl,
            editing_type = editingType,
            color_description = colorDescription,
            hairstyle_description = hairstyleDescription
        };

        try
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiUrl)
            {
                Content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json")
            };
            requestMessage.Headers.Add("x-magicapi-key", ApiKey);

            var response = await _httpClientFactory.CreateClient().SendAsync(requestMessage);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                dynamic json = JsonConvert.DeserializeObject(jsonResponse);
                string requestId = json?.request_id;

                Console.WriteLine($"Request ID: {requestId}");
                TempData["RequestId"] = requestId;

                return RedirectToAction("GetResult");
            }
            else
            {
                Console.WriteLine($"API Error: {jsonResponse}");
                TempData["ErrorMessage"] = "Yapay zeka servisine bağlanılamadı. Lütfen tekrar deneyin.";
                return View("Index");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            TempData["ErrorMessage"] = "Bir hata oluştu. Lütfen tekrar deneyin.";
            return View("Index");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetResult()
    {
        string requestId = TempData["RequestId"]?.ToString();
        string ApiKey = _configuration["MagicApi:ApiKey"];
        string ApiUrl = $"{_configuration["MagicApi:BaseUrl"]}/predictions/{requestId}";

        if (string.IsNullOrEmpty(requestId))
        {
            TempData["ErrorMessage"] = "Request ID bulunamadı.";
            return View("Index");
        }

        try
        {
            var result = await CheckProcessingStatus(ApiUrl, ApiKey, requestId);

            if (!string.IsNullOrEmpty(result))
            {
                ViewData["ProcessedImageUrl"] = result;
                return View("Result");
            }
            else
            {
                TempData["ErrorMessage"] = "İşlem başarısız oldu veya sonuç bulunamadı.";
                return View("Index");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            TempData["ErrorMessage"] = "Bir hata oluştu. Lütfen tekrar deneyin.";
            return View("Index");
        }
    }

    private async Task<string> CheckProcessingStatus(string apiUrl, string apiKey, string requestId)
    {
        var client = _httpClientFactory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, apiUrl);
        requestMessage.Headers.Add("x-magicapi-key", apiKey);

        try
        {
            var response = await client.SendAsync(requestMessage);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            dynamic json = JsonConvert.DeserializeObject(jsonResponse);
            string status = json?.status;
            string result = json?.result;

            Console.WriteLine($"Status: {status}");
            Console.WriteLine($"Result: {result}");

            if (status == "succeeded" && !string.IsNullOrEmpty(result))
            {
                return result;
            }
            else if (status == "starting" || status == "processing")
            {
                Console.WriteLine($"Status is {status}, retrying in 5 seconds...");
                await Task.Delay(5000);
                return await CheckProcessingStatus(apiUrl, apiKey, requestId);
            }
            else
            {
                Console.WriteLine($"Error: Status {status}, Result: {result}");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception during status check: {ex.Message}");
            return null;
        }
    }
}
