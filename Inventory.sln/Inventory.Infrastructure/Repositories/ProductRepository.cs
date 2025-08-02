using Inventory.Application.Features.Products.Queries;
using Inventory.Domain;
using Inventory.Domain.Entities;
using Inventory.Domain.Features.Prouduct.Queries;
using Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product, Guid>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<(IList<Product>, int, int)> GetPagedProductAsync(IGetProductQuery request)
        {
            return await GetDynamicAsync(
                x => x.Name.Contains(request.Search.Value) 
                || x.SKU == request.Search.Value,
                request.FormatSortExpression("Name", "Description", "SKU" ,"Unit" ,"UnitPrice", "Quantity" ,"CreatedAt"),
                y => y.Include(z => z.Unit).Include(x=> x.Category)
                ,
                request.PageIndex,
                request.PageSize,
                true);
        }
        public (IList<Product> data, int total, int totalDisplay) GetPagedProducts(int pageIndex,
            int pageSize, string? order, DataTablesSearch search)
        {
            if (string.IsNullOrWhiteSpace(search.Value))
                return GetDynamic(null, order, null, pageIndex, pageSize, true);
            else
                return GetDynamic(x => x.Name.Contains(search.Value) ||
                x.SKU.Contains(search.Value), order,
                    null, pageIndex, pageSize, true);
        }
        public bool IProductCodeDuplicate(string ProductCode, Guid? id)
        {
            if (id.HasValue)
            {
                return GetCount(t => t.ProductCode == ProductCode && t.Id != id) > 0;
            }
            return GetCount(t => t.ProductCode == ProductCode) > 0;
        }
    }
    
}
