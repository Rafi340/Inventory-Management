using AutoMapper;
using Inventory.Application.Features.Categories.Commands;
using Inventory.Application.Features.Categories.Queries;
using Inventory.Domain.Services;
using Inventory.Web.Areas.Admin.Models;
using Inventory.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Inventory.Application.Features.Units.Commands;

namespace Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CategoriesController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoriesController(IMediator mediator, ILogger<CategoriesController> logger,
            ICategoryService Categoireservice,
            IMapper mapper)
        {
            _mediator = mediator;
            _logger = logger;
            _categoryService = Categoireservice;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Add()
        {
            var model = new AddCategoryModel();
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CategoryAddCommand model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(model);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Category Added Sucessfully",
                        Type = ResponseTypes.Success,
                    });
                    return RedirectToAction("Index", "Categories");
                }
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Can't add Category");
                return View();
            }

        }

        [HttpPost]
        public async Task<JsonResult> GetCategories([FromBody] GetCategoryQuery model)
        {

            var (data, total, totalDisplay) = await _mediator.Send(model);
            var Categoires = new
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
            return Json(Categoires);
        }

        public IActionResult Update(int id)
        {
            var model = new UpdateCategoryModel();
            var cateogry = _categoryService.GetCategory(id);

            _mapper.Map(cateogry, model);

            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CategoryUpdateCommand model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _mediator.Send(model);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Category updated",
                        Type = ResponseTypes.Success
                    });

                    return RedirectToAction("Index", "Categories");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to update Category");

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Failed to update Category",
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
                var category = new CategoryDeleteCommand
                {
                    Id = Id
                };
                await _mediator.Send(category);
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "category Delete Sucessfully",
                    Type = ResponseTypes.Success,
                });
                return RedirectToAction("Index", "Categories");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to Delete category");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "category Can't be deleted",
                    Type = ResponseTypes.Danger,
                });
            }
            return RedirectToAction("Index", "Categories");
        }
    }
}
