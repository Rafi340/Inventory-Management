using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Units.Commands
{
    public class UnitUpdateCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
