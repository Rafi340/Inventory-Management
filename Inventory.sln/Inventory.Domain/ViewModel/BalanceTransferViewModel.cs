using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.ViewModel
{
    public class BalanceTransferViewModel
    {
        public Guid Id { get; set; }
        public int SenderAcountTypeId { get; set; }
        public string SenderAcountType { get; set; }
        public string SenderAccountId { get; set; }
        public string SenderAccountNumber { get; set; }
        public int ReceiverAcountTypeId { get; set; }
        public string ReceiverAccountType { get; set; }
        public string ReceiverAccountId { get; set; }
        public string ReceiverAccountNumber { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TransferAmount { get; set; }
        public DateTime TransferTime { get; set; }
        public string? Note { get; set; }
    }
}
