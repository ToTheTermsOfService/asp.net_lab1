using Core.Dto;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly ITenantServiceRepository _repository;

        public TenantController(ITenantServiceRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public IActionResult GetAllTenantsWithServices()
        {
            var tenants = _repository.GetAllTenantsWithServices();
            return Ok(tenants);
        }
        [HttpGet("{id}")]
        public IActionResult GetTenantById(int id)
        {
            var tenant = _repository.GetTenantById(id);
            if (tenant == null) return NotFound();
            return Ok(tenant);
        }

        // POST api/tenant
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
            return CreatedAtAction(nameof(GetTenantById), new { id = created.Id }, created);
        }

        // PUT api/tenant/5
        [HttpPut("{id}")]
        public IActionResult UpdateTenant(int id, [FromBody] Tenant tenant)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = _repository.UpdateTenant(id, tenant);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

        // DELETE api/tenant/5
        [HttpDelete("{id}")]
        public IActionResult DeleteTenant(int id)
        {
            var deleted = _repository.DeleteTenant(id);
            if (!deleted) return NotFound();

            return NoContent();
        }

        // POST api/tenant/{tenantId}/service/{serviceId}
        [HttpPost("{tenantId}/service/{serviceId}")]
        public IActionResult AddServiceToTenant(int tenantId, int serviceId)
        {
            _repository.AddServiceToTenant(tenantId, serviceId);
            return Ok();
        }
    }
}
