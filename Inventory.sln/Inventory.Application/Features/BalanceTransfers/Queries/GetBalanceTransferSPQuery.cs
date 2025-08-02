using Inventory.Domain;
using Inventory.Domain.Features.BalanceTransfers.Queries;
using Inventory.Domain.ViewModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.BalanceTransfers.Queries
{
    public class GetBalanceTransferSPQuery : DataTables , IRequest<(IList<BalanceTransferViewModel>, int, int)>, IGetBalanceTransfersQuery
    {
    }
}
