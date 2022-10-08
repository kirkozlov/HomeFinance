using HomeFinance.Domain.Models;

namespace HomeFinance.Domain.DomainModels;

public record Wallet(Guid? Id, string Name, string GroupName, string Comment)
{
}