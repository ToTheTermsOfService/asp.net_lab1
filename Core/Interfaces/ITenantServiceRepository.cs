using Core.Dto;
using Core.Entities;

namespace Core.Interfaces
{
    public interface ITenantServiceRepository
    {
        List<Tenant> GetAllTenantsWithServices();
        Tenant? GetTenantById(int id);
        Tenant AddTenant(Tenant tenant);
        Tenant? UpdateTenant(int id, Tenant updatedTenant);
        bool DeleteTenant(int id);
        bool DeleteService(int id); 
        void AddServiceToTenant(int tenantId, int serviceId);
        List<TenantServiceViewModel> GetAllServices();
        void AddService(Service service);
        List<Service> GetServicesByName(string name);
    }
}
