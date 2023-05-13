using HomeFinance.Domain.DomainModels;

namespace HomeFinance.DataAccess.EFBasic.Repositories;

class UserPreferencesRepository : UserDependentRepository<UserPreferences, HomeFinance.DataAccess.Core.DBModels.UserPreferences>
{

    public UserPreferencesRepository(HomeFinanceContextBase homeFinanceContext, string userId) : base(homeFinanceContext, homeFinanceContext.UserPreferences, userId)
    {
    }

    protected override UserPreferences ToDomain(Core.DBModels.UserPreferences db)
    {
        return new UserPreferences(TimeZoneInfo.FindSystemTimeZoneById(db.TimeZoneId));
    }

    protected override Core.DBModels.UserPreferences ToExistingDb(UserPreferences domain)
    {
        var entity = this.DataSet.Single();
        entity.TimeZoneId = domain.TimeZone.Id;

        return entity;
    }

    protected override Core.DBModels.UserPreferences ToNewDb(UserPreferences domain, string userId)
    {
        return new HomeFinance.DataAccess.Core.DBModels.UserPreferences()
        {
            TimeZoneId=domain.TimeZone.Id,
            HomeFinanceUserId = userId
        };
    }
}