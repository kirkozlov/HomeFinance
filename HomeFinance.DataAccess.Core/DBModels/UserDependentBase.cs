using HomeFinance.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace HomeFinanace.DataAccess.Core.DBModels;

public abstract class UserDependentBase
{
    [Required]
    public string HomeFinanceUserId { get; set; }

    public virtual HomeFinanceUser HomeFinanceUser { get; set; }
}