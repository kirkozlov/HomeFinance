using HomeFinance.Domain.Dtos;

namespace HomeFinance.Models
{
    public class OperationViewModel
    {
        public int? Id { get; set; }

        public int WalletId { get; set; }

        public int? CategoryId { get; set; }

        public DateTime DateTime { get; set; }

        public bool Outgo { get; set; }

        public double Amount { get; set; }

        public string? Comment { get; set; }


        public OperationViewModel(OperationDto operation)
        {
            Id = operation.Id;
            WalletId = operation.WalletId;
            CategoryId = operation.CategoryId;
            DateTime = operation.DateTime;
            Outgo = operation.Outgo;
            Amount = operation.Amount;
            Comment = operation.Comment;
        }
        public OperationViewModel()
        {
        }
    }

    public class AddEditOperationViewModel: OperationViewModel
    {
        public IEnumerable<WalletViewModel>? PossibleWallets { get; set; }
        public IEnumerable<CategoryViewModel>? PossibleCategories { get; set; }

        public AddEditOperationViewModel(OperationDto operation):base(operation)
        {
        }

        public AddEditOperationViewModel()
        {
        }

        public OperationDto ToDto()
        {
            return new OperationDto(Id, WalletId, CategoryId, DateTime, Outgo, Amount, Comment);
        }
    }

    public class OperationsOverviewViewModel
    {
        public DateTime Month { get; set; }
        public IEnumerable<OperationViewModel> RelevantOperations { get; set; }
        public double MonthBegin { get; set; }
        public double MonthDiff { get; set; }
        public double MonthEnd { get; set; }

        public IEnumerable<WalletViewModel> AllWallets { get; set; }
        public IEnumerable<CategoryViewModel> AllCategories { get; set; }
    }
}
