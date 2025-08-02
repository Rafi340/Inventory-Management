using Inventory.Domain.Entities;
using Inventory.Domain.Features.Categories.Queries;
using Inventory.Domain.Features.Units.Queries;
using Inventory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category, int>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<(IList<Category>, int, int)> GetPagedCategoryAsync(IGetCategoryQuery request)
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
