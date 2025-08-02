using Inventory.Domain.Entities;
using Inventory.Domain.Features.Prouduct.Queries;
using Inventory.Domain.Features.Units.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Repositories
{
    public interface IUnitRepository : IRepository<Unit, int>
    {
        Task<(IList<Unit>, int, int)> GetPagedUnitAsync(IGetUnitQuery request);
    }
}
