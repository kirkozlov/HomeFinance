using HomeFinance.Domain.Dtos;

namespace HomeFinance.Models
{
    public class OperationViewModel
    {
        public int Id { get; set; }
        public bool IsTransfer { get; set; }

        public string Wallet { get; set; }

        public string Category { get; set; }

        public DateTime DateTime { get; set; }

        public double? Income { get; set; }
        public double? Transfer { get; set; }
        public double? Outgo { get; set; }

        public double? Balance { get; set; }

        public string Comment { get; set; }
    }

    public class DayViewModel
    {
        public DateTime Day { get; set; }
        public double Income { get; set; }
        public double Outgo { get; set; }
        public IEnumerable<OperationViewModel> Operations { get; set; }


        public DayViewModel(DateTime day,IEnumerable<OperationViewModel> operations)
        {
            Day= day;

            if (operations.Any(i => i.DateTime.Date != day))
                throw new Exception();

            Income = operations.Sum(i => i.Income ?? 0);
            Outgo = operations.Sum(i => i.Outgo ?? 0);
            Operations=operations.OrderByDescending(i => i.DateTime).ToList();
        }
    }

    public class MonthViewModel
    {
        public DateTime Month { get; set; }
        public double MonthBegin { get; set; }
        public double MonthDiff { get; set; }
        public double MonthEnd { get; set; }
        public IEnumerable<DayViewModel> Days { get; set; }
    }





    public class AddEditIncomeOutgoOperationViewModel
    {
        public int? Id { get; set; }

        public int WalletId { get; set; }

        public int CategoryId { get; set; }

        public DateTime DateTime { get; set; }

        public bool Outgo { get; set; }

        public double Amount { get; set; }

        public string? Comment { get; set; }

        public bool NavigateToWallet { get; set; }

        public AddEditIncomeOutgoOperationViewModel(OperationDto operation)
        {
            Id=operation.Id;
            WalletId=operation.WalletId;
            CategoryId=operation.CategoryId;
            DateTime = operation.DateTime;
            Outgo=operation.Outgo;
            Amount = operation.Amount;
            Comment = operation.Comment;
        }

        public AddEditIncomeOutgoOperationViewModel()
        {
        }

        public OperationDto ToDto()
        {
            return new OperationDto(Id, WalletId, CategoryId, DateTime, Outgo, Amount, Comment);
        }
    }

    public class AddEditTransferViewModel
    {
        public int? Id { get; set; }

        public int WalletIdFrom { get; set; }

        public int WalletIdTo { get; set; }

        public DateTime DateTime { get; set; }

        public double Amount { get; set; }

        public string? Comment { get; set; }

        public bool NavigateToWallet { get; set; }

        public AddEditTransferViewModel(TransferDto transfer)
        {
            Id = transfer.Id;
            WalletIdFrom = transfer.WalletIdFrom;
            WalletIdTo = transfer.WalletIdTo;
            DateTime = transfer.DateTime;
            Amount = transfer.Amount;
            Comment = transfer.Comment;            
        }

        public AddEditTransferViewModel()
        {

        }

        public TransferDto ToDto()
        {
            return new TransferDto(Id, WalletIdFrom, WalletIdTo, DateTime, Amount, Comment);
        }
    }

    public class AddEditOperationViewModel
    {

        public AddEditIncomeOutgoOperationViewModel Operation { get; set; }
        public AddEditTransferViewModel Transfer { get; set; }

        public IEnumerable<WalletViewModel>? PossibleWallets { get; set; }
        public IEnumerable<CategoryViewModel>? IncomeCategories { get; set; }
        public IEnumerable<CategoryViewModel>? OutgoCategories { get; set; }
        
    }





}
