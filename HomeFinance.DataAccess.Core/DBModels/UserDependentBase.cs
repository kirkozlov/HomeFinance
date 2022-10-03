using HomeFinance.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeFinanace.DataAccess.Core.DBModels;

public abstract class UserDependentBase
{
    [Required]
    public string HomeFinanceUserId { get; set; }

    public virtual HomeFinanceUser HomeFinanceUser { get; set; }
}