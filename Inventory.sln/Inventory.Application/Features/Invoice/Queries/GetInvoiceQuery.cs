using Inventory.Domain.ViewModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Invoice.Queries
{
    public class GetInvoiceQuery : IRequest<SaleViewModel>
    {
        public Guid Id { get; set; }
    }
}
