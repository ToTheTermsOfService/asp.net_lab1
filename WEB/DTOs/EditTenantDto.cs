namespace WEB.DTOs
{
    public class EditTenantRequest
    {
        public int Id { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string Address { get; set; } = string.Empty;
        public string PersonalAccount { get; set; } = string.Empty;
        public int ResidentsCount { get; set; }
        public decimal ApartmentArea { get; set; }
    }
}
