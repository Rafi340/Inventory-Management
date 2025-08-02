using Inventory.Domain.Dtos;
using Inventory.Domain.ViewModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.BalanceTransfers.Queries
{
    public class GetBalanceTransferSPQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : IRequestHandler<GetBalanceTransferSPQuery, (IList<BalanceTransferViewModel>, int, int)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<(IList<BalanceTransferViewModel>, int, int)> Handle(GetBalanceTransferSPQuery request, CancellationToken cancellationToken)
        {
            string procedureName = "GetBalanceTransfer";
            var result = await _applicationUnitOfWork.SqlUtility
                .QueryWithStoredProcedureAsync<BalanceTransferViewModel>(
                procedureName,
                new Dictionary<string, object?>
                {
                    { "PageIndex", request.PageIndex },
                    { "PageSize", request.PageSize },
                    { "OrderBy", request.FormatSortExpression(["SenderAcountType", "SenderAccountNumber", "ReceiverAcountType", "ReceiverAccountNumber", "TransferAmount", "TransferTime", "Note"]) },
                    { "SearchText", string.IsNullOrEmpty(request.Search.Value) ? null :request.Search.Value }
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
