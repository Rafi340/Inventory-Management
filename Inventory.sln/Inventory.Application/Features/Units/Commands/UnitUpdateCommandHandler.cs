using AutoMapper;
using Inventory.Application.Features.Products.Commands;
using Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Units.Commands
{
    public class UnitUpdateCommandHandler : IRequestHandler<UnitUpdateCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;
        public UnitUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
        }
        public async Task Handle(UnitUpdateCommand request, CancellationToken cancellationToken)
        {
            var unit = _mapper.Map<Domain.Entities.Unit>(request);
            await _applicationUnitOfWork.UnitRepository.EditAsync(unit);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
