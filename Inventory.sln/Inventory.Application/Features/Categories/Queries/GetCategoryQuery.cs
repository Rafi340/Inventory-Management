using Inventory.Domain.Features.Units.Queries;
using Inventory.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.Domain.Entities;
using Inventory.Domain.Features.Categories.Queries;

namespace Inventory.Application.Features.Categories.Queries
{
    public class GetCategoryQuery : DataTables, IRequest<(IList<Category>, int, int)>, IGetCategoryQuery
    {
    }
}
