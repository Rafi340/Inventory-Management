using Inventory.Domain.ViewModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Invoice.Queries
{
    public class GetInvoiceQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : IRequestHandler<GetInvoiceQuery, SaleViewModel>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<SaleViewModel> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
        {
            var getSale = await _applicationUnitOfWork.SaleRepository.GetSaleById(request.Id);
            var getSaleItems = await _applicationUnitOfWork.SaleItemsRepository.GetSaleItemsBySaleId(request.Id);
            getSale.SalesItems = getSaleItems.ToList();
            return getSale;
        }
    }
}
