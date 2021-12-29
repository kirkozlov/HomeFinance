using HomeFinance.Domain.Models;

namespace HomeFinance.Domain.Dtos
{
    public class WalletDto
    {
        public int? Id { get;  }=null;
        public string Name { get;  }
        public string? GroupName { get; } = null;

        public string? Comment { get;  } = null;

        public WalletDto(Wallet wallet)
        {
            Id = wallet.Id;
            Name = wallet.Name;
            GroupName = wallet.GroupName;
            Comment = wallet.Comment;
        }

        public WalletDto(int? id, string name, string? groupName, string? comment)
        {
            Id = id;
            Name = name;
            GroupName = groupName;
            Comment = comment;
        }
    }
}
