using HomeFinance.Domain.Models;

namespace HomeFinance.Domain.Dtos
{
    public record TransferDto(int? Id, int WalletIdFrom, int WalletIdTo, DateTime DateTime, double Amount, string? Comment)
    {
        public TransferDto(Transfer transfer) : this(transfer.Id, transfer.WalletIdFrom, transfer.WalletIdTo, transfer.DateTime, transfer.Amount, transfer.Comment)
        {
        }
    }

}
