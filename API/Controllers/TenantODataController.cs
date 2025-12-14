using Core.Dto;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace API.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    public class TenantODataController : ODataController
    {
        private readonly ITenantServiceRepository _repository;

        public TenantODataController(ITenantServiceRepository repository)
        {
            _repository = repository;
        }

        // GET odata/Tenant
        [EnableQuery] // Увімкнення OData запитів
        [HttpGet]
        public IActionResult GetAllTenantsWithServices()
        {
            var tenants = _repository.GetAllTenantsWithServices();
            return Ok(tenants.AsQueryable());
        }

        // GET odata/Tenant(5)
        [EnableQuery]
        [HttpGet("{id}")]
        public IActionResult GetTenantById(int id)
        {
            var tenant = _repository.GetTenantById(id);
            if (tenant == null) return NotFound();
            return Ok(tenant);
        }

        // POST odata/Tenant
        [HttpPost]
        public IActionResult AddTenant([FromBody] CreateTenantDto createTenant)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            Tenant tenant = new Tenant
            {
                LastName = createTenant.LastName,
                FirstName = createTenant.FirstName,
                MiddleName = createTenant.MiddleName,
                Address = createTenant.Address,
                PersonalAccount = createTenant.PersonalAccount,
                ResidentsCount = createTenant.ResidentsCount,
                ApartmentArea = createTenant.ApartmentArea
            };

            var created = _repository.AddTenant(tenant);
            return Created(created);
        }

        // PUT odata/Tenant(5)
        [HttpPut("{id}")]
        public IActionResult UpdateTenant(int id, [FromBody] Tenant tenant)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = _repository.UpdateTenant(id, tenant);
            if (updated == null) return NotFound();

            return Updated(updated);
        }

        // DELETE odata/Tenant(5)
        [HttpDelete("{id}")]
        public IActionResult DeleteTenant(int id)
        {
            var deleted = _repository.DeleteTenant(id);
            if (!deleted) return NotFound();

            return NoContent();
        }

        // POST odata/Tenant(5)/AddService
        [HttpPost("{tenantId}/AddService")]
        public IActionResult AddServiceToTenant(int tenantId, [FromBody] ODataActionParameters parameters)
        {
            if (!parameters.TryGetValue("serviceId", out object serviceIdObj) ||
                !int.TryParse(serviceIdObj?.ToString(), out int serviceId))
                return BadRequest("serviceId is required");

            _repository.AddServiceToTenant(tenantId, serviceId);
            return Ok();
        }
    }
}

