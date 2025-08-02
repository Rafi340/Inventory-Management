using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Sales.Commands
{
    public class SaleDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork) : IRequestHandler<SaleDeleteCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task Handle(SaleDeleteCommand request, CancellationToken cancellationToken)
        {
            var getSaleAndItems = await _applicationUnitOfWork.SaleRepository.GetByIdAsync(request.Id);
            var getItems = await _applicationUnitOfWork.SaleItemsRepository.GetItemsBySaleId(request.Id);
            foreach (var item in getItems) { 
                var getProduct = await _applicationUnitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
                getProduct.Quantity += item.Quantity;
            }
            await _applicationUnitOfWork.SaleItemsRepository.RemoveAsync(t=> t.SalesId == request.Id);
            await _applicationUnitOfWork.SaleRepository.RemoveAsync(request.Id);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
