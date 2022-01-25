using System.ComponentModel.DataAnnotations;

namespace HomeFinance.Domain.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string HomeFinanceUserId { get; set; }

        public virtual HomeFinanceUser HomeFinanceUser { get; set; }

        [Required]
        public string Name { get; set; }

        /// <summary>
        /// true if it is outgo, otherwise it is income
        /// </summary>
        [Required]
        public bool Outgo { get; set; } = true;

        public int? ParentId { get; set; }
        public virtual Category? Parent { get; set; } = null;

        public string? Comment { get; set; } = null;
    }
}
