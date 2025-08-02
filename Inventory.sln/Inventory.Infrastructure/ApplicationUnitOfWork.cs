using Inventory.Application;
using Inventory.Domain.Dtos;
using Inventory.Domain.Entities.ViewModel;
using Inventory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure
{
    public class ApplicationUnitOfWork : UnitOfWork , IApplicationUnitOfWork
    {
        public IProductRepository ProductRepository { get; private set; }
        public IUnitRepository UnitRepository { get; private set; }
        public ICategoryRepository CategoryRepository { get; private set; }

        public ICustomerRepository CustomerRepository { get; private set; }
        public ISalesTypeRepository SalesTypeRepository { get; private set; }
        public IAccountTypeRepository AccountTypeRepository { get; private set; }
        public IAccountRepository AccountRepository { get; private set; }
        public ISaleRepository SaleRepository { get; private set; }
        public ISaleItemsRepository SaleItemsRepository { get; private set; }
        public IBalanceTransferRepository BalanceTransferRepository { get; private set; }
        public ApplicationUnitOfWork(ApplicationDbContext context, 
            IProductRepository productRepository,
            IUnitRepository unitRepository,
            ICategoryRepository categoryRepository,
            ICustomerRepository customerRepository,
            ISalesTypeRepository salesRepository,
            IAccountTypeRepository accountTypeRepository,
            IAccountRepository accountRepository,
            ISaleRepository saleRepository,
            ISaleItemsRepository saleItemsRepository,
            IBalanceTransferRepository balanceTransferRepository) : base(context)
        {
            ProductRepository = productRepository;
            UnitRepository = unitRepository;
            CategoryRepository = categoryRepository;
            CustomerRepository = customerRepository;
            SalesTypeRepository = salesRepository;
            AccountTypeRepository = accountTypeRepository;
            AccountRepository = accountRepository;
            SaleRepository = saleRepository;
            SaleItemsRepository = saleItemsRepository;
            BalanceTransferRepository = balanceTransferRepository;
        }
        public async Task<(IList<ProductViewModel> data, int total, int totalDisplay)> GetProductSP(int pageIndex, int pageSize, string? order, ProductSearchDto search)
        {
            var procedureName = "GetProducts";

            var result = await SqlUtility.QueryWithStoredProcedureAsync<ProductViewModel>(procedureName,
                new Dictionary<string, object?>
                {
                    { "PageIndex", pageIndex },
                    { "PageSize", pageSize },
                    { "OrderBy", order ?? "Name" },
                    { "UnitId", search.UnitId },
                    { "CategoryId" , search.CategoryId },
                    { "Name", string.IsNullOrEmpty(search.Name) ? null : search.Name },
                    { "ProductCode", string.IsNullOrEmpty(search.ProductCode) ? null : search.ProductCode }
                },
                new Dictionary<string, Type>
                {
                    { "Total", typeof(int) },
                    { "TotalDisplay", typeof(int) },
                });

            return (result.result, (int)result.outValues["Total"], (int)result.outValues["TotalDisplay"]);
        }

    }
   
}
