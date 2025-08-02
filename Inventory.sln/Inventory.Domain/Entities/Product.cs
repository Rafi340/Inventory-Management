using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    public class Product : BaseModel, IEntity<Guid> 
    {
        public Guid Id { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string SKU { get; set; }
        public int UnitId { get; set; }
        public int CategoryId { get; set; }
        public decimal UnitPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal MrpPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal WholeSalePrice { get; set; }
        public int? LowStock { get; set; }
        public int? DamangeStock { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }
        public Unit Unit { get; set; }
        public Category Category { get; set; }
    }
    
}
