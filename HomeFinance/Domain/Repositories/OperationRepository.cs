﻿using HomeFinance.Domain.Dtos;
using HomeFinance.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeFinance.Domain.Repositories
{
    public interface IOperationRepository : IUserDependentRepository<OperationDto>
    {
        public Task<List<OperationDto>> GetForWallet(string userId, int walletId);
        
    }
    public class OperationRepository : IOperationRepository
    {
        HomeFinanceContext _homeFinanceContext;

        public OperationRepository(HomeFinanceContext homeFinanceContext)
        {
            _homeFinanceContext = homeFinanceContext;
        }
        public async Task<List<OperationDto>> GetAll(string userId)
        {
            return await _homeFinanceContext.Operations.Where(i => i.HomeFinanceUserId == userId).Select(i => new OperationDto(i)).ToListAsync();
        }
        public async Task<List<OperationDto>> GetForWallet(string userId, int walletId)
        {
            return await _homeFinanceContext.Operations.Where(i => i.HomeFinanceUserId == userId && i.WalletId==walletId).Select(i => new OperationDto(i)).ToListAsync();
        }
        public async Task<OperationDto?> GetById(int id, string userId)
        {
            var category = await _homeFinanceContext.Operations.SingleOrDefaultAsync(i => i.Id == id && i.HomeFinanceUserId == userId);
            if (category == null)
                return null;
            return new OperationDto(category);
        }
        public async Task Add(OperationDto dto, string userId)
        {
            await _homeFinanceContext.Operations.AddAsync(new Operation()
            {
                HomeFinanceUserId=userId,
                WalletId=dto.WalletId,
                CategoryId=dto.CategoryId,
                DateTime=dto.DateTime,
                Outgo=dto.Outgo,
                Amount=dto.Amount,
                Comment = dto.Comment
            });
            await _homeFinanceContext.SaveChangesAsync();
        }
        public async Task Update(OperationDto dto, string userId)
        {
            var operation = await _homeFinanceContext.Operations.SingleOrDefaultAsync(i => i.Id == dto.Id && i.HomeFinanceUserId == userId);
            if (operation == null)
                throw new Exception();

            operation.WalletId = dto.WalletId;
            operation.CategoryId = dto.CategoryId;
            operation.DateTime = dto.DateTime;
            operation.Outgo = dto.Outgo;
            operation.Amount = dto.Amount;
            operation.Comment = dto.Comment;
            await _homeFinanceContext.SaveChangesAsync();
        }
        public async Task Remove(int id, string userId)
        {
            var operation = await _homeFinanceContext.Operations.SingleOrDefaultAsync(i => i.Id == id && i.HomeFinanceUserId == userId);
            if (operation == null)
                throw new Exception();
            _homeFinanceContext.Operations.Remove(operation);
            await _homeFinanceContext.SaveChangesAsync();
        }

    }

  
}
