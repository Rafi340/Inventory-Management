using Inventory.Domain.Entities;
using Inventory.Domain.Features.Categories.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Repositories
{
    public interface ICategoryRepository : IRepository<Category, int>
    {
        Task<(IList<Category>, int, int)> GetPagedCategoryAsync(IGetCategoryQuery request);
    }
    
}
