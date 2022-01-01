using HomeFinance.Domain.Models;

namespace HomeFinance.Domain.Dtos
{
    public record WalletDto(int? Id, string Name, string? GroupName,string? Comment)
    {
        public WalletDto(Wallet wallet):this(wallet.Id,wallet.Name,wallet.GroupName,wallet.Comment)
        {
        }
    }

}
