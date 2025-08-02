using Inventory.Domain.Entities;
using Inventory.Domain.Features.Customers.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Repositories
{
    public interface ICustomerRepository : IRepository<Customer, Guid>
    {
        Task<(IList<Customer>, int, int)> GetPagedCustomerAsync(IGetCustomerQuery request);
        bool IsCustomerIdDuplicate(string CustomerId, Guid? id = null);
        string GenerateCustomerId();
    }
}
