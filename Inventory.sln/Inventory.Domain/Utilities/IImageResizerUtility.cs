using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Utilities
{
    public interface IImageResizerUtility
    {
        Task ResizeImageAsync(string ImagePath);
    }
}
