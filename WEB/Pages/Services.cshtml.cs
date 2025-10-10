using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using WEB.DTOs;

namespace WEB.Pages
{
    public class ServicesModel : PageModel
    {
        public List<ServiceDto> Services { get; set; } = new();
        private readonly IHttpClientFactory _httpClientFactory;
        public ServicesModel(IHttpClientFactory httpClientFactory, ILogger<ServicesModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        private readonly ILogger<ServicesModel> _logger;
        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("API");
            // --- Services ---
            Services = await client.GetFromJsonAsync<List<ServiceDto>>("api/service")
                       ?? new List<ServiceDto>();
        }
    }
}
