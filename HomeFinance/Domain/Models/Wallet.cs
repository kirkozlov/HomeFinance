using System.ComponentModel.DataAnnotations;

namespace HomeFinance.Domain.Models
{
    public class Wallet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string HomeFinanceUserId { get; set; }

        public virtual HomeFinanceUser HomeFinanceUser { get; set; }

        [Required]
        public string Name { get; set; }

        public string? GroupName { get; set; } = null;

        public string? Comment { get; set; } = null;
    }
}
