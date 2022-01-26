using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeFinance.Domain.Models
{
    public enum OperationType
    {
        Income, Outgo, Transfer
    }
    public enum RepeatableType
    {
        Month, Quarter, Year
    }
    public class RepeatableOperation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string HomeFinanceUserId { get; set; }

        public virtual HomeFinanceUser HomeFinanceUser { get; set; }

        [Required]
        public OperationType OperationType { get; set; }

        [Required]
        public int WalletId { get; set; }

        public virtual Wallet Wallet { get; set; }

        public int? CategoryId { get; set; }

        public virtual Category? Category { get; set; }

        public int? WalletIdTo { get; set; }

        [ForeignKey(nameof(WalletIdTo))]
        public virtual Wallet? WalletTo { get; set; }

        [Required]
        public DateTime NextExecution { get; set; }

        [Required]
        public RepeatableType RepeatableType { get; set; }

        [Required]
        public double Amount { get; set; }

        public string? Comment { get; set; } = null;

    }
}
