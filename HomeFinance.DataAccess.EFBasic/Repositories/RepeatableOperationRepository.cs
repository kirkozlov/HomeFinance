using HomeFinance.Domain.DomainModels;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Tag = HomeFinance.DataAccess.Core.DBModels.Tag;

namespace HomeFinance.DataAccess.EFBasic.Repositories;

class RepeatableOperationRepository : UserDependentCollectionRepository<RepeatableOperation, HomeFinance.DataAccess.Core.DBModels.RepeatableOperation, Guid>
{
    readonly DbSet<Tag> _tags;
    public RepeatableOperationRepository(HomeFinanceContextBase homeFinanceContext, string userId) : base(homeFinanceContext, homeFinanceContext.RepeatableOperations, userId)
    {
        this._tags = homeFinanceContext.Tags;
    }

    protected override RepeatableOperation ToDomain(HomeFinance.DataAccess.Core.DBModels.RepeatableOperation db)
    {
        var allTags = new List<Tag>();
        var nextTags = db.Tags.ToList();
        while (nextTags.Any())
        {
            var uniqueTags = nextTags.Where(i => !allTags.Contains(i)).Distinct().ToList();
            allTags.AddRange(uniqueTags);
            nextTags = uniqueTags.Select(i => i.ParentTag).OfType<Tag>().Distinct().ToList();
        }
        return new RepeatableOperation(db.Id, db.WalletId, db.OperationType, allTags.Select(i => i.Name).ToList(), db.Amount, db.Comment, db.WalletToId, DateConverter.ToLocalDateTime( db.NextExecution), db.RepeatableType);
    }

    protected override HomeFinance.DataAccess.Core.DBModels.RepeatableOperation ToNewDb(RepeatableOperation domain, string userId)
    {
        var tags = this._tags.Where(i => domain.Tags.Contains(i.Name)).ToList();
        return new HomeFinance.DataAccess.Core.DBModels.RepeatableOperation()
        {
            Id = domain.Id ?? Guid.NewGuid(),
            WalletId = domain.WalletId,
            OperationType = domain.OperationType,
            Tags = tags,
            WalletToId = domain.WalletToId,
            Amount = domain.Amount,
            Comment = domain.Comment,
            NextExecution = DateConverter.ToUtcDateTime( domain.NextExecution),
            RepeatableType = domain.RepeatableType,
            HomeFinanceUserId = userId
        };
    }

    protected override HomeFinance.DataAccess.Core.DBModels.RepeatableOperation ToExistingDb(RepeatableOperation domain)
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
        entity.NextExecution = DateConverter.ToUtcDateTime(domain.NextExecution);
        entity.RepeatableType = domain.RepeatableType;

        return entity;
    }

    protected override Expression<Func<HomeFinance.DataAccess.Core.DBModels.RepeatableOperation, bool>> CheckKey(Guid key)
    {
        Expression<Func<HomeFinance.DataAccess.Core.DBModels.RepeatableOperation, bool>> exp = db => db.Id == key;
        return exp;
    }
}