using Lab_1_utilities.Data;
using Lab_1_utilities.Interfaces;
using Lab_1_utilities.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Lab_1_utilities.Services
{
    public class TenantServiceRepository: ITenantServiceRepository
    {
        private readonly TenantDbContext _dbContext;

        public TenantServiceRepository(TenantDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<TenantWithServices> GetAllTenantsWithServices()
        {
            var tenants = _dbContext.Tenants
                .Include(t => t.TenantServices)
                    .ThenInclude(ts => ts.Service)
                .OrderBy(t => t.Id)
                .Select(t => new TenantWithServices
                {
                    Id = t.Id,
                    LastName = t.LastName,
                    FirstName = t.FirstName,
                    MiddleName = t.MiddleName ?? "",
                    Address = t.Address,
                    PersonalAccount = t.PersonalAccount,
                    ResidentsCount = t.ResidentsCount,
                    ApartmentArea = t.ApartmentArea,
                    Services = t.TenantServices.Select(ts => new TenantServiceViewModel
                    {
                        ServiceName = ts.Service.Name,
                        BillingType = ts.Service.BillingType,
                        Tariff = ts.Service.Tariff,
                        CalculatedAmount = ts.CalculatedAmount
                    }).ToList()
                })
                .ToList();

            return tenants;
        }

        public Tenant? GetTenantById(int id)
        {
            return _dbContext.Tenants
                .Include(t => t.TenantServices)
                    .ThenInclude(ts => ts.Service)
                .FirstOrDefault(t => t.Id == id);
        }

        public Tenant AddTenant(Tenant tenant)
        {
            _dbContext.Tenants.Add(tenant);
            _dbContext.SaveChanges();
            return tenant;
        }

        public Tenant? UpdateTenant(int id, Tenant updatedTenant)
        {
            var tenant = _dbContext.Tenants.FirstOrDefault(t => t.Id == id);
            if (tenant == null)
                return null;

            tenant.FirstName = updatedTenant.FirstName;
            tenant.LastName = updatedTenant.LastName;
            tenant.MiddleName = updatedTenant.MiddleName;
            tenant.PersonalAccount = updatedTenant.PersonalAccount;
            tenant.Address = updatedTenant.Address;
            tenant.ResidentsCount = updatedTenant.ResidentsCount;
            tenant.ApartmentArea = updatedTenant.ApartmentArea;

            _dbContext.SaveChanges();
            return tenant;
        }

        public bool DeleteTenant(int id)
        {
            var tenant = _dbContext.Tenants.FirstOrDefault(t => t.Id == id);
            if (tenant == null)
                return false;

            _dbContext.Tenants.Remove(tenant);
            _dbContext.SaveChanges();
            return true;
        }

        public void AddServiceToTenant(int tenantId, int serviceId)
        {
            var tenant = _dbContext.Tenants.FirstOrDefault(t => t.Id == tenantId);
            var service = _dbContext.Services.FirstOrDefault(s => s.Id == serviceId);

            if (tenant != null && service != null)
            {
                var calculatedAmount = CalculateAmount(tenant, service);
                var relation = new TenantService
                {
                    TenantId = tenant.Id,
                    ServiceId = service.Id,
                    CalculatedAmount = calculatedAmount
                };

                _dbContext.TenantServices.Add(relation);
                _dbContext.SaveChanges();
            }
        }

        public List<Service> GetAllServices()
        {
            return _dbContext.Services.ToList();
        }

        public void AddService(Service service)
        {
            _dbContext.Services.Add(service);
            _dbContext.SaveChanges();
        }

        public List<Service> GetServicesByName(string name)
        {
            return _dbContext.Services
                .Where(s => s.Name.Contains(name))
                .ToList();
        }

        private decimal CalculateAmount(Tenant tenant, Service service)
        {
            if (service.BillingType == "area")
            {
                return service.Tariff * tenant.ApartmentArea;
            }
            else if (service.BillingType == "person")
            {
                return service.Tariff * tenant.ResidentsCount;
            }

            throw new InvalidOperationException("Невідомий тип нарахування для послуги.");
        }

    }
}
