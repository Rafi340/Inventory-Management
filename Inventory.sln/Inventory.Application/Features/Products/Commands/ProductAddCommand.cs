using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Products.Commands
{
    public class ProductAddCommand : IRequest
    {
        [Required]
        public string ProductCode { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public string SKU { get; set; }
        [Required]
        public int UnitId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal MrpPrice { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal WholeSalePrice { get; set; }
        public int? LowStock { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
