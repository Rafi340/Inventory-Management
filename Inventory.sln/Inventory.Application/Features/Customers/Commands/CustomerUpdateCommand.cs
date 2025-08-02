using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Customers.Commands
{
    public class CustomerUpdateCommand : IRequest
    {
        public Guid Id { get; set; }
        public required string CustomerId { get; set; }
        public required string Name { get; set; }
        public string? CompanyName { get; set; }
        public string? Email { get; set; }
        public required string Mobile { get; set; }
        public string? Address { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? ImageUrl { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal OpeningBalance { get; set; }
    }
}
