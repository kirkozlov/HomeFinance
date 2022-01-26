using HomeFinance.Domain.Dtos;

namespace HomeFinance.Domain.Repositories
{
    public interface IOperationRepository : IUserDependentRepository<OperationDto>
    {
        public Task<List<OperationDto>> GetForWallet(string userId, int walletId);
        
    }
   

  
}
