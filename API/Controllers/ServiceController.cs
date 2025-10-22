using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly ITenantServiceRepository _repository;

        public ServiceController(ITenantServiceRepository repository)
        {
            _repository = repository;
        }
        // GET api/service
        [HttpGet]
        public IActionResult GetAllServices()
        {
            var services = _repository.GetAllServices();
            return Ok(services);
        }

        // POST api/service
        [HttpPost]
        public IActionResult AddService([FromBody] Service service)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _repository.AddService(service);
            return Ok(service);
        }

        // GET api/service/search?name=...
        [HttpGet("search")]
        public IActionResult GetServicesByName([FromQuery] string name)
        {
            var services = _repository.GetServicesByName(name);
            return Ok(services);
        }

        // DELETE api/tenant/5
        [HttpDelete("{id}")]
        public IActionResult DeleteTenant(int id)
        {
            var deleted = _repository.DeleteService(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
