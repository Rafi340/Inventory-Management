using Inventory.Domain;
using Inventory.Domain.Dtos;
using Inventory.Domain.Entities.ViewModel;
using Inventory.Domain.Repositories;

namespace Inventory.Application
{
    public interface IApplicationUnitOfWork : IUnitOfWork
    {
        public IProductRepository ProductRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IUnitRepository UnitRepository { get; }
        public ICustomerRepository CustomerRepository { get; }
        public ISalesTypeRepository SalesTypeRepository { get; }
        public IAccountTypeRepository AccountTypeRepository { get; }
        public IAccountRepository AccountRepository { get; }
        public ISaleRepository SaleRepository { get; }
        public ISaleItemsRepository SaleItemsRepository { get; }
        public IBalanceTransferRepository BalanceTransferRepository { get; }
        Task<(IList<ProductViewModel> data, int total, int totalDisplay)> GetProductSP(int pageIndex, int pageSize, string? order, ProductSearchDto search);
    }
}
