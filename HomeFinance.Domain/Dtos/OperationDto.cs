using HomeFinance.Domain.Enums;
using HomeFinance.Domain.Models;

namespace HomeFinance.Domain.Dtos
{

    public record OperationDto(int? Id, int WalletId, OperationType OperationType, int? CategoryId, int? WalletIdTo, double Amount, string? Comment, DateTime DateTime) : OperationDtoBase(Id,WalletId, OperationType, CategoryId, WalletIdTo, Amount, Comment)
    {
        public OperationDto(Operation operation) 
                           : this(operation.Id,
                                  operation.WalletId,
                                  operation.OperationType,
                                  operation.CategoryId,
                                  operation.WalletIdTo,
                                  operation.Amount,
                                  operation.Comment,
                                  operation.DateTime)
        {
        }

    }


}
