using HomeFinance.Domain.Enums;
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
       
        [Required]
        public OperationType OperationType { get; set; }

        public int? ParentId { get; set; }
        public virtual Category? Parent { get; set; } = null;

        public string? Comment { get; set; } = null;

        public bool IsValid()
        {
            return OperationType != OperationType.Transfer;
        }
    }
}
