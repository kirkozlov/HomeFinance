using HomeFinance.Domain.Models;

namespace HomeFinance.Domain.Dtos
{
    public record OperationDto(int? Id, int WalletId, int CategoryId, DateTime DateTime, bool Outgo, double Amount, string? Comment)
    {
        public OperationDto(Operation operation):this(operation.Id,operation.WalletId,operation.CategoryId,operation.DateTime,operation.Outgo,operation.Amount,operation.Comment)
        {
        }
    }

}
