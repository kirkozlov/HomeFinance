using System.ComponentModel.DataAnnotations;

namespace HomeFinance.DataAccess.Core.DBModels;

public class Operation : OperationBase
{
    [Required]
    public DateTime DateTime { get; set; }
}