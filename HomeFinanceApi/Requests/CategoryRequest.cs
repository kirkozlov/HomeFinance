using HomeFinance.Domain.Dtos;
using HomeFinance.Domain.Enums;

namespace HomeFinanceApi.Requests
{
    public class CategoryRequest
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        OperationType OperationType { get; set; }
        public int? ParentId { get; set; }
        public string? Comment { get; set; }


        public CategoryDto ToDto()
        {
            return new CategoryDto(Id, Name, OperationType,ParentId, Comment);
        }
    }
}
