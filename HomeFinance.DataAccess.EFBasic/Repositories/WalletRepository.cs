using HomeFinance.Domain.DomainModels;
using System.Linq.Expressions;

namespace HomeFinance.DataAccess.EFBasic.Repositories;

class WalletRepository : UserDependentRepository<Wallet, HomeFinance.DataAccess.Core.DBModels.Wallet, Guid>
{
    public WalletRepository(HomeFinanceContextBase homeFinanceContext, string userId) : base(homeFinanceContext, homeFinanceContext.Wallets, userId)
    {
    }

    protected override Wallet ToDomain(HomeFinance.DataAccess.Core.DBModels.Wallet db)
    {
        return new Wallet(db.Id, db.Name, db.GroupName, db.Comment);
    }

    protected override HomeFinance.DataAccess.Core.DBModels.Wallet ToNewDb(Wallet domain, string userId)
    {
        return new HomeFinance.DataAccess.Core.DBModels.Wallet()
        {
            Id = domain.Id ?? Guid.NewGuid(),
            Name = domain.Name,
            GroupName = domain.GroupName,
            Comment = domain.Comment,
            HomeFinanceUserId = userId
        };
    }

    protected override HomeFinance.DataAccess.Core.DBModels.Wallet ToExistingDb(Wallet domain)
    {
        var entity = this.DataSet.Single(i => i.Id == domain.Id);

        entity.Name = domain.Name;
        entity.GroupName = domain.GroupName;
        entity.Comment = domain.Comment;

        return entity;
    }

    protected override Expression<Func<HomeFinance.DataAccess.Core.DBModels.Wallet, bool>> CheckKey(Guid key)
    {
        Expression<Func<HomeFinance.DataAccess.Core.DBModels.Wallet, bool>> exp = db => db.Id == key;
        return exp;
    }
}