using Inventory.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inventory.Web.Areas.Admin.Models
{
    public class ProductSearchModel
    {
        public string Name { get; set; }
        public string ProductCode { get; set; }
        public int? UnitId { get; set; }
        public int? CategoryId { get; set; }
        
    }
}
