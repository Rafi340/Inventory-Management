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
    public class SaleRepository : Repository<Sale, Guid>, ISaleRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public SaleRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public string GenerateInvoiceNo()
        {
            return $"INV_{DateTime.UtcNow:yyMMddHH}_{Random.Shared.Next(1000, 10_000)}";
        }

        public bool IsInvoiceDuplicate(string InvoiceNo, Guid? id)
        {
            if (id.HasValue)
            {
                return GetCount(t => t.InvoiceNo == InvoiceNo && t.Id != id) > 0;
            }
            return GetCount(t => t.InvoiceNo == InvoiceNo) > 0;
        }

        public async Task<SaleViewModel> GetSaleById(Guid id)
        {
            var sale = await  (from s in _dbContext.Sales
                        join at in _dbContext.AccountType on s.AcountTypeId equals at.Id
                         join c in _dbContext.Customer on s.CustomerId equals c.Id
                        join st in _dbContext.SalesType on s.SalesTypeId equals st.Id
                        join a in _dbContext.Account on s.AccountId equals a.Id
                        select new SaleViewModel
                        {
                            Id = s.Id,
                            InvoiceNo = s.InvoiceNo,
                            SaleDate = s.SaleDate,
                            CustomerId = s.CustomerId,
                            SalesTypeId = s.SalesTypeId,
                            SalesType = st.Name,
                            CustomerName = c.Name,
                            CompanyName = c.CompanyName ?? "",
                            Email = c.Email ?? "",
                            Mobile = c.Mobile,
                            Address = c.Address ?? "",
                            Vat = s.Vat,
                            NetAmount = s.NetAmount,
                            Discount = s.Discount,
                            TotalAmount = s.TotalAmount,
                            PaidAmount = s.PaidAmount,
                            DueAmount = s.DueAmount,
                            AcountTypeId = s.AcountTypeId,
                            AccountType = at.Type,
                            AccountId = s.AccountId,
                            AccountNumber = a.AccountNumber,
                            AccountHolderName = a.HolderName ?? "",
                            Note = s.Note,
                        }).Where(t=> t.Id == id).FirstOrDefaultAsync();
            return sale;
        }

        public async Task<IList<Sale>> GetSaleByCustomerId(Guid CustomerId)
        {
            var sale = await GetAsync(t => t.CustomerId == CustomerId, null);
            return sale;
        }
    }
}
