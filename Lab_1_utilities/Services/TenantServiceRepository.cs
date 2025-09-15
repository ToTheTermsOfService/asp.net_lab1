using Lab_1_utilities.Models;
using Microsoft.Data.SqlClient;

namespace Lab_1_utilities.Services
{
    public class TenantServiceRepository
    {
        private readonly string _connectionString;
        public TenantServiceRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Localhost");
        }
        public List<TenantWithServices> GetAllTenantsWithServices()
        {
            var tenants = new List<TenantWithServices>();

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var query = @"
                SELECT 
                    t.tenant_id,
                    (t.last_name + ' ' + t.first_name + 
                        ISNULL(' ' + t.middle_name, '')) AS FullName,
                    t.address,
                    t.personal_account,
                    t.residents_count,
                    t.apartment_area,
                    s.name AS ServiceName,
                    s.billing_type,
                    s.tariff,
                    ts.calculated_amount
                FROM Tenant t
                JOIN Tenant_Service ts ON t.tenant_id = ts.tenant_id
                JOIN Service s ON ts.service_id = s.service_id
                ORDER BY t.tenant_id;
            ";

            using var command = new SqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                int tenantId = reader.GetInt32(0);
                var fullName = reader.GetString(1);

                var tenant = tenants.FirstOrDefault(t => t.PersonalAccount == reader.GetString(3));
                if (tenant == null)
                {
                    tenant = new TenantWithServices
                    {
                        FullName = fullName,
                        Address = reader.GetString(2),
                        PersonalAccount = reader.GetString(3),
                        ResidentsCount = reader.GetInt32(4),
                        ApartmentArea = reader.GetDecimal(5),
                        Services = new List<TenantServiceViewModel>()
                    };
                    tenants.Add(tenant);
                }

                tenant.Services.Add(new TenantServiceViewModel
                {
                    ServiceName = reader.GetString(6),
                    BillingType = reader.GetString(7),
                    Tariff = reader.GetDecimal(8),
                    CalculatedAmount = reader.GetDecimal(9)
                });
            }

            return tenants;
        }
    }
}
