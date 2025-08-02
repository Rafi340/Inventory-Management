using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Exceptions
{
    public class DuplicateSaleExceptions : Exception
    {
        public DuplicateSaleExceptions(): base("Duplicate Invoice") { }
    }
}
