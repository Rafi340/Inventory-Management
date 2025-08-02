using Inventory.Domain.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.ListModel.Product
{
    public class ProductListModel : DataTables , IProductListModel
    {
        public ProductSearchDto? SearchItem { get; set; }
        public List<SelectListItem>? UnitList { get; set; }
        public List<SelectListItem>? CategoryList { get; set; }
    }
}
