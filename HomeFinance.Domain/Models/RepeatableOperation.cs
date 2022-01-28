using HomeFinance.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeFinance.Domain.Models
{
  
    public class RepeatableOperation:OperationBase
    {
        [Required]
        public DateTime NextExecution { get; set; }

        [Required]
        public RepeatableType RepeatableType { get; set; }
    }
}
