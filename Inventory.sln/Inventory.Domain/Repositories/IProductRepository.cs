using Inventory.Domain.Entities;
using Inventory.Domain.Features.Prouduct.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Repositories
{
    public interface IProductRepository : IRepository<Product, Guid>
    {
        Task<(IList<Product>, int, int)> GetPagedProductAsync(IGetProductQuery request);

        (IList<Product> data, int total, int totalDisplay) GetPagedProducts(int pageIndex,
            int pageSize, string? order, DataTablesSearch search);
        bool IProductCodeDuplicate(string ProductCode, Guid? id = null);
    }
}
