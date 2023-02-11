using HomeFinance.Domain.Enums;

namespace HomeFinance.Domain.DomainModels;

public record TransientOperation(Guid? Id, Guid WalletId, OperationType OperationType, double Amount, string Description, DateTime DateTime)
{
}