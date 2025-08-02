using Inventory.Domain.Dtos;
using Inventory.Domain.ViewModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Users.Queries
{
    public class GetUserSPQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : IRequestHandler<GetUserSPQuery, (IList<UserViewModel>, int, int)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<(IList<UserViewModel>, int, int)> Handle(GetUserSPQuery request, CancellationToken cancellationToken)
        {
            string procedureName = "GetUsers";
            var result = await _applicationUnitOfWork.SqlUtility
                .QueryWithStoredProcedureAsync<UserViewModel>(
                procedureName,
                new Dictionary<string, object?>
                {
                    { "PageIndex", request.PageIndex },
                    { "PageSize", request.PageSize },
                    { "OrderBy", request.FormatSortExpression(["UserName", "FirstName", "LastName", "RoleName"]) },
                    { "SearchText", string.IsNullOrEmpty(request.Search.Value) ? null : request.Search.Value.ToUpper() }
                },
                new Dictionary<string, Type>
                {
                    { "Total", typeof(int) },
                    { "TotalDisplay", typeof(int) },
                }
                );
            return (result.result, (int)result.outValues["Total"], (int)result.outValues["TotalDisplay"]);
        }
    }
}
