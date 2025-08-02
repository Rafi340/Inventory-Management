using AutoMapper;
using Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Sales.Commands
{
    public class SaleUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork,
        IMapper mapper
        ) : IRequestHandler<SaleUpdateCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task Handle(SaleUpdateCommand request, CancellationToken cancellationToken)
        {
            if (!_applicationUnitOfWork.SaleRepository.IsInvoiceDuplicate(request.InvoiceNo , request.Id))
            {

                var listOfOldItems = await _applicationUnitOfWork.SaleItemsRepository.GetItemsBySaleId(request.Id);

            foreach (var newItem in request.SalesItems) 
            {
                var oldProduct = listOfOldItems.Where(t=> t.ProductId == newItem.ProductId).FirstOrDefault();
                if (oldProduct != null) 
                {
                  if(oldProduct.Quantity < newItem.Quantity)
                  {
                      var quantity = newItem.Quantity - oldProduct.Quantity;
                      var getOriginalProduct = await _applicationUnitOfWork.ProductRepository.GetByIdAsync(newItem.ProductId);
                      getOriginalProduct.Quantity -= quantity;
                      await _applicationUnitOfWork.ProductRepository.EditAsync(getOriginalProduct);
                  }
                  if (oldProduct.Quantity > newItem.Quantity)
                  {
                        var quantity = oldProduct.Quantity - newItem.Quantity;
                        var gerOriginalProduct = await _applicationUnitOfWork.ProductRepository.GetByIdAsync(newItem.ProductId);
                        gerOriginalProduct.Quantity += quantity;
                        await _applicationUnitOfWork.ProductRepository.EditAsync(gerOriginalProduct);
                    }
                }
            }
            foreach(var oldItem in listOfOldItems)
            {
                var newProduct = request.SalesItems.Where(t => t.ProductId == oldItem.ProductId).FirstOrDefault();
                if(newProduct == null)
                {
                    var getProduct = await _applicationUnitOfWork.ProductRepository.GetByIdAsync(oldItem.ProductId);
                    getProduct.Quantity += oldItem.Quantity;
                    await _applicationUnitOfWork.ProductRepository.EditAsync(getProduct);
                }
            }


            await _applicationUnitOfWork.SaleItemsRepository.RemoveAsync(t => t.SalesId == request.Id);

            var map = _mapper.Map<Sale>(request);
            var netAmount = request.SalesItems.Sum(t => t.Quantity * t.UnitPrice);
            var vatCalulate = (netAmount * map.Vat) / 100;
            map.NetAmount = netAmount + vatCalulate;
            map.TotalAmount = map.NetAmount - map.Discount;
            map.DueAmount = map.TotalAmount - map.PaidAmount;

            if (map.PaidAmount >= map.TotalAmount)
            {
                map.Status = 1;
            }
            else if (map.PaidAmount > 0)
            {
                map.Status = 2;
            }
            if (map.DueAmount == map.TotalAmount)
            {
                map.Status = 0;
            }

            await _applicationUnitOfWork.SaleRepository.EditAsync(map);
            await _applicationUnitOfWork.SaveAsync();

            }
        }
    }
}
