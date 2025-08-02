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
    public class UnitService : IUnitService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
       public UnitService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public Unit GetUnit(int id)
        {
            return _applicationUnitOfWork.UnitRepository.GetById(id);
        }

        public List<SelectListItem> UnitDropdown()
        {
            var getUnitDropdown = _applicationUnitOfWork.UnitRepository.GetAll().Select(s =>new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name
            }).ToList();
            return getUnitDropdown;
        }
    }
}
