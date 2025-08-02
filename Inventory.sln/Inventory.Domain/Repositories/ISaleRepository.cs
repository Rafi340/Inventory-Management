using Inventory.Domain.Entities;
using Inventory.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Repositories
{
    public interface ISaleRepository : IRepository<Sale, Guid>
    {
        Sale GetById(Guid Id);
        bool IsInvoiceDuplicate(string InvoiceNo, Guid? id = null);
        string GenerateInvoiceNo();
        Task<SaleViewModel> GetSaleById(Guid id);
        Task<IList<Sale>> GetSaleByCustomerId(Guid CustomerId);

    }
}
