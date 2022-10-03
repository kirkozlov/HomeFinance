using System.ComponentModel.DataAnnotations;

namespace HomeFinanace.DataAccess.Core.DBModels;

public class Operation : OperationBase
{
    [Required]
    public DateTime DateTime { get; set; }
}