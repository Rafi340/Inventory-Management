using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Categories.Commands
{
    public class CategoryDeleteCommand : IRequest
    {
        public int Id { get; set; }
    }
}
