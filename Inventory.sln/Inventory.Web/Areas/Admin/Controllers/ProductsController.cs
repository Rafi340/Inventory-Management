using AutoMapper;
using Inventory.Application.Exceptions;
using Inventory.Application.Features.Products.Commands;
using Inventory.Application.Features.Products.Queries;
using Inventory.Domain;
using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;
using Inventory.Domain.Features.Prouduct.Queries;
using Inventory.Domain.Services;
using Inventory.Infrastructure;
using Inventory.Web.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Identity;
using System.Web;

namespace Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IUnitService _unitService;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        public ProductsController(ILogger<ProductsController> logger,IMapper mapper,
            IUnitService unitService, 
            ICategoryService categoryService,
            IProductService productService,
            IMediator mediator,
            IConfiguration configuration)
        {
            _logger = logger;
            _unitService = unitService;
            _categoryService = categoryService;
            _productService = productService;
            _mediator = mediator;
            _mapper = mapper;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ReportIndex()
        {
            var model = new ProductListModel
            {
                UnitList = _unitService.UnitDropdown(),
                CategoryList = _categoryService.CategoryDropdown()
            };
            return View(model);
        }
        public IActionResult Add()
        {
            var model = new AddProductModel
                { UnitList = _unitService.UnitDropdown() ,
                CategoryList = _categoryService.CategoryDropdown()} ;
            return View(model);
        }
        public IActionResult Update(Guid id)
        {
            var model = new UpdateProductModel
            {
                UnitList = _unitService.UnitDropdown(),
                CategoryList = _categoryService.CategoryDropdown()
            };
            var product = _productService.GetProduct(id);

            _mapper.Map(product, model);

            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProductUpdateCommand model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _mediator.Send(model);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product updated",
                        Type = ResponseTypes.Success
                    });
                    TempData["ShowToast"] = true;
                    return RedirectToAction("ReportIndex", "Products");
                }
                catch (DuplicateProductCodeExceptions de)
                {
                    ModelState.AddModelError("DuplicateProduct", de.Message);
                    TempData["ShowToast"] = true;
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = de.Message,
                        Type = ResponseTypes.Danger,
                    });

                    return RedirectToAction("Add", "Products");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to update Product");

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Failed to update Product",
                        Type = ResponseTypes.Danger
                    });
                }
            }
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ProductAddCommand model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(model);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product Added Sucessfully",
                        Type = ResponseTypes.Success,
                    });
                    TempData["ShowToast"] = true;
                    return RedirectToAction("ReportIndex", "Products");
                }
                return RedirectToAction("Add", "Products");
            }
            catch(DuplicateProductCodeExceptions de)
            {
                ModelState.AddModelError("DuplicateProduct", de.Message);
                TempData["ShowToast"] = true;
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = de.Message,
                    Type = ResponseTypes.Danger,
                });

                return RedirectToAction("Add", "Products");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Can't add Product");
                return RedirectToAction("Add", "Products");
            }
            
        }

        [HttpPost]
        public async Task<JsonResult> GetProducts([FromBody] GetProductQuery  model)
        {

            var (data, total, totalDisplay) = await _mediator.Send(model);
            string bucketUrl = _configuration["Aws:BucketUrl"];
            var products = new
            {
                recordsTotal = total,
                recordsFiltered = totalDisplay,
                data = (from record in data
                        select new string[]
                        {

                                $"<input type='checkbox' class='form-check-input' />",
                                $"<img src='{(string.IsNullOrEmpty(record.ImageUrl) ?  Url.Content("~/assets/img/avatars/16.png") : bucketUrl+HttpUtility.HtmlEncode(record.ImageUrl))}' class='img-fluid rounded'  height='50' width='50'/>",
                                $"<a href='/Admin/Product/ProductView/{record.Id}'>{HttpUtility.HtmlEncode(record.ProductCode)}</a>",
                                HttpUtility.HtmlEncode(record.Name),
                                HttpUtility.HtmlEncode(record.Unit.Name),
                                HttpUtility.HtmlEncode(record.UnitPrice.ToString()),
                                HttpUtility.HtmlEncode(record.MrpPrice.ToString()),
                                HttpUtility.HtmlEncode(record.WholeSalePrice.ToString()),
                                HttpUtility.HtmlEncode(record.Quantity.ToString()),
                                HttpUtility.HtmlEncode(record?.LowStock?.ToString()),
                                HttpUtility.HtmlEncode(record?.DamangeStock.ToString()),
                                record.Id.ToString()
                        }).ToArray()
            };
            return Json(products);
            // return Json(new { success = true, message = "Post received!" });
        }

        [HttpPost]
        public async Task<JsonResult> GetProductReport([FromBody] GetProductSpQuery model)
        {
            try
            {
                var searchDto = _mapper.Map<ProductSearchDto>(model.SearchItem);
                var (data, total, totalDisplay) = await _mediator.Send(model); ;
                string bucketUrl = _configuration["Aws:BucketUrl"];
                var products = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,
                    data = (from record in data
                            select new string[]
                            {
                                $"<input type='checkbox' class='form-check-input' {record.Id.ToString()} />",
                                $"<img src='{(string.IsNullOrEmpty(record.ImageUrl) ?  Url.Content("~/assets/img/avatars/16.png") : bucketUrl+HttpUtility.HtmlEncode(record.ImageUrl))}' class='img-fluid rounded'  height='50' width='50'/>",
                                HttpUtility.HtmlEncode(record.ProductCode),
                                HttpUtility.HtmlEncode(record.Name),
                                HttpUtility.HtmlEncode(record.UnitName),
                                HttpUtility.HtmlEncode(record.CategoryName),
                                HttpUtility.HtmlEncode(record.UnitPrice),
                                HttpUtility.HtmlEncode(record.MrpPrice ?? 0),
                                HttpUtility.HtmlEncode(record.WholeSalePrice ?? 0),
                                HttpUtility.HtmlEncode(record.Quantity),
                                HttpUtility.HtmlEncode(record.LowStock ?? 0),
                                HttpUtility.HtmlEncode(record.DamageStock ?? 0),
                                record.Id.ToString()
                            }).ToArray()
                };
                return Json(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem with author");
                return Json(DataTables.EmptyResult);
            }

        }

        [HttpPost]
        public  JsonResult GetproductsJsonDataAsync([FromBody] ProductListModel model)
        {
            try
            {
                var (data, total, totalDisplay) = _productService.GetProducts(model.PageIndex, model.PageSize,
                    model.FormatSortExpression("Name", "SKU", "Id"), model.Search);

                var products = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,
                    data = (from record in data
                            select new string[]
                            {
                                HttpUtility.HtmlEncode(record.Name),
                                HttpUtility.HtmlEncode(record.SKU),
                                record.Id.ToString()
                            }).ToArray()
                };

                return Json(products);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem in getting Productss");
                return Json(DataTables.EmptyResult);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid Id)
        {
            try
            {
                var product = new ProductDeleteCommand
                {
                    Id = Id
                };
                await _mediator.Send(product);
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Products Delete Sucessfully",
                    Type = ResponseTypes.Success,
                });
                return RedirectToAction("Index","Products");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to Delete Products");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Products Can't be deleted",
                    Type = ResponseTypes.Danger,
                });
            }
            return RedirectToAction("Index", "Products");
        }

        [HttpGet]
        public async Task<Product> GetProductById(Guid id)
        {
            var product = new GetProductByIdQuery{ Id = id};
            
            return await _mediator.Send(product);
        }

    }
}
