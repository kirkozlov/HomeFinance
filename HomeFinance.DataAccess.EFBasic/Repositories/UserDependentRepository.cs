using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using HomeFinanace.DataAccess.Core.DBModels;
using HomeFinance.Domain.Repositories;
using HomeFinance.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace HomeFinance.DataAccess.EFBasic.Repositories;

abstract class UserDependentRepository<T, TDb, TKey> : IUserDependentRepository<T, TKey> 
    where T : class 
    where TDb: UserDependentBase
{
    protected abstract T ToDomain(TDb db);

    protected abstract TDb ToNewDb(T domain, string userId);

    protected abstract TDb ToExistingDb(T domain);

    private DbSet<TDb> DbSet { get; }

    protected IQueryable<TDb> DataSet =>
        this.DbSet.Where(i => i.HomeFinanceUserId == this.UserId);

    readonly HomeFinanceContextBase _homeFinanceContext;
    protected string UserId { get; }

    protected abstract Expression<Func<TDb,bool>> CheckKey( TKey key);
    

    protected UserDependentRepository(HomeFinanceContextBase homeFinanceContext, DbSet<TDb> dbSet, string userId)
    {
        this._homeFinanceContext = homeFinanceContext;
        this.DbSet = dbSet;
        this.UserId =userId;
    }


    public async Task<List<T>> GetAll()
    {
        var items = (await this.DbSet.Where(i => i.HomeFinanceUserId == this.UserId).ToListAsync()).Select(this.ToDomain).ToList();
        return items;
    }

    public async Task<T?> GetByKey(TKey key)
    {
        var item = await this.DbSet.Where(CheckKey(key)).SingleOrDefaultAsync(i=>i.HomeFinanceUserId == this.UserId);
        if (item == null)
            return null;
        return this.ToDomain(item);
    }

    public async Task<T> Add(T domain)
    {
        var entity = this.ToNewDb(domain, this.UserId);
        await this.DbSet.AddAsync(entity);
        await this._homeFinanceContext.SaveChangesAsync();
        return ToDomain(entity);
    }

    public async Task<T> Update(T domain)
    {
        var entity = this.ToExistingDb(domain);
        await this._homeFinanceContext.SaveChangesAsync();
        return ToDomain(entity);
    }

    public async Task<IEnumerable<T>> Update(IEnumerable<T> domain)
    {
        var entities=domain.Select(d => this.ToExistingDb(d)).ToList();
        await this._homeFinanceContext.SaveChangesAsync();
        return entities.Select(i => ToDomain(i)).ToList();
    }

    public async Task Remove(TKey key)
    {
        var item = await this.DbSet.Where(CheckKey(key)).SingleOrDefaultAsync(i => i.HomeFinanceUserId == this.UserId);
        if (item == null)
            throw new Exception();
        this.DbSet.Remove(item);
        await this._homeFinanceContext.SaveChangesAsync();
    }
    
}