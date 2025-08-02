using Inventory.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Inventory.Domain.Services
{
    public interface IUnitService
    {
        List<SelectListItem> UnitDropdown();
        Unit GetUnit(int id);
    }
}
