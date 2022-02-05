using HomeFinance.Domain.Dtos;

namespace HomeFinanceApi.Requests
{
    public class WalletRequest
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string? GroupName { get; set; }
        public string? Comment { get; set; }


        public WalletDto ToDto()
        {
            return new WalletDto(Id, Name, GroupName, Comment);
        }
    }
}
