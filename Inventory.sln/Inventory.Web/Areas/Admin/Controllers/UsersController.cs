using Inventory.Application.Features.Products.Queries;
using Inventory.Domain.Dtos;
using Inventory.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Inventory.Application.Features.Users.Queries;
using Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Inventory.Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Inventory.Infrastructure;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger ;
        private readonly IMediator _mediator;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        //private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly UserManager<ApplicationUser> _userManager;
        public UsersController(ILogger<UsersController> logger,
        IMediator mediator,
        RoleManager<ApplicationRole> roleManager,
        IUserStore<ApplicationUser> userStore,
        UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _mediator = mediator;
            _roleManager = roleManager;
            _userStore = userStore;
           // _emailStore = GetEmailStore();
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
           
            return View();
        }
        public async Task<IActionResult> Add()
        {
            var roles = _roleManager.Roles.ToList();
            var model = new UserAddModel();
            model.RoleList = new SelectList(roles, "Name", "Name");
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(UserAddModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = CreateUser();

                    await _userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
                    var emailStore = GetEmailStore();
                    await emailStore.SetEmailAsync(user, model.Email, CancellationToken.None);

                    user.RegistrationDate = DateTime.UtcNow;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    var result = await _userManager.CreateAsync(user, model.Password);

                    await _userManager.AddToRoleAsync(user, model.Role);
                    if (model.Role.Equals("SuperAdmin", StringComparison.OrdinalIgnoreCase))
                    {
                         await _userManager.AddClaimAsync(user, new Claim("delete_user", "allowed"));
                    }
                    TempData["ShowToast"] = true;
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "User added",
                        Type = ResponseTypes.Success
                    });
                    return RedirectToAction("Index", "Users");
                }
                catch (Exception ex)
                {
                    var message = "User Create Failed";
                    ModelState.AddModelError("UserCreateFailed", message);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = message,
                        Type = ResponseTypes.Danger
                    });
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Update(Guid Id)
        {
            var roles = _roleManager.Roles.ToList();
            var getUser = await _userManager.FindByIdAsync(Id.ToString());
            var model = new UserUpdateModel();
            model.Id = Id;
            model.Email = getUser.Email; 
            model.FirstName = getUser.FirstName;
            model.LastName = getUser.LastName; 
            model.RoleList = new SelectList(roles, "Name", "Name");

            var userRoles = await _userManager.GetRolesAsync(getUser);
            if (userRoles.Any())
            {
                model.Role = userRoles.First();
            }
            return View(model);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UserUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id.ToString());
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.UserName = model.Email;

                
                var emailStore = (IUserEmailStore<ApplicationUser>)_userStore;
                await emailStore.SetEmailAsync(user, model.Email, CancellationToken.None);

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    foreach (var error in updateResult.Errors)
                        ModelState.AddModelError("", error.Description);

                    return View(model);
                }

                
                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passResult = await _userManager.ResetPasswordAsync(user, token, model.Password);
                    if (!passResult.Succeeded)
                    {
                        foreach (var error in passResult.Errors)
                            ModelState.AddModelError("", error.Description);

                        return View(model);
                    }
                }

                
                var currentRoles = await _userManager.GetRolesAsync(user);
                if (!currentRoles.Contains(model.Role))
                {
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    await _userManager.AddToRoleAsync(user, model.Role);
                }
                TempData["ShowToast"] = true;
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "User updated successfully.",
                    Type = ResponseTypes.Success
                });

                return RedirectToAction("Index", "Users");
            }

            var roles = _roleManager.Roles.ToList();
            model.RoleList = new SelectList(roles, "Name", "Name", model.Role);

            return View(model);
        }

        [HttpPost, Authorize(Policy = "UserDeletePermission")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "User not found.",
                    Type = ResponseTypes.Danger
                });
                return RedirectToAction("Index","Users");
            }
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, roles);
                if (!removeResult.Succeeded)
                {
                    TempData["ShowToast"] = true;
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Failed to remove user roles .",
                        Type = ResponseTypes.Danger
                    });
                }
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["ShowToast"] = true;
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "User deleted successfully.",
                    Type = ResponseTypes.Success
                });
                return RedirectToAction("Index", "Users");
            }
            else
            {
                TempData["ShowToast"] = true;
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Failed to delete user.",
                    Type = ResponseTypes.Danger
                });
            }

            return RedirectToAction("Index", "Users");
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
        [HttpPost]
        public async Task<JsonResult> GetUsers([FromBody] GetUserSPQuery model)
        {
            try
            {
                var (data, total, totalDisplay) = await _mediator.Send(model); ;
                var products = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,
                    data = (from record in data
                            select new string[]
                            {
                                
                                HttpUtility.HtmlEncode(record.UserName),
                                HttpUtility.HtmlEncode(record.FirstName),
                                HttpUtility.HtmlEncode(record.LastName),
                                HttpUtility.HtmlEncode(record.RoleName),
                                HttpUtility.HtmlEncode(record.RegistrationDate),
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

    }
}
