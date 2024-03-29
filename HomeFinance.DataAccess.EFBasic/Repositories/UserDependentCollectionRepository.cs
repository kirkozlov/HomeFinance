﻿using System;
using System.Linq.Expressions;
using HomeFinance.DataAccess.Core.DBModels;
using HomeFinance.DataAccess.EFBasic.Util;
using HomeFinance.Domain.Repositories;
using HomeFinance.Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace HomeFinance.DataAccess.EFBasic.Repositories;

abstract class UserDependentCollectionRepository<T, TDb, TKey> : UserDependentRepository<T,TDb>, IUserDependentCollectionRepository<T, TKey> 
    where T : class 
    where TDb: UserDependentBase
{
    
 
    protected abstract Expression<Func<TDb,bool>> CheckKey( TKey key);

    readonly HomeFinanceContextBase _homeFinanceContext;
    DbSet<TDb> DbSet { get; }

    protected readonly DateConverter _dateConverter;

    protected DateConverter DateConverter=>_dateConverter;

    protected UserDependentCollectionRepository(HomeFinanceContextBase homeFinanceContext, DbSet<TDb> dbSet, string userId)
        :base(homeFinanceContext, dbSet, userId)
    {
        _homeFinanceContext = homeFinanceContext;
        _dateConverter = new DateConverter(TimeZoneInfo
                              .FindSystemTimeZoneById(_homeFinanceContext
                                                .UserPreferences
                                                .Single(i => i.HomeFinanceUserId == userId)
                                                .TimeZoneId)
                    );
        DbSet = dbSet;
    }

    public async Task<T?> GetByKey(TKey key)
    {
        var item = await this.DataSet.Where(CheckKey(key)).SingleOrDefaultAsync();
        if (item == null)
            return null;
        return this.ToDomain(item);
    }

    public async Task<IEnumerable<T>> AddRange(IEnumerable<T> domain)
    {
        var entities = domain.Select(i=>this.ToNewDb(i,this.UserId)).ToList();
        await this.DbSet.AddRangeAsync(entities);
        await this._homeFinanceContext.SaveChangesAsync();
        return entities.Select(this.ToDomain).ToList();
    }

    public async Task<IEnumerable<T>> Update(IEnumerable<T> domain)
    {
        var entities=domain.Select(this.ToExistingDb).ToList();
        await this._homeFinanceContext.SaveChangesAsync();
        return entities.Select(this.ToDomain).ToList();
    }

    public async Task Remove(TKey key)
    {
        var item = await this.DataSet.Where(CheckKey(key)).SingleOrDefaultAsync();
        if (item == null)
            throw new Exception();
        this.DbSet.Remove(item);
        await this._homeFinanceContext.SaveChangesAsync();
    }
    
}