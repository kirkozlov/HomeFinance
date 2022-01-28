using HomeFinance.Domain.Enums;
using HomeFinance.Domain.Models;

namespace HomeFinance.Domain.Dtos
{
    public record CategoryDto(int? Id, string Name, OperationType OperationType,  int? ParentId, string? Comment)
    {
        public CategoryDto(Category category):this(category.Id, category.Name, category.OperationType, category.ParentId, category.Comment)
        {
        }
    }

}
