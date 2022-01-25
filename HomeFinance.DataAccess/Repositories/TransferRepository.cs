using HomeFinance.Domain.Dtos;
using HomeFinance.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeFinance.Domain.Repositories
{

    public class TransferRepository : ITransferRepository
    {
        HomeFinanceContext _homeFinanceContext;

        public TransferRepository(HomeFinanceContext homeFinanceContext)
        {
            _homeFinanceContext = homeFinanceContext;
        }

        public async Task<List<TransferDto>> GetAll(string userId)
        {
            return await _homeFinanceContext.Transfers.Where(i => i.HomeFinanceUserId == userId).Select(i => new TransferDto(i)).ToListAsync();
        }
        public async Task<TransferDto?> GetById(int id, string userId)
        {
            var transfer = await _homeFinanceContext.Transfers.SingleOrDefaultAsync(i => i.Id == id && i.HomeFinanceUserId == userId);
            if (transfer == null)
                return null;
            return new TransferDto(transfer);
        }
        public async Task Add(TransferDto dto, string userId)
        {
            await _homeFinanceContext.Transfers.AddAsync(new Transfer()
            {
                HomeFinanceUserId = userId,
                WalletIdFrom = dto.WalletIdFrom,
                WalletIdTo = dto.WalletIdTo,
                DateTime = dto.DateTime,
                Amount = dto.Amount,
                Comment = dto.Comment
            });
            await _homeFinanceContext.SaveChangesAsync();
        }
      
        public async Task Update(TransferDto dto, string userId)
        {
            var transfer = await _homeFinanceContext.Transfers.SingleOrDefaultAsync(i => i.Id == dto.Id && i.HomeFinanceUserId == userId);
            if (transfer == null)
                throw new Exception();

            transfer.WalletIdFrom = dto.WalletIdFrom;
            transfer.WalletIdTo = dto.WalletIdTo;
            transfer.DateTime = dto.DateTime;
            transfer.Amount = dto.Amount;
            transfer.Comment = dto.Comment;
            await _homeFinanceContext.SaveChangesAsync();
        }
        public async Task Remove(int id, string userId)
        {
            var transfer = await _homeFinanceContext.Transfers.SingleOrDefaultAsync(i => i.Id == id && i.HomeFinanceUserId == userId);
            if (transfer == null)
                throw new Exception();
            _homeFinanceContext.Transfers.Remove(transfer);
            await _homeFinanceContext.SaveChangesAsync();
        }
    }
}
