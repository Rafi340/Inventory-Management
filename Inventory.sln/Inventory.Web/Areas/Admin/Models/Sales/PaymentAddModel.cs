using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Web.Areas.Admin.Models
{
    public class PaymentAddModel
    {
        public Guid Id { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DueAmount { get; set; }
        [Required(ErrorMessage = "Paid amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Paid amount must be greater than zero.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PaidAmount { get; set; }
        public SelectList? AcountTypeList { get; set; }
        public int AcountTypeId { get; set; }
        public SelectList? AccountList { get; set; }
        public string AccountId { get; set; }
    }
}
