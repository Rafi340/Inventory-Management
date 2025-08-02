using Inventory.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Web.Areas.Admin.Models
{
    public class SalesAddModel
    {
        [Required]
        public DateTime? SaleDate { get; set; }
        [Required]
        public Guid? CustomerId { get; set; }
        public SelectList? CustomerList { get; set; }
        public int SalesTypeId { get; set; }
        public SelectList? SalesTypeList { get; set; }
        public SelectList? ProductList { get; set; }
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
        public SelectList? AcountTypeList { get; set; }
        [Required]
        public int AccountId { get; set; }
        public SelectList? AccountList { get; set; }
        public string? Note { get; set; }
        public string? TermsConditions { get; set; }

        public List<SaleItems> SalesItems { get; set; } = new();

    }
}
