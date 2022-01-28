using System.ComponentModel.DataAnnotations;

namespace HomeFinance.Domain.Models
{
    public class Operation: OperationBase
    {
        [Required]
        public DateTime DateTime { get; set; }
    }
}
