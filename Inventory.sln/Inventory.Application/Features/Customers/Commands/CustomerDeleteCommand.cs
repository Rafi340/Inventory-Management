using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Customers.Commands
{
    public class CustomerDeleteCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
