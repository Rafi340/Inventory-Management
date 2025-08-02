using Inventory.Application.Exceptions;
using Inventory.Application.Features.AccountTypes.Queries;
using Inventory.Application.Features.BalanceTransfers.Commands;
using Inventory.Application.Features.BalanceTransfers.Queries;
using Inventory.Application.Features.Products.Commands;
using Inventory.Infrastructure;
using Inventory.Web.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Web;

namespace Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class BalanceTransfersController(IMediator mediator, ILogger<BalanceTransfersController> logger) : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<BalanceTransfersController> _logger = logger;
        public async Task<IActionResult> Index()
        {
            var accountTypeQuery = new GetAccountTypeQuery();

            var model = new BalanceTransferAddModel
            {
                SenderAccountTypeList = new SelectList(await _mediator.Send(accountTypeQuery), "Id", "Type"), 
                ReceiverAccountTypeList = new SelectList(await _mediator.Send(accountTypeQuery), "Id", "Type"), 
            };
            return View(model);
        }

        public async Task<JsonResult> GetBalanceTransfer([FromBody]GetBalanceTransferSPQuery model)
        {
            var (data, total, totalDisplay) = await _mediator.Send(model);
            var balanceTransfer = new
            {
                recordsTotal = total,
                recordsFiltered = totalDisplay,
                data = (from record in data
                        select new string[]
                        {

                                HttpUtility.HtmlEncode(record.SenderAcountType),
                                HttpUtility.HtmlEncode(record.SenderAccountNumber),
                                HttpUtility.HtmlEncode(record.ReceiverAccountType),
                                HttpUtility.HtmlEncode(record.ReceiverAccountNumber),
                                HttpUtility.HtmlEncode(record.TransferAmount.ToString()),
                                HttpUtility.HtmlEncode(record.TransferTime.ToShortDateString()),
                                HttpUtility.HtmlEncode(record.Note ?? ""),
                                record.Id.ToString()
                        }).ToArray()
            };
            return Json(balanceTransfer);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(BalanceTransferAddCommand model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(model);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Balance Transfer Sucessfully",
                        Type = ResponseTypes.Success,
                    });
                    TempData["ShowToast"] = true;
                    return RedirectToAction("Index", "BalanceTransfers");
                }
                return RedirectToAction("Index", "BalanceTransfers");
            }
            catch (InsufficientBalanceException de)
            {
                ModelState.AddModelError("Insufficient Sender Account Balance", de.Message);
                TempData["ShowToast"] = true;
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = de.Message,
                    Type = ResponseTypes.Danger,
                });

                return RedirectToAction("Index", "BalanceTransfers");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to Transfer Balance");

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Failed to Transfer Balance",
                    Type = ResponseTypes.Danger
                });
                return RedirectToAction("Index", "BalanceTransfers");
            }
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid Id)
        {
            try
            {
                var balanceTransferQuery = new BalanceTransferDeleteCommand
                {
                    Id = Id
                };
                await _mediator.Send(balanceTransferQuery);
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Delete Sucessfully",
                    Type = ResponseTypes.Success,
                });
                TempData["ShowToast"] = true;
                return RedirectToAction("Index", "BalanceTransfers");
            }
            catch (Exception ex)
            {
                TempData["ShowToast"] = true;
                _logger.LogError(ex, "Failed to Delete Balance transfers");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Can't be deleted",
                    Type = ResponseTypes.Danger,
                });
            }
            return RedirectToAction("Index", "BalanceTransfers");
        }
    }
}
