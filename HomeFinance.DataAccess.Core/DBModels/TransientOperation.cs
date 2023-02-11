using System.ComponentModel.DataAnnotations;
using HomeFinance.Domain.Enums;

namespace HomeFinance.DataAccess.Core.DBModels;

public class TransientOperation : UserDependentBase
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid WalletId { get; set; }

    public virtual Wallet Wallet { get; set; }
    

    [Required]
    public OperationType OperationType { get; set; }

    [Required]
    public double Amount { get; set; }

    public string Description { get; set; } = "";

    [Required]
    public DateTime DateTime { get; set; }
}