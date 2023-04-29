using HomeFinance.Domain.Enums;

namespace HomeFinance.Domain.DomainModels;

public record Tag(string Name, OperationType OperationType, string ParentTagName, int SortId)
{
}