using System.Linq;
using System.Linq.Expressions;
using HomeFinanace.DataAccess.Core.DBModels;
using HomeFinance.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace HomeFinance.DataAccess.EFBasic.Repositories;

abstract class UserDependentRepository<T, TDb, TKey> : IUserDependentRepository<T, TKey> 
    where T : class 
    where TDb: UserDependentBase
{
    protected abstract T ToDomain(TDb db);

    protected abstract TDb ToDb(T domain, string userId);

    protected DbSet<TDb> DbSet { get; }

    readonly HomeFinanceContextBase _homeFinanceContext;

    protected abstract Expression<Func<TDb,bool>> CheckKey( TKey key);
    

    protected UserDependentRepository(HomeFinanceContextBase homeFinanceContext, DbSet<TDb> dbSet)
    {
        this._homeFinanceContext = homeFinanceContext;
        this.DbSet = dbSet;
    }


    public async Task<List<T>> GetAll(string userId)
    {
        var items = (await this.DbSet.Where(i => i.HomeFinanceUserId == userId).ToListAsync()).Select(this.ToDomain).ToList();
        return items;
    }

    public async Task<T?> GetByKey(TKey key, string userId)
    {
        var item = await this.DbSet.Where(CheckKey(key)).SingleOrDefaultAsync(i=>i.HomeFinanceUserId == userId);
        if (item == null)
            return null;
        return this.ToDomain(item);
    }

    public async Task<T> Add(T domain, string userId)
    {
        var entity = this.ToDb(domain, userId);
        await this.DbSet.AddAsync(entity);
        await this._homeFinanceContext.SaveChangesAsync();
        return ToDomain(entity);
    }

    public async Task<T> Update(T domain, string userId)
    {
        var entity = this.ToDb(domain, userId);
        this.DbSet.Update(entity);
        await this._homeFinanceContext.SaveChangesAsync();
        return ToDomain(entity);
    }

    public async Task Remove(TKey key, string userId)
    {
        var item = await this.DbSet.Where(CheckKey(key)).SingleOrDefaultAsync(i => i.HomeFinanceUserId == userId);
        if (item == null)
            throw new Exception();
        this.DbSet.Remove(item);
        await this._homeFinanceContext.SaveChangesAsync();
    }
}