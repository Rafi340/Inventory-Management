using Inventory.Application.Features.Products.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Categories.Commands
{
    public class CategoryDeleteCommandHandler : IRequestHandler<CategoryDeleteCommand>
    {
        private IApplicationUnitOfWork _applicationUnitOfWork;
        public CategoryDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task Handle(CategoryDeleteCommand request, CancellationToken cancellationToken)
        {
            await _applicationUnitOfWork.CategoryRepository.RemoveAsync(request.Id);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
