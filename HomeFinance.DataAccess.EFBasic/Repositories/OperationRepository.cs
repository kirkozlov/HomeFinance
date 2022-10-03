using HomeFinance.Domain.DomainModels;
using HomeFinance.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Tag = HomeFinanace.DataAccess.Core.DBModels.Tag;

namespace HomeFinance.DataAccess.EFBasic.Repositories;

class OperationRepository : UserDependentRepository<Operation, HomeFinanace.DataAccess.Core.DBModels.Operation, Guid>, IOperationRepository
{
    readonly DbSet<Tag> _tags;
    public OperationRepository(HomeFinanceContextBase homeFinanceContext) : base(homeFinanceContext, homeFinanceContext.Operations)
    {
        this._tags = homeFinanceContext.Tags;
    }

    protected override Operation ToDomain(HomeFinanace.DataAccess.Core.DBModels.Operation db)
    {

        return new Operation(db.Id, db.WalletId, db.OperationType, db.Tags.Select(i=>i.Name).ToList(),db.Amount, db.Comment, db.WalletIdTo, db.DateTime);
    }

    protected override HomeFinanace.DataAccess.Core.DBModels.Operation ToDb(Operation domain, string userId)
    {
        var tags = this._tags.Where(i => domain.Tags.Contains(i.Name)).ToList();
        return new HomeFinanace.DataAccess.Core.DBModels.Operation()
        {
            Id = domain.Id ?? Guid.NewGuid(),
            WalletId = domain.WalletId,
            OperationType = domain.OperationType,
            Tags = tags,
            WalletIdTo = domain.WalletIdTo,
            Amount = domain.Amount,
            Comment = domain.Comment,
            DateTime = domain.DateTime,
            HomeFinanceUserId = userId
        };
    }

    
    protected override Expression<Func<HomeFinanace.DataAccess.Core.DBModels.Operation, bool>> CheckKey( Guid key)
    {
        Expression<Func<HomeFinanace.DataAccess.Core.DBModels.Operation, bool>> exp = db => db.Id == key;
        return exp;
    }

    public async Task<List<Operation>> GetForWallet(string userId, Guid walletId)
    {
        return (await GetAll(userId)).Where(i => i.WalletId == walletId || i.WalletIdTo==walletId).ToList();
    }
}