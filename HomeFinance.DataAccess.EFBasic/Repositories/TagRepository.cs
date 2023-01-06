using HomeFinance.Domain.DomainModels;
using HomeFinance.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        return new Tag(db.Name, db.Comment);
    }

    protected override TagDB ToNewDb(Tag domain, string userId)
    {
        return new TagDB()
        {
            Name = domain.Name,
            Comment = domain.Comment,
            HomeFinanceUserId = userId
        };
    }

    protected override TagDB ToExistingDb(Tag domain, string userId)
    {
        var entity = this.DbSet.Where(i => i.Name == domain.Name && i.HomeFinanceUserId == userId).Single();
        entity.Comment = domain.Comment;
        return entity;
    }

    protected override Expression<Func<HomeFinanace.DataAccess.Core.DBModels.Tag, bool>> CheckKey(string key)
    {
        Expression<Func<HomeFinanace.DataAccess.Core.DBModels.Tag, bool>> exp = db => db.Name == key;
        return exp;
    }

  
}