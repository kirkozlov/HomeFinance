using HomeFinance.Domain.DomainModels;
using HomeFinance.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Expressions;
using Tag = HomeFinance.Domain.DomainModels.Tag;
using TagDB = HomeFinanace.DataAccess.Core.DBModels.Tag;

namespace HomeFinance.DataAccess.EFBasic.Repositories;

class TagRepository : UserDependentRepository<Tag, TagDB, string>
{
    public TagRepository(HomeFinanceContextBase homeFinanceContext): base(homeFinanceContext, homeFinanceContext.Tags)
    {
    }

    protected override Tag ToDomain(TagDB db)
    {
        return new Tag(db.Name, db.Comment);
    }

    protected override TagDB ToDb(Tag domain, string userId)
    {
        return new TagDB()
        {
            Name = domain.Name,
            Comment = domain.Comment,
            HomeFinanceUserId = userId
        };
    }

    protected override Expression<Func<HomeFinanace.DataAccess.Core.DBModels.Tag, bool>> CheckKey(string key)
    {
        Expression<Func<HomeFinanace.DataAccess.Core.DBModels.Tag, bool>> exp = db => db.Name == key;
        return exp;
    }
}