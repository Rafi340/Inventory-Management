using Inventory.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inventory.Web.Areas.Admin.Models
{
    public class ProductListModel : DataTables
    {
        public ProductSearchModel? SearchItem { get; set; }
        public List<SelectListItem>? UnitList { get; set; }
        public List<SelectListItem>? CategoryList { get; set; }
    }
}
