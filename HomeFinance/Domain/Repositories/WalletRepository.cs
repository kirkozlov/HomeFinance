using HomeFinance.Domain.Dtos;
using HomeFinance.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HomeFinance.Domain.Repositories
{
    public interface IWalletRepository
    {
        public Task<List<WalletDto>> GetAll();
        public Task<WalletDto?> GetById(int id);
        public Task Add(WalletDto dto, string userName);
        public Task Update(WalletDto dto);
        public Task Remove(int id);
    }

    public class WalletRepository:IWalletRepository
    {
        HomeFinanceContext _homeFinanceContext;
        UserManager<HomeFinanceUser> _userManager;
        public WalletRepository(HomeFinanceContext homeFinanceContext, UserManager<HomeFinanceUser> userManager)
        {
            _homeFinanceContext = homeFinanceContext;
            _userManager = userManager;
        }

        public async Task<List<WalletDto>> GetAll()
        {
           return await _homeFinanceContext.Wallets.Select(i => new WalletDto(i)).ToListAsync();
        }

        public async Task<WalletDto?> GetById(int id)
        {
            var wallet = await _homeFinanceContext.Wallets.SingleOrDefaultAsync(i => i.Id == id);
            if (wallet == null)
                return null;
            return new WalletDto(wallet);
        }

        public async Task Add(WalletDto dto, string userName)
        {
            var user=_userManager.FindByNameAsync(userName).Result;
            if (user == null)
                throw new Exception();
            await _homeFinanceContext.Wallets.AddAsync(new Wallet()
            {
                HomeFinanceUser = user,
                Name = dto.Name,
                GroupName = dto.GroupName,
                Comment = dto.Comment
            });
            await _homeFinanceContext.SaveChangesAsync();
        }

        public async Task Update(WalletDto dto)
        {
            var wallet =await  _homeFinanceContext.Wallets.SingleOrDefaultAsync(i => i.Id == dto.Id);
            if (wallet == null)
                throw new Exception();
            wallet.Name = dto.Name;
            wallet.GroupName = dto.GroupName;
            wallet.Comment = dto.Comment;

            _homeFinanceContext.Wallets.Update(wallet);
            await _homeFinanceContext.SaveChangesAsync();

        }

        public async Task Remove(int id)
        {
            var wallet =await _homeFinanceContext.Wallets.SingleOrDefaultAsync(i => i.Id == id);
            if (wallet == null)
                throw new Exception();
            _homeFinanceContext.Wallets.Remove(wallet);
            await _homeFinanceContext.SaveChangesAsync();
        }
    }
}
