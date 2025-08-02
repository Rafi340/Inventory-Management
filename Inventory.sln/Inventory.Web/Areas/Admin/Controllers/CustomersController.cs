using AutoMapper;
using Inventory.Application.Exceptions;
using Inventory.Application.Features.Categories.Queries;
using Inventory.Application.Features.Customers.Commands;
using Inventory.Application.Features.Customers.Queries;
using Inventory.Application.Features.Products.Commands;
using Inventory.Domain.Entities;
using Inventory.Infrastructure;
using Inventory.Web.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class CustomersController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CustomersController> _logger;
        private readonly IMapper _mapper;
        private readonly string _imagePath;
        private readonly IConfiguration _configuration;
        public CustomersController(IMediator mediator, ILogger<CustomersController> logger, IMapper mapper, IConfiguration configuration)
        {
            _mediator = mediator;
            _logger = logger;
            _mapper = mapper;
            _configuration = configuration;
            _imagePath = _configuration["Aws:BucketUrl"];
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CustomerView(Guid id)
        {

            var CustomerId = new GetCustomerWithInvoiceListQuery
            {
                Id = id
            };
            var customer = await _mediator.Send(CustomerId);
            if(!string.IsNullOrEmpty(customer.ImageUrl))
            {
               customer.ImageUrl = Path.Combine(_imagePath, customer.ImageUrl);
            }          

            return View(customer);
        }
        [HttpPost]
        public async Task<JsonResult> GetCustomers([FromBody] GetCustomerQuery model)
        {

            var (data, total, totalDisplay) = await _mediator.Send(model);
            var customers = new
            {
                recordsTotal = total,
                recordsFiltered = totalDisplay,
                data = (from record in data
                        select new string[]
                        {
                                $"<a href='/Admin/Customers/CustomerView/{record.Id}'>{HttpUtility.HtmlEncode(record.CustomerId)}</a>",
                                HttpUtility.HtmlEncode(record.Name),
                                HttpUtility.HtmlEncode(record.Email ?? string.Empty),
                                HttpUtility.HtmlEncode(record.Mobile),
                                HttpUtility.HtmlEncode(record.Address ?? string.Empty),
                                HttpUtility.HtmlEncode(record.OpeningBalance),
                                record.Id.ToString()
                        }).ToArray()
            };
            return Json(customers);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CustomerAddCommand model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    await _mediator.Send(model);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Customer Added Sucessfully",
                        Type = ResponseTypes.Success,
                    });
                    TempData["ShowToast"] = true;
                    return RedirectToAction("Index", "Customers");
                }
                return RedirectToAction("Index", "Customers");

            }catch(DuplicateCustomerIdExceptions de)
            {
                ModelState.AddModelError("DuplicateAuthor",de.Message);
                TempData["ShowToast"] = true;
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = de.Message,
                    Type = ResponseTypes.Danger,
                });
                return RedirectToAction("Index", "Customers");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Can't add Customer");
                return RedirectToAction("Index", "Customers");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CustomerUpdateCommand model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _mediator.Send(model);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Customer updated",
                        Type = ResponseTypes.Success
                    });
                    TempData["ShowToast"] = true;
                    return RedirectToAction("Index", "Customers");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to update Customer");
                    TempData["ShowToast"] = true;
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Failed to update Product",
                        Type = ResponseTypes.Danger
                    });
                }
            }
            return RedirectToAction("Index", "Customers");
        }

        public async Task<IActionResult> EditPartial(Guid id)
        {
            var CustomerId = new GetCustomerByIdQuery
            {
                CustomerId = id
            };
            var customer = await _mediator.Send(CustomerId);
            var map  = _mapper.Map<CustomerUpdateModel>(customer);
            map.Image = Path.Combine(_imagePath,customer?.ImageUrl ?? "");
            return PartialView("_CustomerUpdateForm", map);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid Id)
        {
            try
            {
                var product = new CustomerDeleteCommand
                {
                    Id = Id
                };
                await _mediator.Send(product);
                TempData["ShowToast"] = true;
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Customer Delete Sucessfully",
                    Type = ResponseTypes.Success,
                });
                return RedirectToAction("Index", "Customers");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to Delete Customer");
                TempData["ShowToast"] = true;
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Products Can't be deleted",
                    Type = ResponseTypes.Danger,
                });
            }
            return RedirectToAction("Index", "Customers");
        }

    }
}
