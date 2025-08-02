
using Inventory.Domain.Features.Prouduct.Queries;
using Inventory.Domain.Entities;
using Inventory.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.Domain.Features.Units.Queries;

namespace Inventory.Application.Features.Units.Queries
{
    public class GetUnitQuery : DataTables, IRequest<(IList<Domain.Entities.Unit>, int, int)>, IGetUnitQuery
    {
    }
}
