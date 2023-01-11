using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;
using HomeFinance.Domain.Enums;
using HomeFinance.Domain.Models;

namespace HomeFinanace.DataAccess.Core.DBModels;

public class Tag: UserDependentBase
{
    [Key]
    public string Name { get; set; }

    public virtual ICollection<Operation> Operations { get; set; }
    public virtual ICollection<RepeatableOperation> RepeatableOperation { get; set; }

    public OperationType OperationType { get; set; }

    public int SortId { get; set; }
}