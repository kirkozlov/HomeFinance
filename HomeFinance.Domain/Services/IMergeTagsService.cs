using HomeFinance.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeFinance.Domain.Services;
public interface IMergeTagsService
{
    Task<Tag> MergeTags(string newName, IEnumerable<string> oldNames);
}

