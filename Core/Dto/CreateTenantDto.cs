namespace Core.Dto
{
    public class CreateTenantDto
    {
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string PersonalAccount { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int ResidentsCount { get; set; }
        public decimal ApartmentArea { get; set; }
    }
}
