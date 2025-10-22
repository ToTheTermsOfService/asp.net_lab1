using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using WEB.DTOs;

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
        public IndexModel(IHttpClientFactory httpClientFactory, ILogger<IndexModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        private readonly ILogger<IndexModel> _logger;

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("API");

            // --- Tenants ---
            Tenants = await client.GetFromJsonAsync<List<TenantDto>>("api/tenant")
                      ?? new List<TenantDto>();

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
            var tenant = NewTenant;
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.PostAsJsonAsync("api/tenant", tenant);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Помилка при створенні тенанта");
                ShowAddForm = true;
                await OnGetAsync();
                return RedirectToPage();
            }
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostAddServiceAsync(int tenantId, int serviceId)
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.PostAsync($"api/tenant/{tenantId}/service/{serviceId}", null);

            if (!response.IsSuccessStatusCode)
            {

                return BadRequest();
                //Show toaster
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.DeleteAsync($"api/tenant/{id}");

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
            var response = await client.PutAsJsonAsync($"api/tenant/{EditTenant.Id}", EditTenant);

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
    }
}
