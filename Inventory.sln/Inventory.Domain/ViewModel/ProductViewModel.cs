using System;

namespace Inventory.Domain.Entities.ViewModel
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string SKU { get; set; }
        public string UnitName { get; set; }
        public string CategoryName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int? LowStock { get; set; }
	    public int?  DamageStock { get; set; }
	    public string ProductCode { get; set; }
	    public decimal? MrpPrice { get; set; }
        public decimal? WholeSalePrice { get; set; }
        public string?  ImageUrl { get; set; }
        public int Status { get; set; }
	    public string? StatusDesc { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
