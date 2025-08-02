using Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.AccountTypes.Queries
{
    public class GetAccountTypeQuery : IRequest<IList<AccountType>>
    {
    }
}
