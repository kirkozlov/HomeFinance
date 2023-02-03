using HomeFinance.Domain.Enums;

namespace HomeFinanace.DataAccess.Core.DBModels;

public class Tag: UserDependentBase
{
 
    public string Name { get; set; }

    public virtual ICollection<Operation> Operations { get; set; }
    public virtual ICollection<RepeatableOperation> RepeatableOperation { get; set; }
    
    public OperationType OperationType { get; set; }

    public int SortId { get; set; }
}