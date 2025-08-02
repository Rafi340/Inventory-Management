using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Web.Areas.Admin.Models.Sales
{
    public class SaleItemAddModel
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        public SalesAddModel Sales { get; set; }
    }
}
