namespace Lab_1_utilities.Models
{
    public class TenantWithServices
    {
        public int Id { get; set; }
        public string LastName  { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string MiddleName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PersonalAccount { get; set; } = null!;
        public int ResidentsCount { get; set; }
        public decimal ApartmentArea { get; set; }
        public List<TenantServiceViewModel> Services { get; set; } = new();
    }
}
