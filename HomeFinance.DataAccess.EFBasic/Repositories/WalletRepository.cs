using HomeFinance.Domain.Dtos;
using HomeFinance.Domain.Models;
using HomeFinance.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HomeFinance.DataAccess.EFBasic.Repositories
{
    internal class WalletRepository : IWalletRepository
    {
        HomeFinanceContextBase _homeFinanceContext;
        public WalletRepository(HomeFinanceContextBase homeFinanceContext)
        {
            _homeFinanceContext = homeFinanceContext;
        }

        public async Task<List<WalletDto>> GetAll(string userId)
        {
            return await _homeFinanceContext.Wallets.Where(i=>i.HomeFinanceUserId==userId).Select(i => new WalletDto(i)).ToListAsync();
        }

        public async Task<WalletDto?> GetById(int id, string userId)
        {
            var wallet = await _homeFinanceContext.Wallets.SingleOrDefaultAsync(i => i.Id == id && i.HomeFinanceUserId == userId);
            if (wallet == null)
                return null;
            return new WalletDto(wallet);
        }

        public async Task Add(WalletDto dto, string userId)
        {
            await _homeFinanceContext.Wallets.AddAsync(new Wallet()
            {
                HomeFinanceUserId = userId,
                Name = dto.Name,
                GroupName = dto.GroupName,
                Comment = dto.Comment
            });
            await _homeFinanceContext.SaveChangesAsync();
        }

        public async Task Update(WalletDto dto, string userId)
        {
            var wallet =await  _homeFinanceContext.Wallets.SingleOrDefaultAsync(i => i.Id == dto.Id && i.HomeFinanceUserId==userId);
            if (wallet == null)
                throw new Exception();
            wallet.Name = dto.Name;
            wallet.GroupName = dto.GroupName;
            wallet.Comment = dto.Comment;
            await _homeFinanceContext.SaveChangesAsync();

        }

        public async Task Remove(int id, string userId)
        {
            var wallet =await _homeFinanceContext.Wallets.SingleOrDefaultAsync(i => i.Id == id && i.HomeFinanceUserId == userId);
            if (wallet == null)
                throw new Exception();
            _homeFinanceContext.Wallets.Remove(wallet);
            await _homeFinanceContext.SaveChangesAsync();
        }
    }




}
 