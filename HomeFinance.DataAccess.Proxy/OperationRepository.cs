using HomeFinance.Domain.DomainModels;
using HomeFinance.Domain.Repositories;

namespace HomeFinance.DataAccess.Proxy;

class OperationRepository :  UserDependentRepository<Operation, Guid>, IOperationRepository
{
    public OperationRepository(HttpClient client, string path) : base(client, path)
    {
    }

    public double GetSumFor(Guid walletId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Operation>> GetForWalletAndPeriod(Guid? walletId, DateTime? from, DateTime? to)
    {
        throw new NotImplementedException();
    }
}