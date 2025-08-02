using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Exceptions
{
    public class DuplicateCustomerIdExceptions : Exception
    {
        public DuplicateCustomerIdExceptions() : base("CustomerId can't be duplicate") { }
    }
}
