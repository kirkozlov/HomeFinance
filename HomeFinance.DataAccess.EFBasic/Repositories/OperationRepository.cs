using HomeFinance.Domain.DomainModels;
using HomeFinance.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Tag = HomeFinance.DataAccess.Core.DBModels.Tag;

namespace HomeFinance.DataAccess.EFBasic.Repositories;

class OperationRepository : UserDependentCollectionRepository<Operation, HomeFinance.DataAccess.Core.DBModels.Operation, Guid>, IOperationRepository
{
    readonly DbSet<Tag> _tags;
    public OperationRepository(HomeFinanceContextBase homeFinanceContext, string userId) : base(homeFinanceContext, homeFinanceContext.Operations, userId)
    {
        this._tags = homeFinanceContext.Tags;
    }

    protected override Operation ToDomain(HomeFinance.DataAccess.Core.DBModels.Operation db)
    {
        var allTags = new List<Tag>();
        var nextTags = db.Tags.ToList();
        while (nextTags.Any())
        {
            var uniqueTags = nextTags.Where(i => !allTags.Contains(i)).Distinct().ToList();
            allTags.AddRange(uniqueTags);
            nextTags = uniqueTags.Select(i => i.ParentTag).OfType<Tag>().Distinct().ToList();
        }
        return new Operation(db.Id, db.WalletId, db.OperationType, allTags.Select(i=>i.Name).ToList(),db.Amount, db.Comment, db.WalletToId, DateConverter.ToLocalDateTime(db.DateTime));
    }

    protected override HomeFinance.DataAccess.Core.DBModels.Operation ToNewDb(Operation domain, string userId)
    {
        var tags = this._tags.Where(i => domain.Tags.Contains(i.Name)).ToList();
        return new HomeFinance.DataAccess.Core.DBModels.Operation()
        {
            Id = domain.Id ?? Guid.NewGuid(),
            WalletId = domain.WalletId,
            OperationType = domain.OperationType,
            Tags = tags,
            WalletToId = domain.WalletToId,
            Amount = domain.Amount,
            Comment = domain.Comment,
            DateTime = DateConverter.ToUtcDateTime( domain.DateTime),
            HomeFinanceUserId = userId
        };
    }

    protected override HomeFinance.DataAccess.Core.DBModels.Operation ToExistingDb(Operation domain)
    {
        var entity = this.DataSet.Single(i => i.Id == domain.Id);
        var tags = this._tags.Where(i => domain.Tags.Contains(i.Name)).ToList();

        entity.WalletId = domain.WalletId;
        entity.OperationType = domain.OperationType;
        entity.Tags.Clear();
        tags.ForEach(i => entity.Tags.Add(i));
        entity.WalletToId = domain.WalletToId;
        entity.Amount = domain.Amount;
        entity.Comment = domain.Comment;
        entity.DateTime = DateConverter.ToUtcDateTime(domain.DateTime);

        return entity;
    }


    protected override Expression<Func<HomeFinance.DataAccess.Core.DBModels.Operation, bool>> CheckKey( Guid key)
    {
        Expression<Func<HomeFinance.DataAccess.Core.DBModels.Operation, bool>> exp = db => db.Id == key;
        return exp;
    }

    public async Task<List<Operation>> GetForWalletAndPeriod(Guid? walletId, DateTime? from, DateTime? to)
    {
        var result = this.DataSet;
        if (walletId.HasValue)
        {
            result = result.Where(i => i.WalletId == walletId || i.WalletToId == walletId);
        }

        if (from != null && to != null)
        {
            result = result.Where(i => from <= i.DateTime && i.DateTime < to);
        }

        var tmp = await result.Include(x=>x.Tags).ToListAsync();
        return tmp.Select(this.ToDomain).ToList();
    }

    public double GetSumFor(Guid walletId)
    {
        var e = this.DataSet
            .Where(o => o.WalletId == walletId)
            .Where(o=> o.OperationType == Domain.Enums.OperationType.Expense || o.OperationType == Domain.Enums.OperationType.Transfer)
            .Sum(o=>o.Amount);

        var i= this.DataSet
            .Where(o => (o.WalletId == walletId && o.OperationType == Domain.Enums.OperationType.Income) || o.WalletToId==walletId)
            .Sum(o => o.Amount);

        return i - e;
    }
}