using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeFinance.Domain.Models;

namespace HomeFinanace.DataAccess.Core.DBModels;

public class Wallet : UserDependentBase
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; }

    public string? GroupName { get; set; } = null;

    public string? Comment { get; set; } = null;
}