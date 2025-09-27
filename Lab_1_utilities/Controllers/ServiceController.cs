using Lab_1_utilities.Interfaces;
using Lab_1_utilities.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab_1_utilities.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ITenantServiceRepository _repository;

        public ServiceController(ITenantServiceRepository repository)
        {
            _repository = repository;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Service service)
        {
            if (ModelState.IsValid)
            {
                _repository.AddService(service);
                return RedirectToAction("Index", "Home");
            }
            return View(service);
        }
    }
}
