using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.ViewModel
{
    public class DashboardViewModel
    {
        public int TotalCustomer { get; set; }
        public int TotalProduct { get; set; }
        public decimal TotalSellPrice { get; set; }

    }
}
