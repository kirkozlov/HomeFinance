using System.Linq.Expressions;
using Tag = HomeFinance.Domain.DomainModels.Tag;
using TagDB = HomeFinanace.DataAccess.Core.DBModels.Tag;

namespace HomeFinance.DataAccess.EFBasic.Repositories;

class TagRepository : UserDependentRepository<Tag, TagDB, string>
{
    public TagRepository(HomeFinanceContextBase homeFinanceContext, string userId) : base(homeFinanceContext, homeFinanceContext.Tags, userId)
    {
    }

    protected override Tag ToDomain(TagDB db)
    {
        return new Tag(db.Name, db.OperationType, db.SortId);
    }

    protected override TagDB ToNewDb(Tag domain, string userId)
    {
        return new TagDB()
        {
            Name = domain.Name,
            OperationType = domain.OperationType,
            SortId = domain.SortId,
            HomeFinanceUserId = userId
        };
    }

    protected override TagDB ToExistingDb(Tag domain)
    {
        var entity = this.DataSet.Single(i => i.Name == domain.Name && i.OperationType==domain.OperationType );
        entity.OperationType = domain.OperationType;
        entity.SortId = domain.SortId;
        return entity;
    }

    protected override Expression<Func<HomeFinanace.DataAccess.Core.DBModels.Tag, bool>> CheckKey(string key)
    {
        Expression<Func<HomeFinanace.DataAccess.Core.DBModels.Tag, bool>> exp = db => db.Name == key;
        return exp;
    }

  
}