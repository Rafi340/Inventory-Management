using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.BalanceTransfers.Commands
{
    public class BalanceTransferAddCommand : IRequest
    {
        public int SenderAccountTypeId { get; set; }
        public int SenderAccountId { get; set; }
        public int ReceiverAccountTypeId { get; set; }
        public int ReceiverAccountId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TransferAmount { get; set; }
        public string? Note { get; set; }
    }
}
