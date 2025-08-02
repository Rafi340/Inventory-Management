using Inventory.Domain;
using Inventory.Domain.Entities;
using Inventory.Domain.Features.Customers.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Customers.Queries
{
    public class GetCustomerQuery : DataTables, IRequest<(IList<Customer>, int ,int)> , IGetCustomerQuery
    {
    }
}
