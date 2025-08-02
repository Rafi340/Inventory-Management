using Inventory.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Seeds
{
    public static class RoleSeed
    {
        public static ApplicationRole[] GetRoles()
        {
            return [
                new ApplicationRole
                {
                    Id = new Guid("5E76C76D-4CF6-4784-8796-FD9F5C8CF29D"),
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = new DateTime(2025, 8, 1, 1, 2, 1).ToString(),
                },
                new ApplicationRole
                {
                    Id = new Guid("23FE4E81-5015-42A0-8D76-D1F08C6B227A"),
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    ConcurrencyStamp = new DateTime(2025, 7, 1, 1, 2, 3).ToString(),
                }
            ];
        }
    }
}
