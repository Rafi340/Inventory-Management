using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Units.Commands
{
    public class UnitDeleteCommandHandler : IRequestHandler<UnitDeleteCommand>
    {
        private IApplicationUnitOfWork _applicationUnitOfWork;
        public UnitDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task Handle(UnitDeleteCommand request, CancellationToken cancellationToken)
        {
            await _applicationUnitOfWork.UnitRepository.RemoveAsync(request.Id);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
