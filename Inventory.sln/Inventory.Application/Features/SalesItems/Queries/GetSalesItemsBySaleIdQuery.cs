using Inventory.Domain.ViewModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.SalesItems.Queries
{
    public class GetSalesItemsBySaleIdQuery : IRequest<IList<SaleItemsViewModel>>
    {
        public Guid SaleId { get; set; }
    }
}
