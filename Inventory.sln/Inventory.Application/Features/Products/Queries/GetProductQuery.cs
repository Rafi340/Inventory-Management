using Inventory.Domain;
using Inventory.Domain.Entities;
using Inventory.Domain.Features.Prouduct.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Products.Queries
{
    public class GetProductQuery : DataTables , IRequest<(IList<Product>, int, int)> , IGetProductQuery
    {

    }
}
