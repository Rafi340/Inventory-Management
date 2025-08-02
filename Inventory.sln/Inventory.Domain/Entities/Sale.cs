using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    public class Sale : BaseModel, IEntity<Guid>
    {
        public Guid Id { get; set; }
        public required string InvoiceNo { get; set; }
        public required DateTime SaleDate { get; set; }
        public required Guid CustomerId { get; set; }
        public int SalesTypeId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Vat {  get; set; }
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
        public int AccountId { get; set; }
        public string? Note { get; set; }
        public string? TermsConditions { get; set; }
        
        public List<SaleItems> SalesItems { get; set; } = new();
    }
}
