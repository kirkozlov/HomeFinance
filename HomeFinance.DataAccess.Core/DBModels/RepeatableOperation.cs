using System.ComponentModel.DataAnnotations;
using HomeFinance.Domain.Enums;

namespace HomeFinanace.DataAccess.Core.DBModels;

public class RepeatableOperation : OperationBase
{
    [Required]
    public DateTime NextExecution { get; set; }

    [Required]
    public RepeatableType RepeatableType { get; set; }
}