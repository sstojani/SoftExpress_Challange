using Microsoft.AspNetCore.Mvc;
using SoftExpress_Challange.ViewModels;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class ActionsController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ActionsController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    private string GetApiUrl()
    {
        // Construct the base URL dynamically from the current request
        var request = HttpContext.Request;
        var baseUrl = $"{request.Scheme}://{request.Host}";
        return $"{baseUrl}/api/actions/";
    }

    //Index
    public async Task<IActionResult> Index(
        string? userId,
        string? rajoni,
        string? llojiIPajisjes,
        string? aplikacioni,
        DateTime? dataOra
    )
    {
        var apiUrl = GetApiUrl();
        var httpClient = _httpClientFactory.CreateClient();

        var queryParameters = new List<string>();
        if (!string.IsNullOrEmpty(rajoni)) queryParameters.Add($"rajoni={rajoni}");
        if (!string.IsNullOrEmpty(llojiIPajisjes)) queryParameters.Add($"llojiIPajisjes={llojiIPajisjes}");
        if (!string.IsNullOrEmpty(aplikacioni)) queryParameters.Add($"aplikacioni={aplikacioni}");
        if (dataOra.HasValue) queryParameters.Add($"dataOra={dataOra.Value.ToString("yyyy-MM-dd")}");

        var queryString = queryParameters.Count > 0 ? $"?{string.Join("&", queryParameters)}" : "";

        var response = await httpClient.GetAsync($"{apiUrl}{queryString}");
        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Unable to fetch data.";
            return View(new StatistikaIndexViewModel());
        }

        var content = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var statistikas = JsonSerializer.Deserialize<List<StatistikaDto>>(content, options);

        var viewModel = new StatistikaIndexViewModel
        {
            Statistikas = statistikas ?? new List<StatistikaDto>(),
            UserId = userId,
            Rajoni = rajoni,
            LlojiIPajisjes = llojiIPajisjes,
            Aplikacioni = aplikacioni,
            DataOra = dataOra
        };
        return View(viewModel);
    }

    //Create

    [HttpGet]
    public IActionResult Create(string userId)
    {
        var model = new StatistikaCreateDto
        {
            UserId = Guid.Parse(userId),
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(StatistikaCreateDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var apiUrl = GetApiUrl();
        var httpClient = _httpClientFactory.CreateClient();

        var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync(apiUrl, jsonContent);

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Error creating statistika.";
            return View(model);
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Update(Guid id, StatistikaDto model)
    {
        var apiUrl = GetApiUrl();
        var httpClient = _httpClientFactory.CreateClient();

        var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
        var response = await httpClient.PutAsync($"{apiUrl}{id}", jsonContent);

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Error updating statistika.";
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var apiUrl = GetApiUrl();
        var httpClient = _httpClientFactory.CreateClient();

        var response = await httpClient.DeleteAsync($"{apiUrl}{id}");
        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Error deleting statistika.";
        }

        return RedirectToAction("Index");
    }
}
