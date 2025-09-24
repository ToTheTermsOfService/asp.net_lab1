using Lab_1_utilities.Data;
using Lab_1_utilities.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Lab_1_utilities.Services
{
    public class TenantServiceRepository
    {
        private readonly string _connectionString;
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
                    FullName = t.LastName + " " + t.FirstName +
                               (string.IsNullOrEmpty(t.MiddleName) ? "" : " " + t.MiddleName),
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
    }
}
