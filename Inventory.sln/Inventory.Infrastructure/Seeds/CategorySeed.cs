using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Seeds
{
    public class CategorySeed
    {
        public static Category[] CategorySeeds()
        {
            return [
                new Category { Id = 1, Name = "Raw Materials" },
        new Category { Id = 2, Name = "Finished Goods" },
        new Category { Id = 3, Name = "Packaging Materials" },
        new Category { Id = 4, Name = "Spare Parts" },
        new Category { Id = 5, Name = "Machinery" },
        new Category { Id = 6, Name = "Tools" },
        new Category { Id = 7, Name = "Electrical Components" },
        new Category { Id = 8, Name = "Plumbing Supplies" },
        new Category { Id = 9, Name = "Construction Materials" },
        new Category { Id = 10, Name = "Hardware" },
        new Category { Id = 11, Name = "Fasteners" },
        new Category { Id = 12, Name = "Chemicals" },
        new Category { Id = 13, Name = "Lubricants" },
        new Category { Id = 14, Name = "Safety Equipment" },
        new Category { Id = 15, Name = "Cleaning Supplies" },
        new Category { Id = 16, Name = "Office Supplies" },
        new Category { Id = 17, Name = "Furniture" },
        new Category { Id = 18, Name = "Lighting Fixtures" },
        new Category { Id = 19, Name = "Paints & Coatings" },
        new Category { Id = 20, Name = "Glass & Mirrors" },
        new Category { Id = 21, Name = "Batteries" },
        new Category { Id = 22, Name = "Cables & Wires" },
        new Category { Id = 23, Name = "Pipes & Fittings" },
        new Category { Id = 24, Name = "Adhesives & Sealants" },
        new Category { Id = 25, Name = "Textiles" },
        new Category { Id = 26, Name = "Metals & Alloys" },
        new Category { Id = 27, Name = "Plastic Components" },
        new Category { Id = 28, Name = "Wood Products" },
        new Category { Id = 29, Name = "HVAC Equipment" },
        new Category { Id = 30, Name = "Fire Protection Equipment" },
        new Category { Id = 31, Name = "Automotive Parts" },
        new Category { Id = 32, Name = "Agricultural Supplies" },
        new Category { Id = 33, Name = "Medical Supplies" },
        new Category { Id = 34, Name = "Warehouse Equipment" },
        new Category { Id = 35, Name = "IT & Networking Equipment" },
        new Category { Id = 36, Name = "Measurement Instruments" },
        new Category { Id = 37, Name = "Inspection Tools" },
        new Category { Id = 38, Name = "Welding Supplies" },
        new Category { Id = 39, Name = "Construction Equipment" },
        new Category { Id = 40, Name = "Conveyor Components" }
            ];
        }

    }
}
