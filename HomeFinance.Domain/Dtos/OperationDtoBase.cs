using HomeFinance.Domain.Enums;
using HomeFinance.Domain.Models;

namespace HomeFinance.Domain.Dtos
{
    public abstract record OperationDtoBase(int? Id, int WalletId, OperationType OperationType, int? CategoryId, int? WalletIdTo, double Amount, string? Comment)
    {
        public OperationDtoBase(OperationBase operationBase)
                           : this(operationBase.Id,
                                  operationBase.WalletId,
                                  operationBase.OperationType,
                                  operationBase.CategoryId,
                                  operationBase.WalletIdTo,
                                  operationBase.Amount,
                                  operationBase.Comment)
        {
        }
    }


}
