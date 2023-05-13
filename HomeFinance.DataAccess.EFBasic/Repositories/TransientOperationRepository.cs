using System.Linq.Expressions;
using HomeFinance.Domain.DomainModels;

namespace HomeFinance.DataAccess.EFBasic.Repositories;

class TransientOperationRepository : UserDependentCollectionRepository<TransientOperation, HomeFinance.DataAccess.Core.DBModels.TransientOperation, Guid>
{
    public TransientOperationRepository(HomeFinanceContextBase homeFinanceContext, string userId) : base(homeFinanceContext, homeFinanceContext.TransientOperations, userId)
    {
    }

    protected override TransientOperation ToDomain(HomeFinance.DataAccess.Core.DBModels.TransientOperation db)
    {

        return new TransientOperation(db.Id, db.WalletId, db.OperationType, db.Amount, db.Description, db.DateTime);
    }

    protected override HomeFinance.DataAccess.Core.DBModels.TransientOperation ToNewDb(TransientOperation domain, string userId)
    {
        return new HomeFinance.DataAccess.Core.DBModels.TransientOperation()
        {
            Id = domain.Id ?? Guid.NewGuid(),
            WalletId = domain.WalletId,
            OperationType = domain.OperationType,
            Amount = domain.Amount,
            Description = domain.Description,
            DateTime = domain.DateTime,
            HomeFinanceUserId = userId
        };
    }

    protected override HomeFinance.DataAccess.Core.DBModels.TransientOperation ToExistingDb(TransientOperation domain)
    {
        var entity = this.DataSet.Single(i => i.Id == domain.Id);
       
        entity.WalletId = domain.WalletId;
        entity.OperationType = domain.OperationType;
        entity.Amount = domain.Amount;
        entity.Description = domain.Description;
        entity.DateTime = domain.DateTime;

        return entity;
    }


    protected override Expression<Func<HomeFinance.DataAccess.Core.DBModels.TransientOperation, bool>> CheckKey(Guid key)
    {
        Expression<Func<HomeFinance.DataAccess.Core.DBModels.TransientOperation, bool>> exp = db => db.Id == key;
        return exp;
    }

}