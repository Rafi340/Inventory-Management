using Inventory.Domain.ViewModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.SalesItems.Queries
{
    public class GetSalesItemsBySaleIdQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : IRequestHandler<GetSalesItemsBySaleIdQuery, IList<SaleItemsViewModel>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<IList<SaleItemsViewModel>> Handle(GetSalesItemsBySaleIdQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.SaleItemsRepository.GetSaleItemsBySaleId(request.SaleId);
        }
    }
}
