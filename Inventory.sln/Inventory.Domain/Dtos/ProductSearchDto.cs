using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Dtos
{
    public class ProductSearchDto
    {
        public string Name { get; set; }
        public string ProductCode { get; set; }
        public int? UnitId { get; set; }
        public int? CategoryId { get; set; }
    }
}
