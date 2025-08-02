using Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Sales.Queries
{
    public class GetSaleByIdQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : IRequestHandler<GetSaleByIdQuery, Sale>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public Task<Sale> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
        {
            var getSale = _applicationUnitOfWork.SaleRepository.GetById(request.Id);
            return Task.FromResult(getSale);
        }
    }
}
