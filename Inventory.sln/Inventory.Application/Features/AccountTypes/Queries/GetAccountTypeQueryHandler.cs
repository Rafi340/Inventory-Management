using Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.AccountTypes.Queries
{
    public class GetAccountTypeQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : IRequestHandler<GetAccountTypeQuery, IList<AccountType>>
    {
        public readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<IList<AccountType>> Handle(GetAccountTypeQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.AccountTypeRepository.GetAllAsync();
        }
    }
}
