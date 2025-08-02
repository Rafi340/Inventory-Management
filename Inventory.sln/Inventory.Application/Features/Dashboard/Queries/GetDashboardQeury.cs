using Inventory.Domain.Features.Dashboard.Queries;
using Inventory.Domain.ViewModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Dashboard.Queries
{
    public class GetDashboardQeury : IRequest<DashboardViewModel> , IGetDashboardQuery
    {
    }
}
