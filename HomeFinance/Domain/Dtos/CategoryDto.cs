using HomeFinance.Domain.Models;

namespace HomeFinance.Domain.Dtos
{
    public record CategoryDto(int? Id, string Name, bool Outgo,  int? ParentId, string? Comment)
    {
        public CategoryDto(Category category):this(category.Id, category.Name, category.Outgo, category.ParentId, category.Comment)
        {
        }
    }
}
