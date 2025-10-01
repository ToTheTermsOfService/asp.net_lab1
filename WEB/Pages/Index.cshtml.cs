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
    }
}
