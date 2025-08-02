using Inventory.Application.Features.Customers.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Customers.Commands
{
    public class CustomerDeleteCommandHandler : IRequestHandler<CustomerDeleteCommand>
    {
            private IApplicationUnitOfWork _applicationUnitOfWork;
            public CustomerDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork)
            {
                _applicationUnitOfWork = applicationUnitOfWork;
            }
            public async Task Handle(CustomerDeleteCommand request, CancellationToken cancellationToken)
            {
                await _applicationUnitOfWork.CustomerRepository.RemoveAsync(request.Id);
                await _applicationUnitOfWork.SaveAsync();
            }
        
    }
}
