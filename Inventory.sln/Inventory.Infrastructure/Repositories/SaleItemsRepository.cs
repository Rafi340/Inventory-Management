using Inventory.Domain.Entities;
using Inventory.Domain.Repositories;
using Inventory.Domain.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Repositories
{
    public class SaleItemsRepository : Repository<SaleItems, Guid>, ISaleItemsRepository
    {
        private readonly ApplicationDbContext _context;
        public SaleItemsRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IList<SaleItems>> GetItemsBySaleId(Guid saleId)
        {
            return  GetDynamic(t=> t.SalesId == saleId,null,null,false);
        }

        public async Task<IList<SaleItemsViewModel>> GetSaleItemsBySaleId(Guid saleId)
        {
            var saleItems = (from item in _context.SalesItems
                             join product in _context.Product on item.ProductId equals product.Id
                             where item.SalesId == saleId
                             select new SaleItemsViewModel
                             {
                                 Id = item.ProductId,
                                 SalesId = item.SalesId,
                                 ProductId = item.ProductId,
                                 ProductCode = product.ProductCode,
                                 Name = product.Name,
                                 Quantity = item.Quantity,
                                 Stock = product.Quantity,
                                 UnitPrice = item.UnitPrice,
                             }
                             ).AsQueryable();
            return await saleItems.ToListAsync();
        }
    }
}
