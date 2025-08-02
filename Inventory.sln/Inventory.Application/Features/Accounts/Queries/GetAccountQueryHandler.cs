using Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Accounts.Queries
{
    public class GetAccountQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : IRequestHandler<GetAccountQuery, IList<Account>>
    {
        public readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<IList<Account>> Handle(GetAccountQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.AccountRepository.GetAllAsync();
        }
    }
}
