﻿using System.ComponentModel.DataAnnotations;
using HomeFinance.Domain.Enums;
using HomeFinance.Domain.Models;

namespace HomeFinanace.DataAccess.Core.DBModels;

public abstract class OperationBase : UserDependentBase
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid WalletId { get; set; }

    public virtual Wallet Wallet { get; set; }

    [Required]
    public OperationType OperationType { get; set; }
    
    public virtual ICollection<Tag> Tags { get; set; }
    
    public Guid? WalletIdTo { get; set; }
    //[ForeignKey(nameof(WalletIdTo))]
    public virtual Wallet? WalletTo { get; set; }
    
    [Required]
    public double Amount { get; set; }

    public string Comment { get; set; }="";

    public virtual bool IsValid()
    {
        if (this.Amount <= 0) return false;
        if (this.WalletId == this.WalletIdTo) return false;
        return (this.OperationType == OperationType.Transfer && this.WalletIdTo != null)
               || (this.OperationType != OperationType.Transfer && this.WalletIdTo == null);
    }

}