using AutoMapper;
using Inventory.Domain.ViewModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Customers.Queries
{
    public class GetCustomerWithInvoiceListQueryHandler(IApplicationUnitOfWork applicationUnitOfWork,
        IMapper mapper
        ) : IRequestHandler<GetCustomerWithInvoiceListQuery, CustomerView>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<CustomerView> Handle(GetCustomerWithInvoiceListQuery request, CancellationToken cancellationToken)
        {
            var getCustomer = await _applicationUnitOfWork.CustomerRepository.GetByIdAsync(request.Id);
            var map = _mapper.Map<CustomerView>(getCustomer);
            var getInvoiceListOfCustomer = await _applicationUnitOfWork.SaleRepository.GetSaleByCustomerId(request.Id);
            map.SaleViews = getInvoiceListOfCustomer.ToList();
            return map;
        }
    }
}
