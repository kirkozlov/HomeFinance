using HomeFinance.Domain.DomainModels;
using System.Linq.Expressions;

namespace HomeFinance.DataAccess.EFBasic.Repositories;

class WalletRepository : UserDependentRepository<Wallet, HomeFinanace.DataAccess.Core.DBModels.Wallet, Guid>
{
    public WalletRepository(HomeFinanceContextBase homeFinanceContext, string userId) : base(homeFinanceContext, homeFinanceContext.Wallets, userId)
    {
    }

    protected override Wallet ToDomain(HomeFinanace.DataAccess.Core.DBModels.Wallet db)
    {
        return new Wallet(db.Id, db.Name, db.GroupName, db.Comment);
    }

    protected override HomeFinanace.DataAccess.Core.DBModels.Wallet ToDb(Wallet domain, string userId)
    {
        return new HomeFinanace.DataAccess.Core.DBModels.Wallet()
        {
            Id = domain.Id ?? Guid.NewGuid(),
            Name = domain.Name,
            GroupName = domain.GroupName,
            Comment = domain.Comment,
            HomeFinanceUserId = userId
        };
    }

    protected override Expression<Func<HomeFinanace.DataAccess.Core.DBModels.Wallet, bool>> CheckKey(Guid key)
    {
        Expression<Func<HomeFinanace.DataAccess.Core.DBModels.Wallet, bool>> exp = db => db.Id == key;
        return exp;
    }
}