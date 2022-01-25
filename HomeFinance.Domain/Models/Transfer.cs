using System.ComponentModel.DataAnnotations;

namespace HomeFinance.Domain.Models
{
    public class Transfer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string HomeFinanceUserId { get; set; }

        public virtual HomeFinanceUser HomeFinanceUser { get; set; }

        [Required]
        public int WalletIdFrom { get; set; }

        public virtual Wallet WalletFrom { get; set; }

        [Required]
        public int WalletIdTo { get; set; }

        public virtual Wallet WalletTo { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public double Amount { get; set; }

        public string? Comment { get; set; } = null;

    }
}
