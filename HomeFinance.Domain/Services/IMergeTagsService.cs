using HomeFinance.Domain.DomainModels;

namespace HomeFinance.Domain.Services;
public interface IMergeTagsService
{
    Task<Tag> MergeTags(string newName, IEnumerable<string> oldNames);
}