using Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Customers.Queries
{
    public class GetCustomerDropdownQueryHandler(
        IApplicationUnitOfWork applicationUnitOfWork
        ) : IRequestHandler<GetCustomerDropdownQuery, IList<Customer>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<IList<Customer>> Handle(GetCustomerDropdownQuery request, CancellationToken cancellationToken)
        {
           return await _applicationUnitOfWork.CustomerRepository.GetAllAsync();
        }
    }
}
