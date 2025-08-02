using AutoMapper;
using Inventory.Application.Features.Units.Commands;
using Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Categories.Commands
{
    public class CategoryUpdateCommandHandler : IRequestHandler<CategoryUpdateCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;
        public CategoryUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
        }
        public async Task Handle(CategoryUpdateCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Category>(request);
            await _applicationUnitOfWork.CategoryRepository.EditAsync(category);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
