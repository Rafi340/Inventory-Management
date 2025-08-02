using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    public class AccountType : IEntity<int>
    {
        public int Id { get; set; }
        public required string Type { get; set; }
    }
}
