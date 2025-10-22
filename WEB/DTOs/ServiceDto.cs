namespace WEB.DTOs
{
    public class ServiceDto
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string BillingType { get; set; }
        public decimal Tariff { get; set; }
        public decimal CalculatedAmount { get; set; } = 0;
    }
}
