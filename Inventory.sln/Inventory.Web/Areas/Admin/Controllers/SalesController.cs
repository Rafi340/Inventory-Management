using AutoMapper;
using Inventory.Application.Exceptions;
using Inventory.Application.Features.Accounts.Queries;
using Inventory.Application.Features.AccountTypes.Queries;
using Inventory.Application.Features.Customers.Queries;
using Inventory.Application.Features.Invoice.Queries;
using Inventory.Application.Features.Products.Queries;
using Inventory.Application.Features.Sales.Commands;
using Inventory.Application.Features.Sales.Queries;
using Inventory.Application.Features.SalesItems.Queries;
using Inventory.Application.Features.SalesTypes.Queries;
using Inventory.Application.Features.Units.Commands;
using Inventory.Domain;
using Inventory.Domain.Entities;
using Inventory.Domain.ViewModel;
using Inventory.Infrastructure;
using Inventory.Web.Areas.Admin.Models;
using Inventory.Web.Areas.Admin.Models.Sales;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using System.Web;

namespace Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class SalesController(ILogger<SalesController> logger, IMediator mediator, IMapper mapper) : Controller
    {
        private readonly ILogger<SalesController> _logger = logger;
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        
        public async Task<IActionResult> Index()
        {
            var searchModel = new SaleSearchModel();
            var queryCustomer = new GetCustomerDropdownQuery();
            searchModel.CustomerList = new SelectList(await _mediator.Send(queryCustomer), "Id", "Name");
            return View(searchModel);
        }

        public async Task<IActionResult> Add()
        {
            var model = new SalesAddModel();
            var queryCustomer = new GetCustomerDropdownQuery();
            model.CustomerList = new SelectList(await _mediator.Send(queryCustomer), "Id" , "Name");
            var salesType = new GetSalesTypeQuery();
            model.SalesTypeList = new SelectList(await _mediator.Send(salesType), "Id" , "Name");
            var productQuery = new GetProductDropdownQuery();
            model.ProductList = new SelectList(await _mediator.Send(productQuery), "Id" , "Name");
            var accontType = new GetAccountTypeQuery();
            model.AcountTypeList = new SelectList(await _mediator.Send(accontType), "Id" , "Type");
            var accont = new GetAccountQuery();
            var accountList =await _mediator.Send(accont);
            model.AccountList = new SelectList(accountList.Where(t=> t.AccountTypeId == 1), "Id", "AccountNumber");
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(SaleAddCommand model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                        await _mediator.Send(model);
                        TempData.Put("ResponseMessage", new ResponseModel
                        {
                            Message = "Sales Added Sucessfully",
                            Type = ResponseTypes.Success,
                        });
                        TempData["ShowToast"] = true;
                        return RedirectToAction("Index", "Sales");
                   
                    
                }
                return RedirectToAction("Add", "Sales");
            }
            catch (DuplicateSaleExceptions de)
            {
                ModelState.AddModelError("DuplicateInvoice", de.Message);
                TempData["ShowToast"] = true;
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = de.Message,
                    Type = ResponseTypes.Danger,
                });

                return RedirectToAction("Add", "Sales");
            }
            catch(EmtyItemExceptions de)
            {
                ModelState.AddModelError("ItemCan'tBeEmpty", de.Message);
                TempData["ShowToast"] = true;
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = de.Message,
                    Type = ResponseTypes.Danger,
                });

                return RedirectToAction("Add", "Sales");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Can't add Sales");
                return RedirectToAction("Add", "Sales");
            }
        }

        public async Task<IActionResult> Update(Guid Id)
        {
            var getByIdQuery = new GetSaleByIdQuery{ Id = Id };
            var getById = await _mediator.Send(getByIdQuery);

            var getSaleItemsQuery = new GetSalesItemsBySaleIdQuery { SaleId = Id };
            var getSaleItems = await _mediator.Send(getSaleItemsQuery);
            var model = _mapper.Map<SalesUpdateModel>(getById);
            model.SalesItems = getSaleItems.ToList();


            var queryCustomer = new GetCustomerDropdownQuery();
            model.CustomerList = new SelectList(await _mediator.Send(queryCustomer), "Id", "Name");
            var salesType = new GetSalesTypeQuery();
            model.SalesTypeList = new SelectList(await _mediator.Send(salesType), "Id", "Name");
            var productQuery = new GetProductDropdownQuery();
            model.ProductList = new SelectList(await _mediator.Send(productQuery), "Id", "Name");
            var accontType = new GetAccountTypeQuery();
            model.AcountTypeList = new SelectList(await _mediator.Send(accontType), "Id", "Type");
            var accont = new GetAccountQuery();
            var accountList = await _mediator.Send(accont);
            model.AccountList = new SelectList(accountList.Where(t => t.AccountTypeId == 1), "Id", "AccountNumber");
            return View(model);
        }

        public async Task<IActionResult> Invoice(Guid Id)
        {
            var getByIdQuery = new GetInvoiceQuery { Id = Id };
            var getById = await _mediator.Send(getByIdQuery);

            return View(getById);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(SaleUpdateCommand model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _mediator.Send(model);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Sale updated",
                        Type = ResponseTypes.Success
                    });
                    TempData["ShowToast"] = true;
                    return RedirectToAction("Index", "Sales");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to update");
                    TempData["ShowToast"] = true;
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Failed to update",
                        Type = ResponseTypes.Danger
                    });
                }

            }
            return RedirectToAction("Update", "Sales");

        }

        [HttpGet]
        public async Task<SelectList> GetAccountsByType(int accountTypeId)
        {
            var accontQuery = new GetAccountQuery();
            var account = await _mediator.Send(accontQuery);
            var filter = new SelectList(account.Where(t => t.AccountTypeId == accountTypeId), "Id", "AccountNumber");
            return filter;
        }

        [HttpPost]
        public async Task<JsonResult> GetSaleSp([FromBody] GetSaleSPQuery saleQuery)
        {
            try
            {
                var (data, total, totalDisplay) = await _mediator.Send(saleQuery);
                var sale = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,
                    data = (from record in data
                            select new string[]
                            {
                                $"<a href='/admin/Sales/Invoice/{record.Id}'>{HttpUtility.HtmlEncode(record.InvoiceNo.ToString())}</a>",
                                HttpUtility.HtmlEncode(record.SaleDate.ToShortDateString()),
                                HttpUtility.HtmlEncode(record.CustomerName.ToString()),
                                HttpUtility.HtmlEncode(record.SalesTypeName.ToString()),
                                HttpUtility.HtmlEncode(record.Vat.ToString()),
                                HttpUtility.HtmlEncode(record.NetAmount.ToString()),
                                HttpUtility.HtmlEncode(record.Discount.ToString()),
                                HttpUtility.HtmlEncode(record.TotalAmount.ToString()),
                                HttpUtility.HtmlEncode(record.PaidAmount.ToString()),
                                HttpUtility.HtmlEncode(record.DueAmount.ToString()),
                                record.StatusHtml,
                                record.Id.ToString(),
                                record.Status.ToString()
                            }
                            ).ToArray()
                };
                return Json(sale);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "There was a problem in getting Sales");
                return Json(DataTables.EmptyResult);
            }
        }


        public async Task<IActionResult> AddPaymentPartial(Guid id)
        {
            var saleQuery = new GetSaleByIdQuery
            {
                Id = id
            };
            var sale = await _mediator.Send(saleQuery);

            var map = new PaymentAddModel();
            map.DueAmount = sale.DueAmount;
            var accontType = new GetAccountTypeQuery();
            map.AcountTypeList = new SelectList(await _mediator.Send(accontType), "Id", "Type");
            var accont = new GetAccountQuery();
            var accountList = await _mediator.Send(accont);
            map.AccountList = new SelectList(accountList.Where(t => t.AccountTypeId == 1), "Id", "AccountNumber");
            return PartialView("_PaymentPartial", map);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPayment(SaleAddPaymentCommand model)
        {
            if (ModelState.IsValid) 
            {
                try
                {
                    await _mediator.Send(model);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Payment Complete",
                        Type = ResponseTypes.Success
                    });
                    TempData["ShowToast"] = true;
                    return RedirectToAction("Index", "Sales");
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "Failed to Payment");
                    TempData["ShowToast"] = true;
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Failed to Payment",
                        Type = ResponseTypes.Danger
                    });
                }

            }
            return RedirectToAction("AddPaymentPartial", "Sales", new { id = model.Id });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid Id)
        {
            try
            {
                var saleDeleteQuery = new SaleDeleteCommand
                {
                    Id = Id
                };
                await _mediator.Send(saleDeleteQuery);
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Sale Delete Sucessfully",
                    Type = ResponseTypes.Success,
                });
                TempData["ShowToast"] = true;
                return RedirectToAction("Index", "Sales");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to Delete Sale");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Sales Can't be deleted",
                    Type = ResponseTypes.Danger,
                });
            }
            return RedirectToAction("Index", "Sales");
        }
    }
}
