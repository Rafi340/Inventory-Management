using Inventory.Domain.Entities;
using Inventory.Domain.Entities.ViewModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using System.Reflection;

namespace Inventory.Application.Features.Products.Queries
{
    public class GetProductSpQueryHandler : IRequestHandler<GetProductSpQuery, (IList<ProductViewModel>, int, int)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public GetProductSpQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public Task<(IList<ProductViewModel>, int, int)> Handle(GetProductSpQuery request, CancellationToken cancellationToken)
        {
           return _applicationUnitOfWork.GetProductSP(request.PageIndex, request.PageSize, 
               request.FormatSortExpression("ProductCode", "Name", "UnitName", "CategoryName", "UnitPrice", "MrpPrice", "WholeSalePrice", "Quantity", "LowStock", "DamageStock"), request.SearchItem);
        }
    }
}
