using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Sales.Commands
{
    public class SaleAddPaymentCommandHandler(IApplicationUnitOfWork applicationUnitOfWork) : IRequestHandler<SaleAddPaymentCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task Handle(SaleAddPaymentCommand request, CancellationToken cancellationToken)
        {
            var getSale = await _applicationUnitOfWork.SaleRepository.GetByIdAsync(request.Id);
            if (getSale != null) 
            {
                getSale.PaidAmount += request.PaidAmount;
                getSale.DueAmount -= request.PaidAmount;
                if (getSale.PaidAmount >= getSale.TotalAmount)
                {
                    getSale.Status = 1;
                }
                else if (getSale.PaidAmount > 0)
                {
                    getSale.Status = 2;
                }
                

                await _applicationUnitOfWork.SaleRepository.EditAsync(getSale);
                await _applicationUnitOfWork.SaveAsync();
            }
        }
    }
}
