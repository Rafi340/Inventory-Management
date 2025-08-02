using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Exceptions
{
    public class DuplicateProductCodeExceptions : Exception
    {
        public DuplicateProductCodeExceptions() : base("Product code can't be duplicate") { }
    }
}
