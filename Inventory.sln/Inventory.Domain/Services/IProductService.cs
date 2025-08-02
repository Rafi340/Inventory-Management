using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;
using Inventory.Domain.Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Services
{
    public interface IProductService
    {
        void AddProduct(Product model);
        (IList<Product> data, int total, int totalDisplay) GetProducts(int pageIndex, int pageSize,
            string? order, DataTablesSearch search);
        Product GetProduct(Guid productId);
        void Update(Product model);
        Task<(IList<ProductViewModel> data, int total, int totalDisplay)> GetProductSP(int pageIndex, int pageSize, string? order, ProductSearchDto search);
    }
}
