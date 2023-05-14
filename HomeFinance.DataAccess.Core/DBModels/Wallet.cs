using System.ComponentModel.DataAnnotations;

namespace HomeFinance.DataAccess.Core.DBModels;

public class Wallet : UserDependentBase
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; }

    public string GroupName { get; set; } = "";

    public string Comment { get; set; } = "";
}
