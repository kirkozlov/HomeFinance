using HomeFinance.DataAccess.Core.DBModels;
using HomeFinance.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HomeFinance.DataAccess.EFBasic.Repositories;

abstract class UserDependentRepository<T, TDb>: IUserDependentRepository<T>
    where T : class
    where TDb : UserDependentBase
{
    private DbSet<TDb> DbSet { get; }
    protected string UserId { get; }
    readonly HomeFinanceContextBase _homeFinanceContext;

    protected abstract TDb ToNewDb(T domain, string userId);

    protected abstract TDb ToExistingDb(T domain);
    protected abstract T ToDomain(TDb db);

    protected IQueryable<TDb> DataSet =>
     this.DbSet.Where(i => i.HomeFinanceUserId == this.UserId);
   

    protected UserDependentRepository(HomeFinanceContextBase homeFinanceContext, DbSet<TDb> dbSet, string userId)
    {
        this._homeFinanceContext = homeFinanceContext;
        this.DbSet = dbSet;
        this.UserId = userId;
    }

    public async Task<List<T>> GetAll()
    {
        var items = (await this.DataSet.ToListAsync()).Select(this.ToDomain).ToList();
        return items;
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
}
