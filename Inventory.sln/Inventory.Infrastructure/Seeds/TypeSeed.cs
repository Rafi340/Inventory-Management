using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Seeds
{
    public class TypeSeed
    {
        public static SalesType[] SaleTypeSeed()
        {
            return [
               new SalesType { Id= 1, Name = "Mrp Sales"  },
               new SalesType { Id= 2, Name = "Whole Sales"  },
                ];
        }
        public static AccountType[] AcountTypeSeed()
        {
            return [
                new AccountType { Id = 1, Type= "Cash"},
                new AccountType { Id = 2, Type= "Mobile Banking"},
                new AccountType { Id = 3, Type= "Bank"},
                new AccountType { Id = 4, Type= "Card"},
                ];
        }
    }
}
