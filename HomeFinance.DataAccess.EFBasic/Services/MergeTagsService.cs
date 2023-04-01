using HomeFinance.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Tag = HomeFinance.Domain.DomainModels.Tag;
using TagDB = HomeFinance.DataAccess.Core.DBModels.Tag;
using OperationBaseDB = HomeFinance.DataAccess.Core.DBModels.OperationBase;

namespace HomeFinance.DataAccess.EFBasic.Services;

class MergeTagsService : IMergeTagsService
{
    readonly string _userId;
    readonly HomeFinanceContextBase _homeFinanceContext;


    public MergeTagsService(HomeFinanceContextBase homeFinanceContext, string userId)
    {
        _homeFinanceContext = homeFinanceContext;
        _userId = userId;
    }


    public async Task<Tag> MergeTags(string newName, IEnumerable<string> oldNames, string ParentTagName)
    {
        var tags = await _homeFinanceContext.Tags.Where(i => oldNames.Contains(i.Name)).ToListAsync();
        var newTag = tags.SingleOrDefault(i => i.Name == newName);


        var oldTags = tags.Where(i => i.Name != newName);


        var operations = oldTags
            .SelectMany(i => i.Operations)
            .Cast<OperationBaseDB>()
            .Concat(oldTags
                .SelectMany(i => i.RepeatableOperation)
                .Cast<OperationBaseDB>())
            .Distinct().ToList();

        _homeFinanceContext.Tags.RemoveRange(oldTags);


        if (newTag == null)
        {
            var operationType = tags.Select(i => i.OperationType).Distinct().Single();
            newTag = new TagDB()
            {
                Name = newName,
                OperationType = operationType,
                ParentTagName=ParentTagName,
                SortId = tags.Min(i => i.SortId),
                HomeFinanceUserId = _userId
            };
            newTag = (await _homeFinanceContext.Tags.AddAsync(newTag)).Entity;
        }
        else
        {
            newTag.ParentTagName = ParentTagName;
        }


        operations
            .Where(i => i.Tags.All(i => i.Name != newName))
            .ToList()
            .ForEach(i => { i.Tags.Add(newTag); });

        _homeFinanceContext.SaveChanges();

        return new Tag(newTag.Name, newTag.OperationType, newTag.ParentTagName??string.Empty, newTag.SortId);

    }
}