using HomeFinance.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace HomeFinance.DataAccess.Core.DBModels;

public abstract class UserDependentBase
{
    [Required]
    public string HomeFinanceUserId { get; set; }

    public virtual HomeFinanceUser HomeFinanceUser { get; set; }
}