using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HomeFinance.DataAccess.Core.DBModels;

public class UserPreferences: UserDependentBase
{
    // FakeKey, easy workaround
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int FakeKey { get; set; }

    public string TimeZoneId { get; set; }
}