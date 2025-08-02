using Inventory.Domain.Entities;
using Inventory.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Repositories
{
    public interface ISaleItemsRepository : IRepository<SaleItems, Guid>
    {
        Task<IList<SaleItems>> GetItemsBySaleId(Guid saleId);
        Task<IList<SaleItemsViewModel>> GetSaleItemsBySaleId(Guid saleId);
    }
}
