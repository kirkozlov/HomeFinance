using HomeFinance.Domain.DomainModels;
using HomeFinance.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Cryptography;
using Tag = HomeFinanace.DataAccess.Core.DBModels.Tag;

namespace HomeFinance.DataAccess.EFBasic.Repositories;

class OperationRepository : UserDependentRepository<Operation, HomeFinanace.DataAccess.Core.DBModels.Operation, Guid>, IOperationRepository
{
    readonly DbSet<Tag> _tags;
    public OperationRepository(HomeFinanceContextBase homeFinanceContext, string userId) : base(homeFinanceContext, homeFinanceContext.Operations, userId)
    {
        this._tags = homeFinanceContext.Tags;
    }

    protected override Operation ToDomain(HomeFinanace.DataAccess.Core.DBModels.Operation db)
    {

        return new Operation(db.Id, db.WalletId, db.OperationType, db.Tags.Select(i=>i.Name).ToList(),db.Amount, db.Comment, db.WalletToId, db.DateTime);
    }

    protected override HomeFinanace.DataAccess.Core.DBModels.Operation ToNewDb(Operation domain, string userId)
    {
        var tags = this._tags.Where(i => domain.Tags.Contains(i.Name)).ToList();
        return new HomeFinanace.DataAccess.Core.DBModels.Operation()
        {
            Id = domain.Id ?? Guid.NewGuid(),
            WalletId = domain.WalletId,
            OperationType = domain.OperationType,
            Tags = tags,
            WalletToId = domain.WalletToId,
            Amount = domain.Amount,
            Comment = domain.Comment,
            DateTime = domain.DateTime,
            HomeFinanceUserId = userId
        };
    }

    protected override HomeFinanace.DataAccess.Core.DBModels.Operation ToExistingDb(Operation domain, string userId)
    {
        var entity = this.DbSet.Where(i=>i.Id==domain.Id && i.HomeFinanceUserId==userId).Single();
        var tags = this._tags.Where(i => domain.Tags.Contains(i.Name)).ToList();

        entity.WalletId = domain.WalletId;
        entity.OperationType = domain.OperationType;
        entity.Tags.Clear();
        tags.ForEach(i => entity.Tags.Add(i));
        entity.WalletToId = domain.WalletToId;
        entity.Amount = domain.Amount;
        entity.Comment = domain.Comment;
        entity.DateTime = domain.DateTime;

        return entity;
    }


    protected override Expression<Func<HomeFinanace.DataAccess.Core.DBModels.Operation, bool>> CheckKey( Guid key)
    {
        Expression<Func<HomeFinanace.DataAccess.Core.DBModels.Operation, bool>> exp = db => db.Id == key;
        return exp;
    }

    public async Task<List<Operation>> GetForWallet(Guid walletId)
    {
        return await GetByPredicate(i=>i.WalletId==walletId || i.WalletToId==walletId);
    }

    public double GetSumFor(Guid walletId)
    {
        var e = this.DbSet
            .Where(o => o.WalletId == walletId)
            .Where(o=> o.OperationType == Domain.Enums.OperationType.Expense || o.OperationType == Domain.Enums.OperationType.Transfer)
            .Sum(o=>o.Amount);

        var i= this.DbSet
            .Where(o => (o.WalletId == walletId && o.OperationType == Domain.Enums.OperationType.Income) || o.WalletToId==walletId)
            .Sum(o => o.Amount);


        return i - e;
    }
}