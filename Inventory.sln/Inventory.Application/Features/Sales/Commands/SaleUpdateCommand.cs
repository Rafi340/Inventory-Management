using Inventory.Domain.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.Domain.Entities;
using Inventory.Domain.Dtos;

namespace Inventory.Application.Features.Sales.Commands
{
    public class SaleUpdateCommand : IRequest
    {
        public Guid Id { get; set; }
        public DateTime? SaleDate { get; set; }
        public required string InvoiceNo { get; set; }
        public Guid? CustomerId { get; set; }
        public int SalesTypeId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Vat { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal NetAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Discount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PaidAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DueAmount { get; set; }
        public int AcountTypeId { get; set; }
        public string AccountId { get; set; }
        public string? Note { get; set; }
        public string? TermsConditions { get; set; }

        public List<SaleItemAddDto>? SalesItems { get; set; } = new();
    }
}
