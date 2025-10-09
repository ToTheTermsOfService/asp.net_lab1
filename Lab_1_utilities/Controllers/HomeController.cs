using Lab_1_utilities.Interfaces;
using Lab_1_utilities.Models;
using Lab_1_utilities.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Lab_1_utilities.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITenantServiceRepository _repository;

        public HomeController(ILogger<HomeController> logger, ITenantServiceRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public IActionResult Index()
        {
            var tenants = _repository.GetAllTenantsWithServices();
            ViewBag.Services = _repository.GetAllServices();
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

        public IActionResult Details(int id)
        {
            var tenant = _repository.GetTenantById(id);
            if (tenant == null) return NotFound();
            return View(tenant);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Tenant tenant)
        {
            if (ModelState.IsValid)
            {
                _repository.AddTenant(tenant);
                return RedirectToAction(nameof(Index));
            }
            return View(tenant);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var tenant = _repository.GetTenantById(id);
            if (tenant == null) return NotFound();
            return View(tenant);
        }

        [HttpPost]
        public IActionResult Edit(int id, Tenant tenant)
        {
            if (id != tenant.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                var updatedTenant = _repository.UpdateTenant(id, tenant);
                if (updatedTenant == null) return NotFound();

                return RedirectToAction(nameof(Index));
            }
            return View(tenant);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var tenant = _repository.GetTenantById(id);
            if (tenant == null) return NotFound();
            return View(tenant);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _repository.DeleteTenant(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddServiceToTenant(int tenantId, int serviceId)
        {
            _repository.AddServiceToTenant(tenantId, serviceId);
            return RedirectToAction(nameof(Index));
        }
    }
}
