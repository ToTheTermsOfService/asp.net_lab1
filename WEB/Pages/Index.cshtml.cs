// WEB.Pages.IndexModel
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using WEB.DTOs;
using System.Text;
using System.Text.Json.Serialization;

namespace WEB.Pages
{
    public class IndexModel : PageModel
    {
        public List<TenantDto> Tenants { get; set; } = new();
        public List<ServiceDto> Services { get; set; } = new();

        [BindProperty]
        public TenantDto NewTenant { get; set; } = new();

        [BindProperty]
        public EditTenantRequest EditTenant { get; set; } = new();

        public bool ShowAddForm { get; set; } = false;

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IHttpClientFactory httpClientFactory, ILogger<IndexModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task OnGetAsync(string filter = "", string orderBy = "", int page = 1, int pageSize = 10)
        {
            var client = _httpClientFactory.CreateClient("API");

            var odataQuery = new StringBuilder("odata/TenantOData?");

            if (!string.IsNullOrEmpty(filter))
                odataQuery.Append($"$filter={Uri.EscapeDataString(filter)}&");

            if (!string.IsNullOrEmpty(orderBy))
                odataQuery.Append($"$orderby={Uri.EscapeDataString(orderBy)}&");

            odataQuery.Append($"$skip={(page - 1) * pageSize}&$top={pageSize}&$count=true");

            var response = await client.GetAsync(odataQuery.ToString());

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var odataResponse = JsonSerializer.Deserialize<ODataResponse<TenantDto>>(
                    json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                Tenants = odataResponse?.Value ?? new List<TenantDto>();
                TotalCount = odataResponse?.Count ?? 0;
            }

            // --- Services ---
            Services = await client.GetFromJsonAsync<List<ServiceDto>>("api/service")
                   ?? new List<ServiceDto>();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("API");
            var response = await client.PostAsJsonAsync("odata/TenantOData", NewTenant);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Помилка при створенні тенанта");
                ShowAddForm = true;
                await OnGetAsync();
                return Page();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAddServiceAsync(int tenantId, int serviceId)
        {
            var client = _httpClientFactory.CreateClient("API");

            // OData action
            var actionPayload = new { serviceId };
            var response = await client.PostAsJsonAsync(
                $"odata/Tenant({tenantId})/AddService",
                actionPayload
            );

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.DeleteAsync($"odata/TenantOData({id})");

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Мешканця успішно видалено!";
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = $"Помилка при видаленні: {errorContent}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("API");
            var response = await client.PutAsJsonAsync(
                $"odata/Tenant({EditTenant.Id})",
                EditTenant
            );

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Дані мешканця успішно оновлено!";
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = $"Помилка при оновленні: {errorContent}";
            }

            return RedirectToPage();
        }

        public class ODataResponse<T>
        {
            [JsonPropertyName("@odata.context")]
            public string Context { get; set; }

            [JsonPropertyName("@odata.count")]
            public int? Count { get; set; }

            public List<T> Value { get; set; }
        }

        public int TotalCount { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}