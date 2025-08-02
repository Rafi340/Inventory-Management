using Inventory.Domain.Entities;
using Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Repositories
{
    public class AccountTypeRepository : Repository<AccountType, int>, IAccountTypeRepository
    {
        public AccountTypeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
