using System.ComponentModel.DataAnnotations;

namespace WEB.DTOs
{
    public class CreateServiceDto
    {
        [Required(ErrorMessage = "Service name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Billing type is required")]
        [RegularExpression("^(area|person)$", ErrorMessage = "Billing type must be 'area' or 'person'")]
        public string BillingType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tariff is required")]
        [Range(0.01, 10000, ErrorMessage = "Tariff must be between 0.01 and 10000")]
        public decimal Tariff { get; set; }
    }
}
