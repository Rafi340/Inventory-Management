using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    public class Unit : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
       
    }
}
