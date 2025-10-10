namespace WEB.DTOs
{
    public class TenantDto
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string PersonalAccount { get; set; }
        public string Address { get; set; }
        public int ResidentsCount { get; set; }
        public decimal ApartmentArea { get; set; }

        public List<ServiceDto> Services { get; set; } = new();
    }
}
