using Lab_1_utilities.Models;

namespace Lab_1_utilities.Interfaces
{
    public interface ITenantServiceRepository
    {
        List<TenantWithServices> GetAllTenantsWithServices();
        Tenant? GetTenantById(int id);
        Tenant AddTenant(Tenant tenant);
        Tenant? UpdateTenant(int id, Tenant updatedTenant);
        bool DeleteTenant(int id);
        void AddServiceToTenant(int tenantId, int serviceId);
        List<Service> GetAllServices();
        void AddService(Service service);
        List<Service> GetServicesByName(string name);
    }
}
