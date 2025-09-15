using Lab_1_utilities.Models;
using Lab_1_utilities.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Lab_1_utilities.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TenantServiceRepository _repository;

        public HomeController(ILogger<HomeController> logger, TenantServiceRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public IActionResult Index()
        {
            var tenants = _repository.GetAllTenantsWithServices();
            return View(tenants);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
