using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Seeds
{
    public class UnitSeed
    {
        public static Unit[] UnitSeeds()
        {
            return [
                new Unit { Id = 1, Name = "Kilogram" },
        new Unit { Id = 2, Name = "Gram" },
        new Unit { Id = 3, Name = "Milligram" },
        new Unit { Id = 4, Name = "Pound" },
        new Unit { Id = 5, Name = "Metric Ton" },
        new Unit { Id = 6, Name = "Liter" },
        new Unit { Id = 7, Name = "Milliliter" },
        new Unit { Id = 8, Name = "Gallon" },
        new Unit { Id = 9, Name = "Meter" },
        new Unit { Id = 10, Name = "Centimeter" },
        new Unit { Id = 11, Name = "Millimeter" },
        new Unit { Id = 12, Name = "Foot" },
        new Unit { Id = 13, Name = "Inch" },
        new Unit { Id = 14, Name = "Square Meter" },
        new Unit { Id = 15, Name = "Square Foot" },
        new Unit { Id = 16, Name = "Cubic Meter" },
        new Unit { Id = 17, Name = "Piece" },
        new Unit { Id = 18, Name = "Box" },
        new Unit { Id = 19, Name = "Pack" },
        new Unit { Id = 20, Name = "Set" },
        new Unit { Id = 21, Name = "Roll" },
        new Unit { Id = 22, Name = "Bag" },
        new Unit { Id = 23, Name = "Can" },
        new Unit { Id = 24, Name = "Drum" },
        new Unit { Id = 25, Name = "Pallet" },
        new Unit { Id = 26, Name = "Bottle" },
        new Unit { Id = 27, Name = "Jar" },
        new Unit { Id = 28, Name = "Tube" }
            ];
        }

    }
}
