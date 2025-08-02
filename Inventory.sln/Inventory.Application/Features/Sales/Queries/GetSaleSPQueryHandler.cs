using Inventory.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Sales.Queries
{
    public class GetSaleSPQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : IRequestHandler<GetSaleSPQuery, (IList<SaleWithCustomerAccountDto>, int, int)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<(IList<SaleWithCustomerAccountDto>, int, int)> Handle(GetSaleSPQuery request, CancellationToken cancellationToken)
        {
            string procedureName = "GetSales";
            var result = await _applicationUnitOfWork.SqlUtility
                .QueryWithStoredProcedureAsync<SaleWithCustomerAccountDto>(
                procedureName,
                new Dictionary<string, object?>
                {
                    { "PageIndex", request.PageIndex },
                    { "PageSize", request.PageSize },
                    { "OrderBy", request.FormatSortExpression(["InvoiceNo","SaleDate","CustomerName","SalesTypeName","Vat","NetAmount","Discount","TotalAmount","PaidAmount","DueAmount"]) },
                    { "InvoiceNo", string.IsNullOrEmpty(request.InvoiceNo) ? null : request.InvoiceNo },
                    { "CustomerId", string.IsNullOrEmpty(request.CustomerId.ToString()) ? null : request.CustomerId },
                    { "FromDate", request.FromDate },
                    { "ToDate", request.ToDate },
                },
                new Dictionary<string, Type>
                {
                    { "Total", typeof(int) },
                    { "TotalDisplay", typeof(int) },
                }
                );
            return (result.result, (int)result.outValues["Total"], (int)result.outValues["TotalDisplay"]);
        }
    }
}
