using HomeFinance.Domain.Enums;

namespace HomeFinance.Domain.DomainModels;

public record Tag(string Name, OperationType OperationType, int SortId)
{
}