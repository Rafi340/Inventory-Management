using Inventory.Domain;
using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;
using Inventory.Domain.Entities.ViewModel;
using Inventory.Domain.Repositories;
using Inventory.Domain.Services;

namespace Inventory.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public ProductService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public void AddProduct(Product model)
        {
            _applicationUnitOfWork.ProductRepository.Add(model);
            _applicationUnitOfWork.Save();
        }
        public (IList<Product> data, int total, int totalDisplay) GetProducts(int pageIndex,
            int pageSize, string? order, DataTablesSearch search)
        {
            return _applicationUnitOfWork.ProductRepository.GetPagedProducts(pageIndex, pageSize, order, search);
        }
        public Product GetProduct(Guid productId)
        {
           return _applicationUnitOfWork.ProductRepository.GetById(productId);
        }
        public void Update(Product model)
        {
            _applicationUnitOfWork.ProductRepository.Update(model);
            _applicationUnitOfWork.Save();
        }

        public async Task<(IList<ProductViewModel> data, int total, int totalDisplay)> GetProductSP(int pageIndex, int pageSize, string? order, ProductSearchDto search)
        {
            return await _applicationUnitOfWork.GetProductSP(pageIndex, pageSize, order,search);
        }
    }
}
