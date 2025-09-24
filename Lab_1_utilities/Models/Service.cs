namespace Lab_1_utilities.Models
{
    public class Service: BaseEntity
    {
        public string Name { get; set; } = null!;
        public string BillingType { get; set; } = null!; // "area" | "person"
        public decimal Tariff { get; set; }
        public ICollection<TenantService> TenantServices { get; set; } = new List<TenantService>();
    }
}
