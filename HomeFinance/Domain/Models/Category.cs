using System.ComponentModel.DataAnnotations;

namespace HomeFinance.Domain.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public HomeFinanceUser HomeFinanceUser { get; set; }

        [Required]
        public string Name { get; set; }

        public Category? Parent { get; set; } = null;

        public string? Comment { get; set; } = null;
    }
}
