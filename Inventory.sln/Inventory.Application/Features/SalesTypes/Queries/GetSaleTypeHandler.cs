using Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.SalesTypes.Queries
{
    public class GetSaleTypeHandler(IApplicationUnitOfWork applicationUnitOfWork) : IRequestHandler<GetSalesTypeQuery, IList<SalesType>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<IList<SalesType>> Handle(GetSalesTypeQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.SalesTypeRepository.GetAllAsync();
        }
    }
}
