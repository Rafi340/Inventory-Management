using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.Domain.Entities;
using Inventory.Domain.Repositories;
using Inventory.Domain.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Inventory.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public CategoryService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public List<SelectListItem> CategoryDropdown()
        {
            var getCategoryDropdown = _applicationUnitOfWork.CategoryRepository.GetAll().Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name
            }).ToList();
            return getCategoryDropdown;
        }

        public Category GetCategory(int id)
        {
            return _applicationUnitOfWork.CategoryRepository.GetById(id);
        }
    }
}
