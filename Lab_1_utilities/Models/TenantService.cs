namespace Lab_1_utilities.Models
{
    public class TenantService : BaseEntity
    {
        public int TenantId { get; set; }
        public int ServiceId { get; set; }
        public decimal? CalculatedAmount { get; set; }
        public Tenant Tenant { get; set; } = null!;
        public Service Service { get; set; } = null!;
    }
}
