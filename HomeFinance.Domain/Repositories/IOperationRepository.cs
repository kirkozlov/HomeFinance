using HomeFinance.Domain.DomainModels;

namespace HomeFinance.Domain.Repositories;

public interface IOperationRepository : IUserDependentRepository<Operation, Guid>
{
    public double GetSumFor(Guid walletId);
    public Task<List<Operation>> GetForWalletAndPeriod(Guid? walletId, DateTime? from, DateTime? to);

}