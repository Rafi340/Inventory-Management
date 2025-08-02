using Inventory.Application.Features.Products.Commands;
using Inventory.Infrastructure;
using Inventory.Application.Features.Units.Commands;
using Inventory.Application.Features.Units.Queries;
using Inventory.Web.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Inventory.Domain.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class UnitsController : Controller
    {

        private readonly IMediator _mediator;
        private readonly ILogger<UnitsController> _logger;
        private readonly IUnitService _unitService;
        private readonly IMapper _mapper;

        public UnitsController(IMediator mediator, ILogger<UnitsController> logger, 
            IUnitService unitService,
            IMapper mapper)
        {
            _mediator = mediator;
            _logger = logger;
            _unitService = unitService;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Add()
        {
            var model = new AddUnitModel();
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(UnitAddCommand model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(model);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Unit Added Sucessfully",
                        Type = ResponseTypes.Success,
                    });
                    return RedirectToAction("Index", "Units");
                }
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Can't add Unit");
                return View();
            }

        }

        [HttpPost]
        public async Task<JsonResult> GetUnits([FromBody]GetUnitQuery model)
        {

            var (data, total, totalDisplay) = await _mediator.Send(model);
            var units = new
            {
                recordsTotal = total,
                recordsFiltered = totalDisplay,
                data = (from record in data
                        select new string[]
                        {
                                HttpUtility.HtmlEncode(record.Name),
                                record.Id.ToString()
                        }).ToArray()
            };
            return Json(units);
        }

        public IActionResult Update(int id)
        {
            var model = new UpdateUnitModel();
            var unit = _unitService.GetUnit(id);

            _mapper.Map(unit, model);

            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UnitUpdateCommand model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _mediator.Send(model);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Unit updated",
                        Type = ResponseTypes.Success
                    });

                    return RedirectToAction("Index", "Units");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to update Unit");

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Failed to update Unit",
                        Type = ResponseTypes.Danger
                    });
                }
            }
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                var unit = new UnitDeleteCommand
                {
                    Id = Id
                };
                await _mediator.Send(unit);
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Unit Delete Sucessfully",
                    Type = ResponseTypes.Success,
                });
                return RedirectToAction("Index", "Units");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to Delete Unit");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Unit Can't be deleted",
                    Type = ResponseTypes.Danger,
                });
            }
            return RedirectToAction("Index", "Units");
        }
    }
}
