using Inventory.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Inventory.Web.Areas.Admin.Models
{
    public class AddProductModel
    {
        [Required]
        public string ProductCode { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public string SKU { get; set; }
        [Required]
        public int UnitId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal MrpPrice { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal WholeSalePrice { get; set; }
        public int? LowStock { get; set; }
        public IFormFile? ImageFile { get; set; }
        public List<SelectListItem>? UnitList { get; set; }
        public List<SelectListItem>? CategoryList { get; set; }
    }
}
