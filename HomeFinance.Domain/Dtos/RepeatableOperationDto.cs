using HomeFinance.Domain.Enums;
using HomeFinance.Domain.Models;

namespace HomeFinance.Domain.Dtos
{
    public record RepeatableOperationDto(int? Id, int WalletId, OperationType OperationType, int? CategoryId, int? WalletIdTo, double Amount, string? Comment, DateTime NextExecution, RepeatableType RepeatableType) : OperationDtoBase(Id, WalletId, OperationType, CategoryId, WalletIdTo, Amount, Comment)
    {
        public RepeatableOperationDto(RepeatableOperation ro)
                           : this(ro.Id,
                                  ro.WalletId,
                                  ro.OperationType,
                                  ro.CategoryId,
                                  ro.WalletIdTo,
                                  ro.Amount,
                                  ro.Comment,
                                  ro.NextExecution,
                                  ro.RepeatableType)
        {
        }

    }


}
