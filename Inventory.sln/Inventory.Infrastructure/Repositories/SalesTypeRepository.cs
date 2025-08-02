using Inventory.Domain.Entities;
using Inventory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Repositories
{
    public class SalesTypeRepository : Repository<SalesType, int> , ISalesTypeRepository
    {
        public SalesTypeRepository(ApplicationDbContext dbContext) : base(dbContext) { }
        
    }
}
