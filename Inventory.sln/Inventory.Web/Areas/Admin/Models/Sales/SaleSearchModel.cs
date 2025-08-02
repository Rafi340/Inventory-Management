using Inventory.Domain;
using Inventory.Domain.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inventory.Web.Areas.Admin.Models
{
    public class SaleSearchModel :  DataTables
    {
        public SaleSearchDto SearchItem { get; set; }
        public SelectList CustomerList { get; set; }
    }
}
