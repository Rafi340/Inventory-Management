using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Web.Areas.Admin.Models
{
    public class BalanceTransferAddModel : IRequest
    {
        public SelectList? SenderAccountTypeList { get; set; }
        [Required]
        public int SenderAccountTypeId { get; set; }
        [Required]
        public int SenderAccountId { get; set; }
        public SelectList? ReceiverAccountTypeList { get; set; }
        [Required]
        public int ReceiverAccountTypeId { get; set; }
        [Required]
        public int ReceiverAccountId { get; set; }
        [Required(ErrorMessage = "Transfer amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Transfer amount must be greater than zero.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TransferAmount { get; set; }
        public string? Note { get; set; }
    }
}
