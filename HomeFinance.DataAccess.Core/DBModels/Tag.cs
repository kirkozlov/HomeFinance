using System.ComponentModel.DataAnnotations;
using HomeFinance.Domain.Models;

namespace HomeFinanace.DataAccess.Core.DBModels;

public class Tag: UserDependentBase
{
    [Key]
    public string Name { get; set; }
    
    public ICollection<Operation> Operations { get; set; }
    public ICollection<RepeatableOperation> RepeatableOperation { get; set; }

    public string Comment { get; set; } ="";
}