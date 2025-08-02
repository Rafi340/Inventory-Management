using Inventory.Domain.Entities;
using Inventory.Domain.Features.Customers.Queries;
using Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Repositories
{
    public class CustomerRepository : Repository<Customer,Guid> , ICustomerRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CustomerRepository(ApplicationDbContext dbContext): base(dbContext)
        {
            _dbContext = dbContext;
        }

        public string GenerateCustomerId()
        {

            return $"CUS_{DateTime.UtcNow:yyMMddHH}_{Random.Shared.Next(1000, 10_000)}"; ;
        }

        public async Task<(IList<Customer>, int, int)> GetPagedCustomerAsync(IGetCustomerQuery request)
        {
            return await GetDynamicAsync(
                x => x.Name.Contains(request.Search.Value) ||
                x.CustomerId.Contains(request.Search.Value) ||
                x.Email.Contains(request.Search.Value) ||
                x.Mobile == request.Search.Value,
                request.FormatSortExpression("CustomerId", "Name", "Email", "Mobile", "Address" , "OpeningBalance"),
                null,
                request.PageIndex,
                request.PageSize,
                true
                );
        }

        public bool IsCustomerIdDuplicate(string CustomerId, Guid? id)
        {
            if (id.HasValue)
            {
                return GetCount(t=> t.CustomerId == CustomerId && t.Id != id) > 0;
            }
            return GetCount(t=> t.CustomerId == CustomerId) > 0;
        }
    }
}
