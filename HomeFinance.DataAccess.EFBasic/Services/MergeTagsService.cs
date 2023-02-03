using HomeFinance.Domain.DomainModels;
using HomeFinance.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tag = HomeFinance.Domain.DomainModels.Tag;
using TagDB = HomeFinanace.DataAccess.Core.DBModels.Tag;
using OperationBaseDB = HomeFinanace.DataAccess.Core.DBModels.OperationBase;

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


    public async Task<Tag> MergeTags(string newName, IEnumerable<string> oldNames)
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
                SortId = tags.Min(i => i.SortId),
                HomeFinanceUserId = _userId
            };
            newTag = (await _homeFinanceContext.Tags.AddAsync(newTag)).Entity;
        }


        operations
            .Where(i => i.Tags.All(i => i.Name != newName))
            .ToList()
            .ForEach(i => { i.Tags.Add(newTag); });

        _homeFinanceContext.SaveChanges();

        return new Tag(newTag.Name, newTag.OperationType, newTag.SortId);

    }
}