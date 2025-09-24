namespace Lab_1_utilities.Models
{
    public class TenantServiceViewModel
    {
        public string ServiceName { get; set; } = null!;
        public string BillingType { get; set; } = null!;
        public decimal Tariff { get; set; }
        public decimal? CalculatedAmount { get; set; }
    }
}
