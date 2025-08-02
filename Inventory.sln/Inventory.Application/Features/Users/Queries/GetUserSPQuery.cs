using Inventory.Domain;
using MediatR;
using Inventory.Domain.ViewModel;
using Inventory.Domain.Features.Users.Queries;

namespace Inventory.Application.Features.Users.Queries
{
    public class GetUserSPQuery : DataTables, IRequest<(IList<UserViewModel>, int, int)>, IGetUserSPQuery
    {
    }
}
