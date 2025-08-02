using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    public class Account : BaseModel, IEntity<int>
    {
        public int Id { get; set; }
        public required int AccountTypeId { get; set; }
        public required string AccountNumber { get; set; }
        public string? HolderName { get; set; }
        public string? BankName { get; set; }
        public string? BranchName { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal OpeningBalance { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
    }
}
