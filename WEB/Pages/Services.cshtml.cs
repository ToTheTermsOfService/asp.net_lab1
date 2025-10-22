using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using WEB.DTOs;

namespace WEB.Pages
{
    public class ServicesModel : PageModel
    {
        public List<ServiceDto> Services { get; set; } = new();
        [BindProperty]
        public CreateServiceDto NewService { get; set; } = new();
        private readonly IHttpClientFactory _httpClientFactory;
        [BindProperty]
        public ServiceDto EditService { get; set; } = new();
        public ServicesModel(IHttpClientFactory httpClientFactory, ILogger<ServicesModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        private readonly ILogger<ServicesModel> _logger;
        public async Task OnGetAsync()
        {
            await LoadServices();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.PostAsJsonAsync("api/service", NewService);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Service created successfully!";
                return RedirectToPage("");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Error creating service: {errorContent}");
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.DeleteAsync($"api/service/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Сервіс успішно видалено!";
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = $"Помилка при видаленні: {errorContent}";
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditServiceAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadServices();
                return Page();
            }

            try
            {
                var client = _httpClientFactory.CreateClient("API");
                var response = await client.PutAsJsonAsync($"api/service/{EditService.Id}", EditService);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Сервіс успішно оновлено!";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = $"Помилка при оновленні сервісу: {errorContent}";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Сталася помилка: {ex.Message}";
            }

            return RedirectToPage();
        }

        private async Task LoadServices()
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.GetAsync("api/service");

            if (response.IsSuccessStatusCode)
            {
                Services = await response.Content.ReadFromJsonAsync<List<ServiceDto>>() ?? new List<ServiceDto>();
            }
        }

    }
}
