using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Exceptions
{
    public class EmtyItemExceptions : Exception
    {
        public EmtyItemExceptions() : base("Please Add Items For Create Sell") { }
    }
}
