using Inventory.Domain;
using Inventory.Domain.Utilities;
using Inventory.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbConext;
        public ISqlUtility SqlUtility { get; private set; }
        public UnitOfWork(DbContext dbConext)
        {
            _dbConext = dbConext;
            SqlUtility = new SqlUtility(_dbConext.Database.GetDbConnection());
        }
        public void Save()
        {
            _dbConext.SaveChanges();
        }

        public async Task SaveAsync()
        {
           await _dbConext.SaveChangesAsync();
        }
    }
}
