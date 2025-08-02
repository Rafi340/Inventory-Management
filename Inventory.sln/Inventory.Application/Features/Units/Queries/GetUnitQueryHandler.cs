using Inventory.Application.Features.Products.Queries;
using Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Units.Queries
{
    public class GetUnitQueryHandler : IRequestHandler<GetUnitQuery, (IList<Domain.Entities.Unit>, int, int)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public GetUnitQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public Task<(IList<Domain.Entities.Unit>, int, int)> Handle(GetUnitQuery request, CancellationToken cancellationToken)
        {
            return _applicationUnitOfWork.UnitRepository.GetPagedUnitAsync(request);
        }
    }
}
