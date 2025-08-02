using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Web.Areas.Admin.Models
{
    public class CustomerUpdateModel
    {
        public Guid Id { get; set; }
        public required string CustomerId { get; set; }
        [Required]
        public required string Name { get; set; }
        public string? CompanyName { get; set; }
        public string? Email { get; set; }
        [Required]
        public required string Mobile { get; set; }
        public string? Address { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? Image { get; set; }
        public string? ImageUrl { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OpeningBalance { get; set; }
        [Required]
        public int Status { get; set; }
    }
}
