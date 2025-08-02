using Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Products.Queries
{
    public class GetProductDropdownQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : IRequestHandler<GetProductDropdownQuery, IList<Product>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<IList<Product>> Handle(GetProductDropdownQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.ProductRepository.GetAllAsync();
        }
    }
}
