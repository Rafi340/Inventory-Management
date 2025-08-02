using Inventory.Application.Features.Dashboard.Queries;
using Inventory.Domain.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Web.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class DashboardController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;
        public async Task<IActionResult> Index()
        {
            var query = new GetDashboardQeury();
            DashboardViewModel model = await _mediator.Send(query);
            return View(model);
        }
    }
}
