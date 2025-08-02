using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Products.Commands
{
    public class ProductDeleteCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
