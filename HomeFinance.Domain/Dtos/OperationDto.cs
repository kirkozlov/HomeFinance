using HomeFinance.Domain.Models;

namespace HomeFinance.Domain.Dtos
{
    public record OperationDto(int? Id, int WalletId, int CategoryId, DateTime DateTime, bool Outgo, double Amount, string? Comment)
    {
        public OperationDto(Operation operation):this(operation.Id,operation.WalletId,operation.CategoryId,operation.DateTime,operation.Outgo,operation.Amount,operation.Comment)
        {
        }
    }



    public record RepeatableOperationDto(int? Id, int WalletId, OperationType OperationType, int? CategoryId, int? WalletIdTo, DateTime NextExecution, RepeatableType RepeatableType, double Amount, string? Comment)
    {
        public RepeatableOperationDto(RepeatableOperation ro) : this(ro.Id, ro.WalletId, ro.OperationType, ro.CategoryId, ro.WalletIdTo, ro.NextExecution, ro.RepeatableType, ro.Amount, ro.Comment)
        {
        }
    }

}
