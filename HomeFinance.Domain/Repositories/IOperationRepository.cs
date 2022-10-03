using HomeFinance.Domain.DomainModels;

namespace HomeFinance.Domain.Repositories;

public interface IOperationRepository : IUserDependentRepository<Operation, Guid>
{
    public Task<List<Operation>> GetForWallet(string userId, Guid walletId);
        
}