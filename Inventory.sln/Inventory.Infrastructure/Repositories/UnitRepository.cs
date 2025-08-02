using Inventory.Domain.Entities;
using Inventory.Domain.Features.Prouduct.Queries;
using Inventory.Domain.Features.Units.Queries;
using Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Repositories
{
    public class UnitRepository : Repository<Unit, int>, IUnitRepository
    {
        public UnitRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<(IList<Unit>, int, int)> GetPagedUnitAsync(IGetUnitQuery request)
        {
            return await GetDynamicAsync(
                x => x.Name.Contains(request.Search.Value)
                ,
                request.FormatSortExpression("Name"),
                null,
                request.PageIndex,
                request.PageSize,
                true);
        }
    }
   
}
