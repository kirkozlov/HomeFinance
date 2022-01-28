using HomeFinance.Domain.Dtos;
using HomeFinance.Domain.Models;
using HomeFinance.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HomeFinance.DataAccess.Repositories
{
    internal class RepeatableOperationRepository : IRepeatableOperationRepository
    {
        HomeFinanceContext _homeFinanceContext;

        public RepeatableOperationRepository(HomeFinanceContext homeFinanceContext)
        {
            _homeFinanceContext = homeFinanceContext;
        }

        public async Task<List<RepeatableOperationDto>> GetAll(string userId)
        {
            return await _homeFinanceContext.RepeatableOperations.Where(i => i.HomeFinanceUserId == userId).Select(i => new RepeatableOperationDto(i)).ToListAsync();

        }

        public async Task<RepeatableOperationDto?> GetById(int id, string userId)
        {
            var operation = await _homeFinanceContext.RepeatableOperations.SingleOrDefaultAsync(i => i.Id == id && i.HomeFinanceUserId == userId);
            if (operation == null)
                return null;
            return new RepeatableOperationDto(operation);
        }

        public async Task Add(RepeatableOperationDto dto, string userId)
        {
            await _homeFinanceContext.RepeatableOperations.AddAsync(new RepeatableOperation()
            {
                HomeFinanceUserId = userId,
                OperationType = dto.OperationType,
                WalletId = dto.WalletId,
                CategoryId = dto.CategoryId,
                WalletIdTo=dto.WalletIdTo,
                NextExecution = dto.NextExecution,
                RepeatableType = dto.RepeatableType,
                Amount = dto.Amount,
                Comment = dto.Comment
            });
            await _homeFinanceContext.SaveChangesAsync();
        }

        

        public async Task Update(RepeatableOperationDto dto, string userId)
        {
            var operation = await _homeFinanceContext.RepeatableOperations.SingleOrDefaultAsync(i => i.Id == dto.Id && i.HomeFinanceUserId == userId);
            if (operation == null)
                throw new Exception();


            operation.WalletId = dto.WalletId;
            operation.OperationType = dto.OperationType;
            operation.CategoryId = dto.CategoryId;
            operation.WalletIdTo = dto.WalletIdTo;
            operation.NextExecution = dto.NextExecution;
            operation.RepeatableType = dto.RepeatableType;
            operation.Amount = dto.Amount;
            operation.Comment = dto.Comment;

            await _homeFinanceContext.SaveChangesAsync();
        }

        public async Task Remove(int id, string userId)
        {
            var operation = await _homeFinanceContext.RepeatableOperations.SingleOrDefaultAsync(i => i.Id == id && i.HomeFinanceUserId == userId);
            if (operation == null)
                throw new Exception();
            _homeFinanceContext.RepeatableOperations.Remove(operation);
            await _homeFinanceContext.SaveChangesAsync();
        }
    }
}
 