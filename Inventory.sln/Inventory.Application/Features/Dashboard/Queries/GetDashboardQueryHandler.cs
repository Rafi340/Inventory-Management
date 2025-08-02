using Inventory.Domain.ViewModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Dashboard.Queries
{
    public class GetDashboardQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : IRequestHandler<GetDashboardQeury, DashboardViewModel>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<DashboardViewModel> Handle(GetDashboardQeury request, CancellationToken cancellationToken)
        {
            var getTotalCustomer = _applicationUnitOfWork.CustomerRepository.GetCount();
            var getProductTotalProduct = _applicationUnitOfWork.ProductRepository.GetCount();
            var getTotalSellPrice =  _applicationUnitOfWork.SaleRepository.GetAll().Sum(t=> t.TotalAmount);
            var model = new DashboardViewModel
            {
                TotalCustomer = getTotalCustomer,
                TotalProduct = getProductTotalProduct,
                TotalSellPrice = getTotalSellPrice,
            };
            return model;

        }
    }
}
