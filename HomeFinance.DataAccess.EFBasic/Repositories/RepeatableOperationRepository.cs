using HomeFinance.Domain.DomainModels;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Tag = HomeFinanace.DataAccess.Core.DBModels.Tag;

namespace HomeFinance.DataAccess.EFBasic.Repositories;

class RepeatableOperationRepository : UserDependentRepository<RepeatableOperation, HomeFinanace.DataAccess.Core.DBModels.RepeatableOperation, Guid>
{
    readonly DbSet<Tag> _tags;
    public RepeatableOperationRepository(HomeFinanceContextBase homeFinanceContext, string userId) : base(homeFinanceContext, homeFinanceContext.RepeatableOperations, userId)
    {
        this._tags = homeFinanceContext.Tags;
    }

    protected override RepeatableOperation ToDomain(HomeFinanace.DataAccess.Core.DBModels.RepeatableOperation db)
    {

        return new RepeatableOperation(db.Id, db.WalletId, db.OperationType, db.Tags.Select(i => i.Name).ToList(), db.Amount, db.Comment, db.WalletIdTo, db.NextExecution, db.RepeatableType);
    }

    protected override HomeFinanace.DataAccess.Core.DBModels.RepeatableOperation ToDb(RepeatableOperation domain, string userId)
    {
        var tags = this._tags.Where(i => domain.Tags.Contains(i.Name)).ToList();
        return new HomeFinanace.DataAccess.Core.DBModels.RepeatableOperation()
        {
            Id = domain.Id ?? Guid.NewGuid(),
            WalletId = domain.WalletId,
            OperationType = domain.OperationType,
            Tags = tags,
            WalletIdTo = domain.WalletIdTo,
            Amount = domain.Amount,
            Comment = domain.Comment,
            NextExecution = domain.NextExecution,
            RepeatableType = domain.RepeatableType,
            HomeFinanceUserId = userId
        };
    }

    protected override Expression<Func<HomeFinanace.DataAccess.Core.DBModels.RepeatableOperation, bool>> CheckKey(Guid key)
    {
        Expression<Func<HomeFinanace.DataAccess.Core.DBModels.RepeatableOperation, bool>> exp = db => db.Id == key;
        return exp;
    }
}