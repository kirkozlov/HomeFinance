using HomeFinance.Domain.Dtos;
using HomeFinance.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace HomeFinance.Models
{
    public class WalletViewModel
    {
        public int? Id { get; set; } = null;

        [Display(Name = "Name", ResourceType = typeof(Domain.Localization.Common))]
        public string Name { get; set; }

        public string? GroupName { get; set; } = null;

        public double Balance { get; set; } = 0;

        public string? Comment { get; set; } = null;

        public WalletViewModel(WalletDto wallet)
        {
            Id= wallet.Id;
            Name= wallet.Name;
            GroupName= wallet.GroupName;
            Comment= wallet.Comment;
        }

        public WalletDto ToDto()
        {
            return new WalletDto(Id,Name, GroupName, Comment);
        }

        // Needed For View
        public WalletViewModel()
        {

        }


    }

    public class WalletOperationsViewModel: WalletViewModel
    {
        public MonthViewModel MonthViewModel { get; set; }

        public WalletOperationsViewModel(WalletDto wallet):base(wallet)
        {
           
        }
    }
}
