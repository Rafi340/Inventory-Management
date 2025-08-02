using Inventory.Domain;
using Inventory.Domain.Dtos;
using Inventory.Domain.Features.Sale;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Sales.Queries
{
    public class GetSaleSPQuery : SaleSearchDto, IRequest<(IList<SaleWithCustomerAccountDto> , int, int)> , IGetSalesQuery
    {
           
    }
}
