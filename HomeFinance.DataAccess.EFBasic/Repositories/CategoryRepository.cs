using HomeFinance.Domain.Dtos;
using HomeFinance.Domain.Models;
using HomeFinance.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HomeFinance.DataAccess.EFBasic.Repositories
{
    internal class CategoryRepository : ICategoryRepository
    {
        HomeFinanceContextBase _homeFinanceContext;
        public CategoryRepository(HomeFinanceContextBase homeFinanceContext)
        {
            _homeFinanceContext = homeFinanceContext;
        }

        public async Task<List<CategoryDto>> GetAll(string userId)
        {

            var categories= await _homeFinanceContext.Categories.Where(i => i.HomeFinanceUserId == userId).Select(i => new CategoryDto(i)).ToListAsync();
            return categories;
        }

        public async Task<CategoryDto?> GetById(int id, string userId)
        {
            var category = await _homeFinanceContext.Categories.SingleOrDefaultAsync(i => i.Id == id && i.HomeFinanceUserId == userId);
            if (category == null)
                return null;
            return new CategoryDto(category);
        }

        public async Task Add(CategoryDto dto, string userId)
        {
            await _homeFinanceContext.Categories.AddAsync(new Category()
            {
                HomeFinanceUserId = userId,
                Name = dto.Name,
                OperationType = dto.OperationType,
                ParentId = dto.ParentId,
                Comment = dto.Comment
            });
            await _homeFinanceContext.SaveChangesAsync();
        }

        public async Task Update(CategoryDto dto, string userId)
        {
            var category =await  _homeFinanceContext.Categories.SingleOrDefaultAsync(i => i.Id == dto.Id && i.HomeFinanceUserId== userId);
            if (category == null)
                throw new Exception();

            category.Name = dto.Name;
            category.OperationType = dto.OperationType;
            category.ParentId = dto.ParentId;
            category.Comment = dto.Comment;

            await _homeFinanceContext.SaveChangesAsync();

        }

        public async Task Remove(int id, string userId)
        {
            var category =await _homeFinanceContext.Categories.SingleOrDefaultAsync(i => i.Id == id && i.HomeFinanceUserId == userId);
            if (category == null)
                throw new Exception();
            _homeFinanceContext.Categories.Remove(category);
            await _homeFinanceContext.SaveChangesAsync();
        }
    }


}
 