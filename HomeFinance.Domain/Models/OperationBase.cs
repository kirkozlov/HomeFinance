using HomeFinance.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeFinance.Domain.Models
{
   
    public abstract class OperationBase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string HomeFinanceUserId { get; set; }

        public virtual HomeFinanceUser HomeFinanceUser { get; set; }

        [Required]
        public int WalletId { get; set; }

        public virtual Wallet Wallet { get; set; }

        [Required]
        public OperationType OperationType { get; set; }

  
        public int? CategoryId { get; set; }

        public virtual Category? Category { get; set; }

        
        public int? WalletIdTo { get; set; }
        //[ForeignKey(nameof(WalletIdTo))]
        public virtual Wallet? WalletTo { get; set; }


        [Required]
        public double Amount { get; set; }

        public string? Comment { get; set; } = null;

        public virtual bool IsValid()
        {
            if (Amount <= 0) return false;
            if (WalletId == WalletIdTo) return false;
            return (OperationType==OperationType.Transfer && WalletIdTo!=null && CategoryId==null) 
                || (OperationType!=OperationType.Transfer && WalletIdTo==null && CategoryId!=null);
        }

    }
}
